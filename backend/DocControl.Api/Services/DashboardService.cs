using DocControl.Api.Models;
using MySqlConnector;

namespace DocControl.Api.Services;

public class DashboardService : IDashboardService
{
    private readonly string _connectionString;

    public DashboardService(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DocControl") ?? throw new InvalidOperationException("Can't connect to the database.");
    }

    public DashboardSummary GetSummary()
    {
        var summary = new DashboardSummary();

        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        var totalSql = @"SELECT COUNT(*) FROM tbl_procedures WHERE status = 'Effective'";
        using (var cmd = new MySqlCommand(totalSql, conn))
        {
            summary.TotalDocuments = Convert.ToInt32(cmd.ExecuteScalar());
        }

        var revisionSql = @"SELECT COUNT(*) FROM tbl_procedures WHERE status = 'Effective' AND revision_status <> 'Completed'";
        using (var cmd = new MySqlCommand(revisionSql, conn))
        {
            summary.UnderRevision = Convert.ToInt32(cmd.ExecuteScalar());
        }

        var typeSql = @"SELECT t.name, COUNT(p.id) AS total FROM tbl_docType t LEFT JOIN tbl_procedures p ON p.doc_type = t.name AND p.status = 'Effective' GROUP BY t.id, t.name ORDER BY t.name";
        using (var cmd = new MySqlCommand(typeSql, conn))
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                summary.DocTypeCounts.Add(new DocTypeCount
                {
                    Name = reader.GetString("name"),
                    Total = Convert.ToInt32(reader["total"])
                });
            }
        }

        return summary;
    }
}