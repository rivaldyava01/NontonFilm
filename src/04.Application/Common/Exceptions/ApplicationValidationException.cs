using System.Text;
using FluentValidation.Results;

namespace Zeta.NontonFilm.Application.Common.Exceptions;

public class ApplicationValidationException : Exception
{
    public IDictionary<string, string[]> Errors { get; }

    public ApplicationValidationException()
        : base("One or more validation failures have occurred.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ApplicationValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }

    public string Summary
    {
        get
        {
            var summary = new StringBuilder();

            foreach (var failure in Errors)
            {
                foreach (var errorMessage in failure.Value)
                {
                    summary.Append(errorMessage);
                }
            }

            return summary.ToString();
        }
    }
}
