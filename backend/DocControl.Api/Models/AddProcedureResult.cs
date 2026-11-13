namespace DocControl.Api.Models;

public enum AddProcedureStatus
{
    Created, 
    InvalidFile,
    RequestorNotFound,
    Duplicate
}

public class AddProcedureResult
{
    public AddProcedureStatus Status { get; set;}
    public string Message { get; set; } = "";
    public long Id { get; set; }
}