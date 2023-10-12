using Application.Abstractions.CQRS;
using Application.Abstractions.Security;
using Application.Users.Login;
using Application.Users.Register;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Users;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace WebAPI.Unit.Tests.Users;

[ApiController]
[Route("api/users")]
[Consumes("application/json")]
[Produces("application/json")]
public sealed class UsersController : ControllerBase
{
    private readonly ICommandHandler<RegisterCommand> _register;
    private readonly ICommandHandler<LoginCommand> _login;
    private readonly ITokenStorage _tokenStorage;

    public UsersController(
        ICommandHandler<RegisterCommand> register,
        ICommandHandler<LoginCommand> login,
        ITokenStorage tokenStorage
    )
    {
        _register = register;
        _login = login;
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

        return Ok(_tokenStorage.Get());
    }

    // Refresh token
    // Change password
    // Reset password
}