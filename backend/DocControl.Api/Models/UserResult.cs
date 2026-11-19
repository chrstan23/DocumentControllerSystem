namespace DocControl.Api.Models;

public enum UserOpStatus
{
    Ok, 
    NotFound,
    Invalid,
    Conflict
}

public class UserResult
{
    public UserOpStatus Status { get; set; }
    public string Message { get; set; } = "";

    public static UserResult Ok()
    {
        return new UserResult { Status = UserOpStatus.Ok};
    }

    public static UserResult Fail(UserOpStatus status, string message)
    {
        return new UserResult {Status = status, Message = message };
    }
}