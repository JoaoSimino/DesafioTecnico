using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DesafioTecnico1.Exceptions.Handlers
{
    public class ClienteExceptionHandler : IExceptionHandler
    {
        public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is not ClienteExceptions)
            {
                return ValueTask.FromResult(false);
            }
            ProblemDetails problemDetails = new ProblemDetails
            {
                Title = "Houve um problema na criação do Cliente!",
                Status = StatusCodes.Status400BadRequest,
                Detail = exception.Message
            };

            httpContext.Response.StatusCode = problemDetails.Status.Value;
            httpContext.Response.WriteAsJsonAsync(problemDetails);
            return ValueTask.FromResult(true);
        }
    }
}
