namespace ApiCatalogo.Exceptions;

public class UnauthorizedException : Exception
{
  public UnauthorizedException(string message = "Access denied") : base(message)
  {
    
  }
}
