using Application.Abstractions.CQRS;
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

    public UsersController(
        ICommandHandler<RegisterCommand> register
    )
    {
        _register = register;
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

    // Log in
    // Refresh token
    // Change password
    // Reset password
}