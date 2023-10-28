namespace Article.Api.Models;

public sealed class ArticleDto
{
    public required int Id { get; set; }
    public string? Title { get; set; }
    public required DateTime LastUpdate { get; set; }
    public required int WriterId { get; set; }
}