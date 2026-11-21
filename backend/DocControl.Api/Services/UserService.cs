using DocControl.Api.Models;
using MySqlConnector;

namespace DocControl.Api.Services;

public class UserService : IUserService
{
    private readonly string _connectionString;

    private record UserState(string Role, string Status);

    public UserService(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DocControl") ?? throw new InvalidOperationException("Can't connect to the database");
    }

    public List<UserListItems> GetUsers()
    {
        var users = new List<UserListItems>();

        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        var sql = @"SELECT employee_id, username, fullname, section, role, account_status FROM tbl_user ORDER BY fullname";

        using var cmd = new MySqlCommand(sql, conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            users.Add(new UserListItems
            {
                EmployeeId = reader.GetString("employee_id"),
                Username = reader.GetString("username"),
                FullName = reader.GetString("fullname"),
                Section = reader.GetString("section"),
                Role = reader.GetString("role"),
                AccountStatus = reader.GetString("account_status")
            });
        }
        return users;
    }

    public UserResult Update(string employeeId, UpdateUserRequest request)
    {
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        var state = GetState(conn, employeeId);
        if (state == null)
            return UserResult.Fail(UserOpStatus.NotFound, "User not found.");

        // var section = request.Section.Trim();
        // if(!SectionExists(conn, section))
        //     return UserResult.Fail(UserOpStatus.Invalid, "Section not found.");

        if (request.Role != "ADMIN" && IsOnlyActiveAdmin(conn, employeeId, state))
            return UserResult.Fail(UserOpStatus.Conflict, "You cannot change the role of the only active administrator.");

        var sql = @"UPDATE tbl_user SET username = @username, fullname = @fullname, section = @section, role = @role WHERE employee_id = @id";

        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@username", request.UserName.Trim());
        cmd.Parameters.AddWithValue("@fullname", request.FullName.Trim());
        cmd.Parameters.AddWithValue("@section", request.Section.Trim());
        cmd.Parameters.AddWithValue("@role", request.Role);
        cmd.Parameters.AddWithValue("@id", employeeId);

        try
        {
            cmd.ExecuteNonQuery();
        }
        catch (MySqlException ex) when (ex.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
        {
            return UserResult.Fail(UserOpStatus.Conflict, "That username is already taken.");
        }

        return UserResult.Ok();
    }

    public UserResult SetStatus(string employeeId, bool active)
    {
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        var state = GetState(conn, employeeId);
        if (state == null)
            return UserResult.Fail(UserOpStatus.NotFound, "User not found.");

        if (!active && IsOnlyActiveAdmin(conn, employeeId, state))
            return UserResult.Fail(UserOpStatus.Conflict, "You cannot deactivate the only active administrator.");
        
        var sql = "UPDATE tbl_user SET account_status = @status WHERE employee_id = @id";

        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@status", active ? "ACTIVE" : "INACTIVE");
        cmd.Parameters.AddWithValue("@id", employeeId);
        cmd.ExecuteNonQuery();

        return UserResult.Ok();
    }

    public UserResult Delete(string employeeId)
    {
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        var state = GetState(conn, employeeId);

        if (state == null)
            return UserResult.Fail(UserOpStatus.Invalid, "User not found");
        if (IsOnlyActiveAdmin(conn, employeeId, state))
            return UserResult.Fail(UserOpStatus.Conflict, "You cannot delete the only active administrator.");
        
        var sql = "DELETE FROM tbl_user WHERE employee_id = @id";

        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", employeeId);
        cmd.ExecuteNonQuery();

        return UserResult.Ok();
    }

    private static UserState? GetState(MySqlConnection conn, string employeeId)
    {
        var sql = "SELECT role, account_status FROM tbl_user WHERE employee_id = @id";

        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", employeeId);
        using var reader = cmd.ExecuteReader();

        if(!reader.Read())
            return null;
        
        return new UserState(reader.GetString("role"), reader.GetString("account_status"));
    }

    // private static bool SectionExists(MySqlConnection conn, string name)
    // {
    //     var sql = "SELECT COUNT(*) FROM tbl_section WHERE name = @name";

    //     using var cmd = new MySqlCommand(sql, conn);
    //     cmd.Parameters.AddWithValue("name", name);

    //     return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
    // }

    private static bool IsOnlyActiveAdmin(MySqlConnection conn, string employeeId, UserState state)
    {
        if (state.Role != "ADMIN" || state.Status != "ACTIVE")
            return false;

        var sql = @"SELECT COUNT(*) FROM tbl_user WHERE role = 'ADMIN' AND account_status = 'ACTIVE' AND employee_id <> @id";

        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", employeeId);

        return Convert.ToInt32(cmd.ExecuteScalar()) == 0;
    }
}

