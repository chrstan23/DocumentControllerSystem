using DocControl.Api.Models;
using MySqlConnector;

namespace DocControl.Api.Services;

public class RequestorService : IRequestorService
{
    private readonly string _connectionString;

    public RequestorService(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DocControl") ?? throw new InvalidOperationException("Can't connect to the database.");

    }

    public List<Requestor> GetRequestors()
    {
        var requestors = new List<Requestor>();

        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        var sql = "SELECT employee_id, fullname, section FROM tbl_user WHERE account_status IN ('ACTIVE', 'INACTIVE') ORDER BY fullname";

        using var cmd = new MySqlCommand(sql, conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            requestors.Add(new Requestor
            {
                EmployeeId = reader.GetString("employee_id"),
                FullName = reader.GetString("fullname"),
                Section = reader.GetString("section")
            });
        }

        return requestors;
    }
}