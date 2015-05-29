using System.Collections.Generic;
using tapStoryWebApi.Common.Extensions;

namespace tapStoryWebApi.Files.Services
{
    public interface IPhysicalFileService
    {

        string CreateFileGroup(string fileGroupServerId = null);
        void RemoveFileGroup(string fileGroupServerId);

        IEnumerable<HttpPostedFile> AddFiles(string fileGroupServerId, IEnumerable<HttpPostedFile> files);
        void RemoveFiles(string fileGroupServerId, IEnumerable<string> filePaths);

        IEnumerable<HttpPostedFile> OverwriteFileGroup(string fileGroupServerId, IEnumerable<HttpPostedFile> files);

        IEnumerable<HttpPostedFile> GetFiles(string fileGroupServerId);
        HttpPostedFile GetFile(string fileGroupServerId, string fileServerId);

    }
}
