namespace Article.Api.Models
{
    public class ArticleDto
    {
        public required int Id { get; set; }
        public string? Title { get; set; }
        public required DateTime LastUpdate { get; set; }
        public required int WriterId { get; set; }
    }
}