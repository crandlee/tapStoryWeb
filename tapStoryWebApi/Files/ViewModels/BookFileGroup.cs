using System.Collections.Generic;
using tapStoryWebData.EF.Models;

namespace tapStoryWebApi.Files.ViewModels
{
    public class BookFileGroup
    {
        public int Id { get; set; }
        public string BookName { get; set; }
        public virtual ICollection<File> Files { get; set; }
    }
}