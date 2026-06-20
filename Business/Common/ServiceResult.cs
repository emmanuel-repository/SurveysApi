namespace SurveysApi.Business.Common;

/// <summary>
/// Estados posibles que un service puede devolver. El controller los traduce
/// al código HTTP correspondiente mediante <see cref="ServiceResultExtensions"/>.
/// </summary>
public enum ResultStatus
{
    Ok,
    Created,
    BadRequest,
    Unauthorized,
    NotFound,
    Conflict,
    Error
}

/// <summary>
/// Resultado de una operación de negocio. Encapsula el estado, los datos y un
/// mensaje opcional, de forma que la capa de negocio no dependa de ASP.NET.
/// </summary>
public class ServiceResult<T>
{
    public ResultStatus Status { get; private init; }
    public T? Data { get; private init; }
    public string? Message { get; private init; }

    public bool IsSuccess => Status is ResultStatus.Ok or ResultStatus.Created;

    public static ServiceResult<T> Ok(T data, string? message = null) =>
        new() { Status = ResultStatus.Ok, Data = data, Message = message };

    public static ServiceResult<T> OkMessage(string message) =>
        new() { Status = ResultStatus.Ok, Message = message };

    public static ServiceResult<T> Created(T data, string? message = null) =>
        new() { Status = ResultStatus.Created, Data = data, Message = message };

    public static ServiceResult<T> NotFound(string message = "Recurso no encontrado") =>
        new() { Status = ResultStatus.NotFound, Message = message };

    public static ServiceResult<T> Conflict(string message) =>
        new() { Status = ResultStatus.Conflict, Message = message };

    public static ServiceResult<T> BadRequest(string message) =>
        new() { Status = ResultStatus.BadRequest, Message = message };

    public static ServiceResult<T> Unauthorized(string message) =>
        new() { Status = ResultStatus.Unauthorized, Message = message };

    public static ServiceResult<T> Error(string message = "Error interno del servidor") =>
        new() { Status = ResultStatus.Error, Message = message };
}
