using DocControl.Api.Models;
using DocControl.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DocControl.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RequestorsController : ControllerBase
{
    private readonly IRequestorService _requestorService;

    public RequestorsController(IRequestorService requestorService)
    {
        _requestorService = requestorService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Requestor>> GetRequestors()
    {
        return Ok(_requestorService.GetRequestors());
    }
}