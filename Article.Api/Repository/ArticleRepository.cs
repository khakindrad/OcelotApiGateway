using Article.Api.Models;

namespace Article.Api.Repository
{
    public class ArticleRepository : List<ArticleDto>, IArticleRepository
    {
        private readonly static List<ArticleDto> _articles = Populate();

        private static List<ArticleDto> Populate()
        {
            var result = new List<ArticleDto>();
            var dateTime = DateTime.Now;
            for (int i = 0; i < 10; i++)
            {
                result.Add(new ArticleDto { Id = i, Title = $"Article {i}", WriterId = i, LastUpdate = dateTime.AddMinutes(i) });
            }
            return result;
        }

        public List<ArticleDto> GetAll()
        {
            return _articles;
        }

        public ArticleDto? GetById(int id)
        {
            return _articles.FirstOrDefault(x => x.Id == id);
        }

        public int Delete(int id)
        {
            var removed = _articles.SingleOrDefault(x => x.Id == id);
            if (removed != null)
                _articles.Remove(removed);

            return removed?.Id ?? 0;
        }
    }
}
