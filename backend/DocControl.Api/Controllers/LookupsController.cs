using DocControl.Api.Services;
using DocControl.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace DocControl.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LookupsController : ControllerBase
{
    private readonly ILookupService _lookupService;

    public LookupsController(ILookupService lookupService)
    {
        _lookupService = lookupService;
    }

    [HttpGet("sections")]
    public ActionResult<IEnumerable<LookupItem>> GetSections()
    {
        return Ok(_lookupService.GetSections());
    }

    [HttpGet("doctypes")]
    public ActionResult<IEnumerable<LookupItem>> GetDoctypes()
    {
        return Ok(_lookupService.GetDocTypes());
    }
}