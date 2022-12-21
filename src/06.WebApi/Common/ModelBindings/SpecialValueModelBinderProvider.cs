using System.Reflection;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Zeta.NontonFilm.Shared.Common.Attributes;

namespace Zeta.NontonFilm.WebApi.Common.ModelBindings;

public class SpecialValueModelBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var propertyName = context.Metadata.PropertyName;

        if (propertyName is null)
        {
            return null;
        }

        var propertyInfo = context.Metadata.ContainerType?.GetProperty(propertyName);

        if (propertyInfo is null)
        {
            return null;
        }

        var attribute = propertyInfo.GetCustomAttribute<SpecialValueAttribute>();

        if (attribute is not null && attribute.ValueType == SpecialValueType.Json)
        {
            return new JsonModelBinder();
        }
        else
        {
            return null;
        }
    }
}
