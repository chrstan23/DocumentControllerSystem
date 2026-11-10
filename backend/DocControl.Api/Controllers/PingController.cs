using MySqlConnector;
using Microsoft.AspNetCore.Mvc;

namespace DocControl.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PingController : ControllerBase
{
    private readonly IConfiguration _config;

    public PingController(IConfiguration config)
    {
        _config = config;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var connectionString = _config.GetConnectionString("DocControl");

        using var conn = new MySqlConnection(connectionString);
        conn.Open();

        using var cmd = new MySqlCommand("SELECT COUNT(*) FROM tbl_user", conn);
        var count = Convert.ToInt32(cmd.ExecuteScalar());

        return Ok(new {users = count});

    }
}