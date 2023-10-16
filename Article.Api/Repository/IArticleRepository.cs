using Article.Api.Models;

namespace Article.Api.Repository
{
    public interface IArticleRepository
    {
        List<ArticleDto> GetAll();
        ArticleDto? GetById(int id);
        int Delete(int id);
    }
}
