using tapStoryWebData.EF.Models;

namespace tapStoryWebApi.Files.DTO
{
    public class FileModel
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileLocation { get; set; }
        public FileType FileType { get; set; }
        public string ServerId { get; set; }
        public int FileGroupId { get; set; }
    }
}