using System.Net;
using System.Net.Mime;
using System.Text;
using Newtonsoft.Json;
using RestSharp;
using Zeta.NontonFilm.Client.Common.Responses;
using Zeta.NontonFilm.Shared.Common.Extensions;
using Zeta.NontonFilm.Shared.Common.Responses;

namespace Zeta.NontonFilm.Client.Common.Extensions;

public static class RestResponseExtensions
{
    public static ResponseResult<T> ToResponseResult<T>(this RestResponse restResponse)
    {
        var responseResult = new ResponseResult<T>();

        try
        {
            if (restResponse.IsSuccessful)
            {
                responseResult.Result = CreateResultResponse<T>(restResponse);
            }
            else
            {
                responseResult.Error = CreateErrorResponse(restResponse);
            }
        }
        catch (Exception exception)
        {
            var exceptionType = exception.GetType().FullName;

            if (string.IsNullOrWhiteSpace(exceptionType))
            {
                exceptionType = "Unknown Exception";
            }

            var detail = new StringBuilder();
            detail.AppendLine("Unhandled exception occured when retrieving response from Back-End service.");
            detail.AppendLine($"Response content from Back-End service: {restResponse.Content}.");
            detail.AppendLine($"Exception type: {exceptionType}.");
            detail.AppendLine($"Exception message: {exception.Message}.");
            detail.AppendLine($"Exception stack trace: {exception.StackTrace}.");

            responseResult.Error = new ErrorResponse
            {
                Type = "Unhandled Exception",
                Title = exception.Message,
                Status = restResponse.StatusCode,
                Detail = detail.ToString(),
            };
        }

        return responseResult;
    }

    private static T CreateResultResponse<T>(RestResponse restResponse)
    {
        if (typeof(T).IsSubclassOf(typeof(FileResponse)))
        {
            if (restResponse.ContentHeaders is null)
            {
                throw new Exception("Response headers is null");
            }

            var contentDispositionContentHeader = restResponse.ContentHeaders
                    .Where(x => x.Name == "Content-Disposition")
                    .FirstOrDefault();

            if (contentDispositionContentHeader is null)
            {
                throw new Exception("Content-Disposition Content Header is null");
            }

            if (contentDispositionContentHeader.Value is not string contentDispositionValue)
            {
                throw new Exception("Content-Disposition Value is null");
            }

            var contentDisposition = new ContentDisposition(contentDispositionValue);
            var fileName = contentDisposition.FileName;

            if (fileName is null)
            {
                throw new Exception("ContentDisposition.FileName is null");
            }

            if (restResponse.RawBytes is null)
            {
                throw new Exception("Response content is null");
            }

            if (restResponse.ContentType is null)
            {
                throw new Exception("Response MIME content type is null");
            }

            var response = new FileResponse
            {
                FileName = fileName,
                Content = restResponse.RawBytes,
                ContentType = restResponse.ContentType
            };

            var serializedFileResponse = JsonConvert.SerializeObject(response);
            var result = JsonConvert.DeserializeObject<T>(serializedFileResponse);

            if (result is null)
            {
                throw new Exception($"Failed to deserialize {nameof(FileResponse)} {serializedFileResponse} into {typeof(T).FullName}.");
            }

            return result;
        }
        else
        {
            if (string.IsNullOrWhiteSpace(restResponse.Content))
            {
                if (new NoContentResponse() is not T noContentResponse)
                {
                    throw new Exception($"Cannot cast {nameof(NoContentResponse)} into {typeof(T).FullName}.");
                }

                return noContentResponse;
            }
            else
            {
                var response = JsonConvert.DeserializeObject<T>(restResponse.Content);

                if (response is null)
                {
                    throw new Exception($"Failed to deserialize JSON content {restResponse.Content} into {typeof(T).FullName}.");
                }

                return response;
            }
        }
    }

    private static ErrorResponse CreateErrorResponse(RestResponse restResponse)
    {
        if (restResponse.StatusCode == 0 && restResponse.ErrorException is WebException)
        {
            return new ErrorResponse
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.4",
                Title = HttpStatusCode.ServiceUnavailable.ToString().SplitWords(),
                Status = HttpStatusCode.ServiceUnavailable,
                Detail = $"Service at {restResponse.ResponseUri} is not available. Error message: {restResponse.ErrorMessage}"
            };
        }

        if (!string.IsNullOrWhiteSpace(restResponse.Content))
        {
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(restResponse.Content);

            if (errorResponse is null)
            {
                throw new Exception($"Failed to deserialize JSON content {restResponse.Content} into {nameof(ErrorResponse)}.");
            }

            return errorResponse;
        }

        return new ErrorResponse
        {
            Title = "Something went wrong.",
            Status = restResponse.StatusCode,
            Type = "Unknown error type.",
            Detail = "Unknown error detail."
        };
    }
}
