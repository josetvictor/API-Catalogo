namespace ApiCatalogo.Exceptions;

public class BadRequestException : Exception
{
  public BadRequestException(string message = "Argument invalid") : base(message)
  {
    
  }
}
