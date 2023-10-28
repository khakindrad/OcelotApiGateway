using Article.Api.Models;
using Article.Api.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Article.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class ArticlesController : ControllerBase
{
    private readonly IArticleRepository _articleRepository;

    public ArticlesController(IArticleRepository articleRepository)
    {
        _articleRepository = articleRepository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<ArticleDto>> Get()
    {
        return Ok(_articleRepository.GetAll());
    }

    [HttpGet("{id}")]
    public ActionResult<ArticleDto> Get(int id)
    {
        var article = _articleRepository.GetById(id);
        if (article is null)
            return NotFound();
        return Ok(article);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var deletedId = _articleRepository.Delete(id);
        if (deletedId == 0)
        {
            return NotFound();
        }

        return NoContent();
    }
}
