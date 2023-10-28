using System.Collections.ObjectModel;
using Article.Api.Models;

namespace Article.Api.Repository;

public interface IArticleRepository
{
    ReadOnlyCollection<ArticleDto> GetAll();
    ArticleDto? GetById(int id);
    int Delete(int id);
}
