namespace RPSSL.GameService.Common.Exceptions;

[Serializable]
public class CantResolveUserException : Exception
{
    public CantResolveUserException() : base("Can't resolve user.")
    {
    }

    public CantResolveUserException(string message) : base(message)
    {
    }

    public CantResolveUserException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
