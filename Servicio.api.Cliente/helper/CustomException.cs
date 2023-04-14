namespace Servicio.api.Cliente.Helper;

public class CustomException : Exception
{
    public CustomException(string message) : base(message)
    {
    }

    public CustomException(string message, Exception innerException) : base(message, innerException)
    {
    }
}