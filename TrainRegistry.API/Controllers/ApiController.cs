using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace TrainRegistry.API.Controllers
{
    [ApiController]
    public class ApiController: ControllerBase
    {
        protected IActionResult Problem(List<Error> errors)
        {
            var firstError = errors.First();

            var statusCode = firstError.Type switch
            {
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };

            return Problem(statusCode: statusCode, detail: firstError.Description);
        }
    }
}
