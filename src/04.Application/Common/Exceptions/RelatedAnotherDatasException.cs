using Zeta.NontonFilm.Shared.Common.Constants;

namespace Zeta.NontonFilm.Application.Common.Exceptions;

public class RelatedAnotherDatasException : Exception
{
    public RelatedAnotherDatasException()
        : base()
    {
    }

    public RelatedAnotherDatasException(string message)
        : base(message)
    {
    }

    public RelatedAnotherDatasException(string entityName, object key)
        : base($"{entityName} with {CommonDisplayTextFor.Id} [{key}] Cannot be Delete, because {entityName} already has data.")
    {
    }

    public RelatedAnotherDatasException(string entityName, string entityField, object value)
        : base($"{entityName} with {entityField}: {value} Cannot be Delete, because {entityName} already has data.")
    {
    }
}
