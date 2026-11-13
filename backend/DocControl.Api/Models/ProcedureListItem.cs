namespace DocControl.Api.Models;

public class ProcedureListItem
{
    public int Id { get; set; }
    public string DocNo { get; set; } = "";
    public string DocTitle { get; set; } = "";
    public string DocType { get; set; } = "";
    public string Department { get; set; } = "";
    public string Revision { get; set; } = "";
    public DateOnly EffectivityDate { get; set; }
    public string PreparedBy { get; set; } = "";
    public string FileName { get; set; } = "";
    public long FileSize{ get; set; }


}