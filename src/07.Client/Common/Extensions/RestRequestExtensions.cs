using System.Collections;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RestSharp;
using Zeta.NontonFilm.Shared.Common.Attributes;

namespace Zeta.NontonFilm.Client.Common.Extensions;

public static class RestRequestExtensions
{
    private const string UniversalDateTimeFormat = "yyyy/MM/dd HH:mm:ss";

    public static void AddParameters<T>(this RestRequest restRequest, T request) where T : notnull
    {
        foreach (var property in typeof(T).GetProperties())
        {
            var value = property.GetValue(request);

            if (value is null)
            {
                continue;
            }

            if (value is IFormFile formFile)
            {
                restRequest.AddFile(property.Name, formFile.ToBytes(), formFile.FileName, contentType: formFile.ContentType);
            }
            else
            {
                var specialValueAttributes = property.GetCustomAttributes(typeof(SpecialValueAttribute), false);

                if (specialValueAttributes.Any())
                {
                    var specialValueAttribute = (SpecialValueAttribute)specialValueAttributes.First();

                    if (specialValueAttribute.ValueType == SpecialValueType.Json)
                    {
                        restRequest.AddParameter(property.Name, JsonConvert.SerializeObject(value));
                    }
                }
                else if (property.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                {
                    var index = 0;

                    foreach (var childItem in ((IEnumerable)value).OfType<object>())
                    {
                        if (childItem.GetType().IsValueType)
                        {
                            restRequest.AddParameter(property.Name, childItem.ToString());
                        }
                        else
                        {
                            foreach (var childItemProperty in childItem.GetType().GetProperties())
                            {
                                var childItemValue = childItemProperty.GetValue(childItem);

                                if (childItemValue is not null)
                                {
                                    var name = $"{property.Name}[{index}].{childItemProperty.Name}";
                                    restRequest.AddParameter(name, childItemValue.ToString());
                                }
                            }
                        }

                        index++;
                    }
                }
                else if (property.PropertyType == typeof(DateTime))
                {
                    var dateTime = (DateTime)value;

                    restRequest.AddParameter(property.Name, dateTime.ToString(UniversalDateTimeFormat));
                }
                else if (property.PropertyType == typeof(DateTime?))
                {
                    var nullableDateTime = (DateTime?)value;

                    restRequest.AddParameter(property.Name, nullableDateTime.Value.ToString(UniversalDateTimeFormat));
                }
                else
                {
                    restRequest.AddParameter(property.Name, value.ToString());
                }
            }
        }
    }
}
