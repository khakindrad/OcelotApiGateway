using Common.Extensions;
using Common.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Writer.Api.Models;
using Writer.Api.Repository;

namespace Writer.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class WritersController : ControllerBase
{
    private readonly IWriterRepository _WriterRepository;
    private readonly IHttpContextAccessor _ctxAccessor;
    private readonly ILogger<WritersController> _logger;

    public WritersController(IWriterRepository writerRepository, IHttpContextAccessor ctxAccessor, ILogger<WritersController> logger)
    {
        _WriterRepository = writerRepository;
        _ctxAccessor = ctxAccessor;
        _logger = logger;
    }

    [HttpGet]
    public ActionResult<IEnumerable<WriterDto>> Get()
    {
        if (_ctxAccessor.HttpContext is not null)
        {
            var headers = HttpHeadersHelper.ExtractHeaders(_ctxAccessor.HttpContext.Request.Headers, new string[] { "TransformationTest" });

            _logger.LogMessage($"Request Headers : " + string.Join(',', headers));
        }

        return Ok(_WriterRepository.GetAll());
    }

    [HttpGet("{id}")]
    public ActionResult<WriterDto> Get(int id)
    {
        var writer = _WriterRepository.GetById(id);

        if (writer is null)
            return NotFound();

        return Ok(writer);
    }

    [HttpPost]
    [Authorize(Policy = "SuperAdminOnly")]
    public IActionResult Post([FromBody] WriterDto writer)
    {
        var result = _WriterRepository.Create(writer);

        return Created(new Uri($"/get/{result.Id}"), result);
    }
}
