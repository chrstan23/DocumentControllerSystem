using DocControl.Api.Models;
using DocControl.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DoccControl.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProceduresController : ControllerBase
{
    private readonly IProcedureService _procedureService;

    public ProceduresController(IProcedureService procedureService)
    {
        _procedureService = procedureService;
    }

    //POST API for adding existing Procedure
    [HttpPost("existing")]
    [Consumes("multipart/form-data")]
    public IActionResult AddExisting([FromForm] ExistingProcedureForm form)
    {
        var result = _procedureService.AddExisting(form);

        return result.Status switch
        {
            AddProcedureStatus.Created => Created($"/api/procedures/{result.Id}", new {id = result.Id }),
            AddProcedureStatus.Duplicate => Conflict(new { message = result.Message}), _=> BadRequest(new  { message = result.Message})
        };
    }

    //GET API for the Masterlists data
    [HttpGet]
    public ActionResult<IEnumerable<ProcedureListItem>> GetMasterlist()
    {
        return Ok(_procedureService.GetMasterlist());

    }

    //GET API for the PDF files
    [HttpGet("{id:int}/file")]
    public IActionResult GetFile(int id)
    {
        var file = _procedureService.GetFile(id);

        if (file == null)
            return NotFound();

        return File(file.Data, file.ContentType);
    }
}