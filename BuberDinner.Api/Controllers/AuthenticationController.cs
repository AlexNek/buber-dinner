using Microsoft.AspNetCore.Mvc;
using BuberDinner.Contracts.Authentication;
using BuberDinner.Domain.Common.Errors;
using MediatR;
using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Authentication.Queries.Login;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;

namespace BuberDinner.Api.Controllers;

[AllowAnonymous]
[Route("authentication")]
public class AuthenticationController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public AuthenticationController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var command = _mapper.Map<RegisterCommand>(request);
        var authenticationResult = await _mediator.Send(command);

        return authenticationResult.Match(
            authenticationResult => Ok(_mapper.Map<AuthenticationResponse>(authenticationResult)),
            errors => Problem(errors));
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var query = _mapper.Map<LoginQuery>(request);
        var authenticationResult = await _mediator.Send(query);

        if(authenticationResult.IsError && authenticationResult.FirstError == Errors.Authentication.InvalidCredentials)
        {
            return Problem(
                statusCode: StatusCodes.Status401Unauthorized,
                title: authenticationResult.FirstError.Description);
        }

        return authenticationResult.Match(
            authenticationResult => Ok(_mapper.Map<AuthenticationResponse>(authenticationResult)),
            errors => Problem(errors));
    }
}
