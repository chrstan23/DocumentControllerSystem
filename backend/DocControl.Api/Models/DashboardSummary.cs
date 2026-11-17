namespace DocControl.Api.Models;

public class DocTypeCount
{
    public string Name { get; set; } = "";
    public int Total { get; set; }
}

public class DashboardSummary
{
    public int TotalDocuments { get; set; }
    public int UnderRevision { get; set; }
    public List<DocTypeCount> DocTypeCounts { get; set; } = new();
}