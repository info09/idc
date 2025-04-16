namespace IDC.Domain.Exceptions;

public class IDCException : Exception
{
    public IDCException()
    { }

    public IDCException(string message)
        : base(message)
    { }

    public IDCException(string message, Exception innerException)
        : base(message, innerException)
    { }
}
