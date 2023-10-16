using Microsoft.AspNetCore.Mvc;
using Writer.Api.Models;
using Writer.Api.Repository;

namespace Writer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WritersController : ControllerBase
    {
        private readonly IWriterRepository _WriterRepository;

        public WritersController(IWriterRepository WriterRepository)
        {
            _WriterRepository = WriterRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<WriterDto>> Get()
        {
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
        public IActionResult Post([FromBody] WriterDto writer)
        {
            var result = _WriterRepository.Create(writer);

            return Created($"/get/{result.Id}", result);
        }
    }
}
