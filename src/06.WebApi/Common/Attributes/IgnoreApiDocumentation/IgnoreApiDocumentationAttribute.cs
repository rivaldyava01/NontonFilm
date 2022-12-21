using Microsoft.AspNetCore.Mvc;
using Zeta.NontonFilm.Shared.Common.Constants;

namespace Zeta.NontonFilm.WebApi.Common.Attributes.IgnoreApiDocumentation;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class IgnoreApiDocumentationAttribute : ApiExplorerSettingsAttribute
{
    public IgnoreApiDocumentationAttribute(string environments) : base()
    {
        if (environments is not null)
        {
            var ignoreApiDocumentationEnvironments = environments.Split(',').Select(x => x.Trim());

            var isIgnoreApiDocumentation = ignoreApiDocumentationEnvironments.Any(x => x.Contains(CommonValueFor.EnvironmentName));

            IgnoreApi = isIgnoreApiDocumentation;
        }
    }
}
