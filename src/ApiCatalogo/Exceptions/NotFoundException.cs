namespace ApiCatalogo.Exceptions;

public class NotFoundException : Exception
{

  public NotFoundException(string message = "Item not found") : base(message)
  {
    
  }

}
