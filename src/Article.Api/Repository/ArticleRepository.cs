using System.Collections.ObjectModel;
using Article.Api.Models;

namespace Article.Api.Repository;

public sealed class ArticleRepository : List<ArticleDto>, IArticleRepository
{
    private static readonly Collection<ArticleDto> _articles = Populate();

    private static Collection<ArticleDto> Populate()
    {
        var result = new Collection<ArticleDto>();
        var dateTime = DateTime.UtcNow;
        for (var i = 0; i < 10; i++)
        {
            result.Add(new ArticleDto { Id = i, Title = $"Article {i}", WriterId = i, LastUpdate = dateTime.AddMinutes(i) });
        }
        return result;
    }

    public ReadOnlyCollection<ArticleDto> GetAll()
    {
        return _articles.AsReadOnly();
    }

    public ArticleDto? GetById(int id)
    {
        return _articles.FirstOrDefault(x => x.Id == id);
    }

    public int Delete(int id)
    {
        var removed = _articles.SingleOrDefault(x => x.Id == id);
        if (removed != null)
        {
            _articles.Remove(removed);
        }

        return removed?.Id ?? 0;
    }
}
