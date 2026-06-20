using Microsoft.AspNetCore.Mvc;

namespace SurveysApi.Business.Common;

/// <summary>
/// Traduce un <see cref="ServiceResult{T}"/> al <see cref="IActionResult"/>
/// correspondiente. Permite que los controllers sean una sola línea.
/// </summary>
public static class ServiceResultExtensions
{
    public static IActionResult ToActionResult<T>(this ServiceResult<T> result)
    {
        var body = BuildBody(result);

        return result.Status switch
        {
            ResultStatus.Ok => new OkObjectResult(body),
            ResultStatus.Created => new ObjectResult(body) { StatusCode = StatusCodes.Status201Created },
            ResultStatus.BadRequest => new BadRequestObjectResult(body),
            ResultStatus.Unauthorized => new UnauthorizedObjectResult(body),
            ResultStatus.NotFound => new NotFoundObjectResult(body),
            ResultStatus.Conflict => new ConflictObjectResult(body),
            _ => new ObjectResult(body) { StatusCode = StatusCodes.Status500InternalServerError }
        };
    }

    private static object? BuildBody<T>(ServiceResult<T> result)
    {
        if (result.Data is null) return new { message = result.Message };
        if (result.Message is null) return result.Data;
        return new { message = result.Message, data = result.Data };
    }
}
