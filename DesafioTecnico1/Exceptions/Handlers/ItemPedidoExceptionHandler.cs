using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DesafioTecnico1.Exceptions.Handlers;

public class ItemPedidoExceptionHandler : IExceptionHandler
{
    public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ItemPedidoExceptions)
        {
            return ValueTask.FromResult(false);
        }
        ProblemDetails problemDetails = new ProblemDetails
        {
            Title = "Houve um problema na criação do Item Pedido!",
            Status = StatusCodes.Status400BadRequest,
            Detail = exception.Message
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        httpContext.Response.WriteAsJsonAsync(problemDetails);
        return ValueTask.FromResult(true);
    }
}
