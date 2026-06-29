using MediatR;
using Microsoft.AspNetCore.Mvc;
using TrainRegistry.Application.Auhentication.Commands.RegisterUser;
using TrainRegistry.Application.Auhentication.Queries.LoginUser;
using RegisterRequest = TrainRegistry.API.DTOs.Requests.RegisterRequest;
using LoginRequest = TrainRegistry.API.DTOs.Requests.LoginRequest;
using TrainRegistry.Application.Common.Enums;

namespace TrainRegistry.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    public class AuthController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IMediator mediator, ILogger<AuthController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Received registration request for user {UserName}", registerRequest.UserName);

            var registerResponse = await _mediator.Send(new RegisterUserCommand(registerRequest.UserName, registerRequest.Password), cancellationToken);

            if (registerResponse.Success)
            {
                return Ok(registerResponse);
            }

            return registerResponse.RegisterUserErrorCode switch
            {
                RegisterUserErrorCode.UserAlreadyExists => Conflict(registerResponse),
                RegisterUserErrorCode.DatabaseError => StatusCode(500, registerResponse),
                _ => BadRequest(registerResponse)
            };
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Received login request for user {Name}", loginRequest.UserName);
            
            var loginResponse = await _mediator.Send(new LoginUserQuery(loginRequest.UserName, loginRequest.Password), cancellationToken);

            if (!loginResponse.Success)
            {
                return loginResponse.LoginErrorCode switch
                {
                    LoginErrorCode.UserNotFound => NotFound(loginResponse),
                    LoginErrorCode.InvalidPassword => Unauthorized(loginResponse),
                    _ => BadRequest(loginResponse)
                };
            }

            _logger.LogInformation("User {Name} logged in successfully", loginRequest.UserName);
            return Ok(loginResponse);
        }
    }
}
