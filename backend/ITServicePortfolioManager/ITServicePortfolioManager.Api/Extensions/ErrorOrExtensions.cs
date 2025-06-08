using ErrorOr;

namespace ITServicePortfolioManager.Api.Extensions;

public static class ErrorOrExtensions
{
    public static IResult ToResult<T>(this ErrorOr<T> result)
    {
        return result.Match(
            Results.Ok,
            errors =>
            {
                var firstError = errors[0];

                var statusCode = firstError.Type switch
                {
                    ErrorType.Validation => StatusCodes.Status400BadRequest,
                    ErrorType.NotFound => StatusCodes.Status404NotFound, 
                    ErrorType.Conflict => StatusCodes.Status409Conflict,
                    _ => StatusCodes.Status500InternalServerError 
                };

                return Results.Problem(statusCode: statusCode, title: firstError.Description);
            }
        );
    }
}