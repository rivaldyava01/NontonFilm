namespace Zeta.NontonFilm.Application.Common.Exceptions;

public class EarlyDateException : Exception
{
    public EarlyDateException()
        : base()
    {
    }

    public EarlyDateException(string message)
        : base(message)
    {
    }

    public EarlyDateException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public EarlyDateException(string entityName, object key, string entityName2)
        : base($"{entityName} with ShowDate [{key}] cannot be earlier than {entityName2} ")
    {
    }

}
