namespace IDC.Shared.SeedWorks;

public class ApiErrorResult<T> : ApiResult<T>
{
    public List<string> Errors { get; set; }
    public ApiErrorResult(int statusCode, string message) : base(statusCode, false, message)
    {
    }

    public ApiErrorResult(int statusCode, List<string> errors) : base(statusCode, false)
    {
        Errors = errors;
    }
}
