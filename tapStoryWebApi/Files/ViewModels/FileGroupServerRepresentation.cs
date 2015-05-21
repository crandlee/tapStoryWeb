using System.Collections.Generic;

namespace tapStoryWebApi.Files.ViewModels
{
    public class FileGroupServerRepresentation
    {
        public string FileGroupServerId { get; set; }
        public List<FileServerRepresentation> Files { get; set; } 
    }

    public class FileServerRepresentation
    {
        public string FileServerId { get; set; }
        public string FileName { get; set; }
        public string FileLocation { get; set; }
    }
}