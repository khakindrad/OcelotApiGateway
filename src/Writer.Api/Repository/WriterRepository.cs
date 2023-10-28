using Writer.Api.Models;

namespace Writer.Api.Repository;

public sealed class WriterRepository : IWriterRepository
{
    private static readonly List<WriterDto> _writers = Populate();

    private static List<WriterDto> Populate()
    {
        var result = new List<WriterDto>();
        var dateTime = DateTime.UtcNow;
        for (var i = 0; i < 10; i++)
        {
            result.Add(new WriterDto { Id = i, Name = $"Writer {i}", CreatedDate = dateTime.AddMinutes(i) });
        }
        return result;
    }

    public IReadOnlyCollection<WriterDto> GetAll()
    {
        return _writers.AsReadOnly();
    }

    public WriterDto Create(WriterDto writer)
    {
        var maxId = _writers.Max(x => x.Id);

        writer.Id = maxId + 1;
        _writers.Add(writer);

        return writer;
    }

    public WriterDto? GetById(int id)
    {
        return _writers.Find(x => x.Id == id);
    }
}
