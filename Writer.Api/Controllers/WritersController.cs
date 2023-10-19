using Common.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Writer.Api.Models;
using Writer.Api.Repository;

namespace Writer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WritersController : ControllerBase
    {
        private readonly IWriterRepository _WriterRepository;
        private readonly IHttpContextAccessor _ctxAccessor;
        private readonly ILogger<WritersController> _logger;

        public WritersController(IWriterRepository WriterRepository, IHttpContextAccessor ctxAccessor, ILogger<WritersController> logger)
        {
            _WriterRepository = WriterRepository;
            _ctxAccessor = ctxAccessor;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<WriterDto>> Get()
        {
            List<string> headers = HttpHeadersHelper.ExtractHeaders(_ctxAccessor.HttpContext.Request.Headers, new string[] { "TransformationTest" });

            _logger.LogInformation($"Request Headers : " + string.Join(",", headers));

            return Ok(_WriterRepository.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<WriterDto> Get(int id)
        {
            var Writer = _WriterRepository.GetById(id);

            if (Writer is null)
                return NotFound();

            return Ok(Writer);
        }

        [HttpPost]
        [Authorize(Policy = "SuperAdminOnly")]
        public IActionResult Post([FromBody] WriterDto writer)
        {
            var result = _WriterRepository.Create(writer);

            return Created($"/get/{result.Id}", result);
        }
    }
}
