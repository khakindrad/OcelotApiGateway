using Writer.Api.Models;

namespace Writer.Api.Repository;

public interface IWriterRepository
{
    IReadOnlyCollection<WriterDto> GetAll();
    WriterDto? GetById(int id);
    WriterDto Create(WriterDto writer);
}
