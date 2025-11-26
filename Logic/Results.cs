namespace CodeChallenge.Api.Logic;

public abstract record Result;

public record Success : Result;
public record Created<T>(T Value) : Result;
public record Updated : Result;
public record Deleted : Result;
public record NotFound(string Message) : Result;
public record Conflict(string Message) : Result;
public record ValidationError(Dictionary<string, string[]> Errors) : Result;




public abstract class Resultt
{
    public bool Success { get; set; }
    public string? Error { get; set; }
}
public class SuccessResult : Resultt
{
    public SuccessResult() { Success = true; }
}

public class DataResult<T> : Resultt
{
    public T? Data { get; set; }

    public static DataResult<T> Ok(T data) =>
        new DataResult<T> { Success = true, Data = data };

    public static DataResult<T> Fail(string error) =>
        new DataResult<T> { Success = false, Error = error };
}