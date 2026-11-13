namespace DocControl.Api.Models;

public class ProcedureFile
{
    public string FileName { get; set; } = "";
    public string ContentType { get; set; } = "";

    public byte[] Data { get; set; } = Array.Empty<byte>();
}