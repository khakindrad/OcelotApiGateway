using Writer.Api.Models;

namespace Writer.Api.Repository
{
    public class WriterRepository : IWriterRepository
    {
        private readonly static List<WriterDto> _writers = Populate();

        private static List<WriterDto> Populate()
        {
            var result = new List<WriterDto>();
            var dateTime = DateTime.Now;
            for (int i = 0; i < 10; i++)
            {
                result.Add(new WriterDto { Id = i, Name= $"Writer {i}", CreatedDate = dateTime.AddMinutes(i) });
            }
            return result;
        }

        public List<WriterDto> GetAll()
        {
            return _writers;
        }

        public WriterDto Create(WriterDto writer)
        {
            var maxId = _writers.Max(x => x.Id);

            writer.Id = ++maxId;
            _writers.Add(writer);

            return writer;
        }

        public WriterDto? GetById(int id)
        {
            return _writers.FirstOrDefault(x => x.Id == id);
        }
    }
}
