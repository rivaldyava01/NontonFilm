using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Zeta.NontonFilm.Application.Common.Exceptions;

namespace Zeta.NontonFilm.WebApi.Common.Filters.ApiException;

public sealed class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        var exception = context.Exception;

        if (!context.ModelState.IsValid)
        {
            var details = new ValidationProblemDetails(context.ModelState)
            {
                Type = ProblemDetailsFor.InvalidModelState.Type
            };

            context.Result = new UnprocessableEntityObjectResult(details);
        }
        else if (context.Exception is MismatchException)
        {
            var details = new ProblemDetails()
            {
                Status = StatusCodes.Status422UnprocessableEntity,
                Type = ProblemDetailsFor.MismatchException.Type,
                Title = ProblemDetailsFor.MismatchException.Title,
                Detail = context.Exception.Message
            };

            context.Result = new UnprocessableEntityObjectResult(details);
        }
        else if (exception is ApplicationValidationException validationException)
        {
            var details = new ValidationProblemDetails(validationException.Errors)
            {
                Type = ProblemDetailsFor.ValidationException.Type
            };

            context.Result = new UnprocessableEntityObjectResult(details);
        }
        else if (context.Exception is ArgumentException)
        {
            var details = new ProblemDetails()
            {
                Status = StatusCodes.Status422UnprocessableEntity,
                Type = ProblemDetailsFor.ArgumentException.Type,
                Title = ProblemDetailsFor.ArgumentException.Title,
                Detail = context.Exception.Message
            };

            context.Result = new UnprocessableEntityObjectResult(details);
        }
        else if (exception is UnauthorizedAccessException)
        {
            var details = new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Type = ProblemDetailsFor.UnauthorizedAccessException.Type,
                Title = ProblemDetailsFor.UnauthorizedAccessException.Title,
                Detail = exception.Message
            };

            context.Result = new UnauthorizedObjectResult(details);
        }
        else if (exception is ForbiddenAccessException)
        {
            var details = new ProblemDetails()
            {
                Status = StatusCodes.Status403Forbidden,
                Type = ProblemDetailsFor.ForbiddenAccessException.Type,
                Title = ProblemDetailsFor.ForbiddenAccessException.Title,
                Detail = context.Exception.Message
            };

            context.Result = new ObjectResult(details);
        }
        else if (context.Exception is NotFoundException)
        {
            var details = new ProblemDetails()
            {
                Status = StatusCodes.Status404NotFound,
                Type = ProblemDetailsFor.NotFoundException.Type,
                Title = ProblemDetailsFor.NotFoundException.Title,
                Detail = context.Exception.Message
            };

            context.Result = new NotFoundObjectResult(details);
        }
        else if (context.Exception is AlreadyExistsException)
        {
            var details = new ProblemDetails()
            {
                Status = StatusCodes.Status409Conflict,
                Type = ProblemDetailsFor.AlreadyExistsExceptions.Type,
                Title = ProblemDetailsFor.AlreadyExistsExceptions.Title,
                Detail = context.Exception.Message
            };

            context.Result = new ConflictObjectResult(details);
        }
        else if (context.Exception is InvalidOperationException)
        {
            var details = new ProblemDetails()
            {
                Status = StatusCodes.Status422UnprocessableEntity,
                Type = ProblemDetailsFor.InvalidOperationException.Type,
                Title = ProblemDetailsFor.InvalidOperationException.Title,
                Detail = context.Exception.Message
            };

            context.Result = new UnprocessableEntityObjectResult(details);
        }
        else
        {
            var details = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Type = ProblemDetailsFor.UnknownException.Type,
                Title = ProblemDetailsFor.UnknownException.Title,
                Detail = context.Exception.Message
            };

            context.Result = new ObjectResult(details);
        }

        context.ExceptionHandled = true;
    }
}
