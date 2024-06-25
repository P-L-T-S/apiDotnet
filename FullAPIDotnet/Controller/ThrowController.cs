using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace FullAPIDotnet.Controller;

// configura que o swagger ignore a rota 
[ApiExplorerSettings(IgnoreApi = true)]
public class ThrowController : ControllerBase
{
    [HttpGet]
    [Route("/error")]
    public IActionResult HandleError() => Problem();

    [Route("/error-development")]
    public IActionResult HandleErrorDevelopment(
        [FromServices] IHostEnvironment hostEnvironment
    )
    {
        if (!hostEnvironment.IsDevelopment())
        {
            return NotFound();
        }

        // verifica o erro e tipo de erro
        var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>()!;

        return Problem(
            detail: exceptionHandlerFeature.Error.StackTrace,
            title: exceptionHandlerFeature.Error.Message
        );
    }
}