using System.Collections.Generic;

namespace tapStoryWebApi.Files.DTO
{
    public class BookModel
    {
        public int Id { get; set; }
        public string BookName { get; set; }
        public virtual IEnumerable<FileModel> OdFiles { get; set; }
        public string ServerId { get; set; }
    }
}