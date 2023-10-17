using Application.Abstractions.CQRS;
using Application.Abstractions.Security;
using Application.Identity.Login;
using Application.Identity.Refresh;
using Application.Identity.Register;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace WebAPI.Identity;

[ApiController]
[Route("api/identity")]
[Consumes("application/json")]
[Produces("application/json")]
public sealed class IdentityController : ControllerBase
{
    private readonly ICommandHandler<RegisterCommand> _register;
    private readonly ICommandHandler<LoginCommand> _login;
    private readonly ICommandHandler<RefreshCommand> _refresh;
    private readonly ITokenStorage _tokenStorage;

    public IdentityController(
        ICommandHandler<RegisterCommand> register,
        ICommandHandler<LoginCommand> login,
        ICommandHandler<RefreshCommand> refresh,
        ITokenStorage tokenStorage
    )
    {
        _register = register;
        _login = login;
        _refresh = refresh;
        _tokenStorage = tokenStorage;
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

        await _login.HandleAsync(
            new LoginCommand(
                email,
                password
            ),
            token
        );

        var jwtToken = _tokenStorage.Get();
        return Ok(new AuthenticationResponse(
            jwtToken.AccessToken,
            jwtToken.RefreshToken
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

        await _refresh.HandleAsync(
            new RefreshCommand(
                accessToken,
                refreshToken
            ),
            token
        );

        var jwtToken = _tokenStorage.Get();
        return Ok(new AuthenticationResponse(
            jwtToken.AccessToken,
            jwtToken.RefreshToken
        ));
    }

    // Change password
    // Reset password
}