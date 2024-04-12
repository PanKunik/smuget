using Application.Abstractions.Authentication;
using Application.Abstractions.CQRS;
using Application.Exceptions;
using Application.Identity.Login;
using Application.Identity.Logout;
using Application.Identity.Refresh;
using Application.Identity.Register;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Services.Users;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace WebAPI.Identity;

[ApiController]
[Route("api/identity")]
[Consumes("application/json")]
[Produces("application/json")]
public sealed class IdentityController
    : ControllerBase
{
    private readonly ICommandHandler<RegisterCommand> _register;
    private readonly ICommandHandler<LoginCommand> _login;
    private readonly ICommandHandler<RefreshCommand> _refresh;
    private readonly ICommandHandler<LogoutCommand> _logout;
    private readonly ITokenStorage _tokenStorage;
    private readonly IUserService _userService;


    public IdentityController(
        ICommandHandler<RegisterCommand> register,
        ICommandHandler<LoginCommand> login,
        ICommandHandler<RefreshCommand> refresh,
        ICommandHandler<LogoutCommand> logout,
        ITokenStorage tokenStorage,
        IUserService userService
    )
    {
        _register = register
            ?? throw new ArgumentNullException(nameof(register));
        _login = login
            ?? throw new ArgumentNullException(nameof(login));
        _refresh = refresh
            ?? throw new ArgumentNullException(nameof(refresh));
        _logout = logout
            ?? throw new ArgumentNullException(nameof(logout));
        _tokenStorage = tokenStorage
            ?? throw new ArgumentNullException(nameof(tokenStorage));
        _userService = userService
            ?? throw new ArgumentNullException(nameof(userService));
    }

    /// <summary>
    /// Creates a user with passed credentials.
    /// </summary>
    [HttpPost("register")]
    [ProducesResponseType(typeof(NoContentResult), Status201Created)]
    [ProducesResponseType(typeof(Error), Status400BadRequest)]
    [ProducesResponseType(typeof(Error), Status500InternalServerError)]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest request,
        CancellationToken token = default
    )
    {
        var (email, firstName, password) = request;

        await _register.HandleAsync(
            new RegisterCommand(
                email,
                firstName,
                password
            ),
            token
        );

        return Created("", null);
    }

    /// <summary>
    /// Logs in a user with valid credentials.
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(NoContentResult), Status200OK)]
    [ProducesResponseType(typeof(Error), Status400BadRequest)]
    [ProducesResponseType(typeof(Error), Status500InternalServerError)]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        CancellationToken token = default
    )
    {
        var (email, password) = request;

        if (string.IsNullOrWhiteSpace(_userService.IpAddress))
        {
            throw new SourceAddressNotFoundException();
        }

        await _login.HandleAsync(
            new LoginCommand(
                email,
                password,
                _userService.IpAddress
            ),
            token
        );

        var jwtToken = _tokenStorage.Get();
        return Ok(new AuthenticationResponse(
            jwtToken.AccessToken,
            jwtToken.RefreshToken,
            jwtToken.ExpirationDateTime
        ));
    }

    /// <summary>
    /// Refreshes token generating new access token and refresh token for user.
    /// </summary>
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(NoContentResult), Status200OK)]
    [ProducesResponseType(typeof(Error), Status400BadRequest)]
    [ProducesResponseType(typeof(Error), Status500InternalServerError)]
    public async Task<IActionResult> Refresh(
        [FromBody] RefreshRequest request,
        CancellationToken token = default
    )
    {
        var (accessToken, refreshToken) = request;

        if (string.IsNullOrWhiteSpace(_userService.IpAddress))
        {
            throw new SourceAddressNotFoundException();
        }

        await _refresh.HandleAsync(
            new RefreshCommand(
                accessToken,
                refreshToken,
                _userService.IpAddress
            ),
            token
        );

        var jwtToken = _tokenStorage.Get();
        return Ok(new AuthenticationResponse(
            jwtToken.AccessToken,
            jwtToken.RefreshToken,
            jwtToken.ExpirationDateTime
        ));
    }

    /// <summary>
    /// Logs out the currently logged user.
    /// </summary>
    [Authorize]
    [HttpPost("logout")]
    [ProducesResponseType(typeof(NoContentResult), Status200OK)]
    [ProducesResponseType(typeof(Error), Status400BadRequest)]
    [ProducesResponseType(typeof(Error), Status500InternalServerError)]
    public async Task<IActionResult> Logout(
        [FromBody] LogoutRequest request,
        CancellationToken token = default
    )
    {
        var userId = _userService.UserId;

        await _logout.HandleAsync(
            new LogoutCommand(
                request.RefreshToken,
                userId
            ),
            token
        );

        return Ok();
    }

    // Change password
    // Reset password
}