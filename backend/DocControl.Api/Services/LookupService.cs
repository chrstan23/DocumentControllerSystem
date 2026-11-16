using DocControl.Api.Models;
using MySqlConnector;

namespace DocControl.Api.Services;

public class LookupService : ILookupService
{
    private readonly string _connectionString;

    public LookupService(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DocControl") ?? throw new InvalidOperationException("Can't connect to the Database.");
    }

    public List<LookupItem> GetSections(string? docType = null)
    {
        if (string.IsNullOrWhiteSpace(docType))
        {
            return GetItems("tbl_section");
        }

        var items = new List<LookupItem>();

        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        var sql = @"SELECT s.id, s.name FROM tbl_section s JOIN tbl_doctype t ON FIND_IN_SET(t.id, s.doctype_ids) > 0 WHERE t.name = @docType ORDER BY s.name";

        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@docType", docType.Trim());
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            items.Add(new LookupItem
            {
                Id = Convert.ToInt32(reader["id"]),
                Name = reader.GetString("name")
            });
        }

        return items;
    }

    public List<LookupItem> GetDocTypes()
    {
        return GetItems("tbl_doctype");
    }

    private List<LookupItem> GetItems(string table)
    {
        var items = new List<LookupItem>();

        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        var sql = $"SELECT id, name FROM {table} ORDER BY name";

        using var cmd = new MySqlCommand(sql, conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            items.Add (new LookupItem
            {
                Id = Convert.ToInt32(reader["id"]),
                Name = reader.GetString("name")
            });
        }
        return items;
    }
}