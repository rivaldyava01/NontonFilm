using Zeta.NontonFilm.Shared.Common.Constants;

namespace Zeta.NontonFilm.Application.Common.Exceptions;

public class OneMovieOneDaysException : Exception
{
    public OneMovieOneDaysException()
        : base()
    {
    }

    public OneMovieOneDaysException(string message)
        : base(message)
    {
    }

    public OneMovieOneDaysException(string entityName, object key)
        : base($"{entityName} with {CommonDisplayTextFor.Id} [{key}] Cannot be processed, because only one movie for one day")
    {
    }

    public OneMovieOneDaysException(string entityName, string entityField, object value)
        : base($"{entityName} with {entityField}: {value} Cannot be processed, because only one movie for one day")
    {
    }
}
