using System.Collections.Generic;

namespace tapStoryWebApi.Files.DTO
{
    public class UserStoryModel
    {
        public int Id { get; set; }
        public string StoryName { get; set; }
        public virtual IEnumerable<FileModel> OdFiles { get; set; }
        public int UserId { get; set; }
    }
}