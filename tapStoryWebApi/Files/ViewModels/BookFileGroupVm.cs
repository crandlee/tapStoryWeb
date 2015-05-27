using System.Collections.Generic;

namespace tapStoryWebApi.Files.ViewModels
{
    public class BookFileGroupVm
    {
        public int Id { get; set; }
        public string BookName { get; set; }
        public virtual IEnumerable<FileVm> OdFiles { get; set; }
        public string ServerId { get; set; }
    }
}