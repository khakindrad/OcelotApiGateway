namespace Writer.Api.Models;

public sealed class WriterDto
{
    public required int Id { get; set; }
    public required string? Name { get; set; }
    public required DateTime? CreatedDate { get; set; }
}
