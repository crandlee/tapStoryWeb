using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using tapStoryWebApi.Common.Extensions;

namespace tapStoryWebApi.Files.Services
{
    public class LocalPhysicalFileService : IPhysicalFileService
    {
        private readonly string _basePath;

        public LocalPhysicalFileService(string basePath)
        {
            _basePath = (basePath.Length > 0 && basePath.Substring(basePath.Length - 1) == "\\") ? basePath : basePath + "\\";

        }

        public string CreateFileGroup(string fileGroupServerId = null)
        {
            if (fileGroupServerId == null) fileGroupServerId = Guid.NewGuid().ToString();
            if (String.IsNullOrEmpty(_basePath)) throw new InvalidOperationException("No base path defined.  Cannot create file group.");
            if (!Directory.Exists(_basePath + fileGroupServerId)) Directory.CreateDirectory(_basePath + fileGroupServerId);
            return fileGroupServerId;
        }

        public void RemoveFileGroup(string fileGroupServerId)
        {            
            if (String.IsNullOrEmpty(_basePath)) throw new InvalidOperationException("No base path defined.  Cannot remove file group.");
            if (Directory.Exists(_basePath + fileGroupServerId)) Directory.Delete(_basePath + fileGroupServerId, true);
        }

        public void ClearFileGroup(string fileGroupServerId)
        {
            if (String.IsNullOrEmpty(_basePath)) throw new InvalidOperationException("No base path defined.  Cannot clear file group.");
            if (!Directory.Exists(_basePath + fileGroupServerId)) return;
            var directory = new DirectoryInfo(_basePath + fileGroupServerId);
            directory.Empty();
        }

        public IEnumerable<HttpPostedFile> AddFiles(string fileGroupServerId, IEnumerable<HttpPostedFile> files)
        {
            var serverPath = _basePath + fileGroupServerId + "\\";
            if (!Directory.Exists(serverPath)) throw new InvalidOperationException("Cannot add files.  File group requested does not exist or is not valid.");
            var httpPostedFiles = files as HttpPostedFile[] ?? files.ToArray();
            foreach (var file in httpPostedFiles)
            {
                if (String.IsNullOrEmpty(file.ServerId)) file.ServerId = Guid.NewGuid().ToString();
                if (File.Exists(serverPath + file.ServerId)) file.Exists = true;
                File.WriteAllBytes(serverPath + file.ServerId, file.File.ToArray());
            }
            return httpPostedFiles;
        }

        public void RemoveFiles(string fileGroupServerId, IEnumerable<string> filePaths)
        {
            if (!Directory.Exists(_basePath + fileGroupServerId)) throw new InvalidOperationException("Cannot delete files.  File group requested does not exist or is not valid.");
            foreach (var filePath in filePaths.Where(filePath => !String.IsNullOrEmpty(filePath) && File.Exists(filePath)))
            {
                File.Delete(filePath);                
            }
        }

        public IEnumerable<HttpPostedFile> OverwriteFileGroup(string fileGroupServerId, IEnumerable<HttpPostedFile> files)
        {
            ClearFileGroup(fileGroupServerId);
            CreateFileGroup(fileGroupServerId);
            return AddFiles(fileGroupServerId, files);
        }

        public IEnumerable<HttpPostedFile> GetFiles(string fileGroupServerId)
        {
            var filesInFolder = Directory.GetFiles(_basePath + fileGroupServerId);
            return (from file in filesInFolder let fileContents = File.ReadAllBytes(file) let fileServerId = Path.GetFileName(file) select new HttpPostedFile(fileServerId, file, fileContents, fileServerId)).ToList();
        }

        public HttpPostedFile GetFile(string fileGroupServerId, string fileServerId)
        {
            var file = _basePath + fileGroupServerId + "\\" + fileServerId;
            var fileContents = File.ReadAllBytes(file);
            return new HttpPostedFile(Path.GetFileName(file), file, fileContents, fileServerId);
        }
    }
}