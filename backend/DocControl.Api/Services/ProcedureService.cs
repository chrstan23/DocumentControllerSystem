using System.Text;
using DocControl.Api.Models;
using MySqlConnector;

namespace DocControl.Api.Services;

public class ProcedureService : IProcedureService
{
    private const long MaxFileBytes = 200L * 1024 *1024; // 200 MB
    private readonly string _connectionString;

    public ProcedureService(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DocControl") ?? throw new InvalidOperationException("Can't connect to the database.");
    }

    public AddProcedureResult AddExisting(ExistingProcedureForm form)
    {
        var file = form.File!;

        if (file.Length == 0)   //if there is no file uploaded, return an error
            return Fail(AddProcedureStatus.InvalidFile, "The File is Empty.");

        if (file.Length > MaxFileBytes) //if the file is larger than 200MB, return an error
            return Fail(AddProcedureStatus.InvalidFile, "The File is too large. Maximum allowed size is 200MB.");

        if (!Path.GetExtension(file.FileName).Equals(".pdf", StringComparison.OrdinalIgnoreCase)) //if the file is not a PDF, return an error
            return Fail(AddProcedureStatus.InvalidFile, "Only PDF files are allowed.");

        byte[] bytes; 
        using (var ms = new MemoryStream())
        {
            file.CopyTo(ms);
            bytes = ms.ToArray();

        }

        if (bytes.Length < 5 || Encoding.ASCII.GetString(bytes, 0, 5) != "%PDF-")   
            return Fail(AddProcedureStatus.InvalidFile, "The file is not a valid PDF.");

        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        var checkSql = @"SELECT COUNT(*) FROM tbl_user WHERE employee_id = @id AND account_status in ('ACTIVE', 'INACTIVE')";

        using (var check = new MySqlCommand(checkSql, conn))
        {
            check.Parameters.AddWithValue("@id", form.RequestorId);
            if (Convert.ToInt32(check.ExecuteScalar()) == 0)
                return Fail(AddProcedureStatus.RequestorNotFound, "Requestor Id not found.");
        }

        var docNo = form.DocNo.Trim();
        var fileName = Path.GetFileName(file.FileName);

        var insertSql = @"INSERT INTO tbl_procedures (doc_no, doc_title, doc_type, department, revision, status, revision_status, requestor_id, effectivity_date, file_name, content_type, file_size, file_data)
                        VALUES (@doc_no, @doc_title, @doc_type, @department, @revision, 'ACTIVE', 'APPROVED', @requestor_id, @effectivity_date, @file_name, 'application/pdf', @file_size, @file_data)";

        using var cmd = new MySqlCommand(insertSql, conn);
        cmd.Parameters.AddWithValue("doc_no", docNo);
        cmd.Parameters.AddWithValue("doc_title", form.DocTitle.Trim());
        cmd.Parameters.AddWithValue("doc_type", form.DocType.Trim());
        cmd.Parameters.AddWithValue("department", form.Department.Trim());
        cmd.Parameters.AddWithValue("revision", form.Revision);
        cmd.Parameters.AddWithValue("requestor_id", form.RequestorId);
        cmd.Parameters.AddWithValue("effectivity_date", form.EffectivityDate!.Value.ToDateTime(TimeOnly.MinValue));
        cmd.Parameters.AddWithValue("file_name", fileName);
        cmd.Parameters.AddWithValue("file_size", (uint)bytes.Length);
        cmd.Parameters.AddWithValue("file_data", bytes);

        try
        {
            cmd.ExecuteNonQuery();

        }
        catch (MySqlException ex) when (ex.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
        {
            return Fail(AddProcedureStatus.Duplicate, $"Document number {docNo} already exists.");
        }

        return new AddProcedureResult
        {
            Status = AddProcedureStatus.Created,
            Message = "Document added successfully.",
            Id = cmd.LastInsertedId
        };
    }
    private static AddProcedureResult Fail(AddProcedureStatus status, string message)
    {
        return new AddProcedureResult { Status = status, Message = message};
    }

    public List<ProcedureListItem> GetMasterlist()
    {
        var items = new List<ProcedureListItem>();

        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        var sql = @"SELECT a.id, a.doc_no, a.doc_title, a.doc_type, a.department, a.revision, a.effectivity_date, b.fullname, a.file_name, a.file_size
                    FROM tbl_procedures a JOIN tbl_user b ON b.employee_id = a.requestor_id WHERE a.status = 'Effective' ORDER BY a.effectivity_date";

        using var cmd = new MySqlCommand(sql, conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            items.Add(new ProcedureListItem
            {
                Id = Convert.ToInt32(reader["id"]),
                DocNo = reader.GetString("doc_no"),
                DocTitle = reader.GetString("doc_title"),
                DocType = reader.GetString("doc_type"),
                Department = reader.GetString("department"),
                Revision = reader.GetString("revision"),
                EffectivityDate = DateOnly.FromDateTime(reader.GetDateTime("effectivity_date")),
                PreparedBy = reader.GetString("fullname"),
                FileSize = Convert.ToInt64(reader["file_size"])
            });
        }

        return items;
    }

    public ProcedureFile? GetFile(int id)
    {
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        var sql = @"SELECT file_name, content_type, file_data FROM tbl_procedures WHERE id = @id, AND status = 'Effective'";

        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", id);
        using var reader = cmd.ExecuteReader();

        if (!reader.Read() || reader.IsDBNull(reader.GetOrdinal("file_data")))
            return null;

        return new ProcedureFile
        {
            FileName = reader.GetString("file_name"),
            ContentType = reader.GetString("content_type"),
            Data = (byte[])reader["file_data"]
        };

    }

}