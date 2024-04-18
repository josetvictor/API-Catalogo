using System.Net;
using System.Text.Json;
using ApiCatalogo.Exceptions;

namespace ApiCatalogo.Middleware;

public class GlobalErrorHandlingMiddleware
{
  private readonly RequestDelegate _next;

  public GlobalErrorHandlingMiddleware(RequestDelegate next)
  {
    _next = next;
  }

  public async Task InvokeAsync(HttpContext context)
  {
    try
    {
      await _next(context);

    }
    catch (Exception ex)
    {
      await HandlerExceptionAsync(context, ex);
    }
  }

  private static Task HandlerExceptionAsync(HttpContext context, Exception ex)
  {
    HandlerExceptionDTO handlerException;

    var exceptionType = ex.GetType();

    if (exceptionType == typeof(BadRequestException))
    {
      handlerException = new HandlerExceptionDTO()
      {
        StatusCode = HttpStatusCode.BadRequest,
        Message = ex.Message,
        StackTrace = ex.StackTrace
      };
    }
    else if (exceptionType == typeof(NotFoundException))
    {
      handlerException = new HandlerExceptionDTO()
      {
        StatusCode = HttpStatusCode.NotFound,
        Message = ex.Message,
        StackTrace = ex.StackTrace
      };
    }
    else if (exceptionType == typeof(UnauthorizedException))
    {
      handlerException = new HandlerExceptionDTO()
      {
        StatusCode = HttpStatusCode.Forbidden,
        Message = ex.Message,
        StackTrace = ex.StackTrace
      };
    }
    else
    {
      handlerException = new HandlerExceptionDTO()
      {
        StatusCode = HttpStatusCode.InternalServerError,
        Message = "An error occurred while processing your request",
        StackTrace = ex.StackTrace
      };
    }



    context.Response.ContentType = "application/json";
    context.Response.StatusCode = (int)handlerException.StatusCode;
    return context.Response.WriteAsync(JsonSerializer.Serialize(handlerException));
  }
}