using System.Collections.Generic;

namespace tapStoryWebApi.Files.ViewModels
{
    public class UserStoryVm
    {
        public int Id { get; set; }
        public string StoryName { get; set; }
        public virtual IEnumerable<FileVm> OdFiles { get; set; }
        public int UserId { get; set; }
    }
}