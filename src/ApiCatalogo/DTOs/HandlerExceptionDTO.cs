using System.Net;

namespace ApiCatalogo;

public class HandlerExceptionDTO
{
  public HttpStatusCode StatusCode { get; set; }
  public string? Message { get; set; }
  public string? StackTrace { get; set; }
}
