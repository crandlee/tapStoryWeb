using System;
using System.Collections.Generic;
using System.Linq;
using tapStoryWebApi.Common.Services;
using tapStoryWebApi.Exceptions;
using tapStoryWebData.Identity.Contexts;
using tapStoryWebData.Identity.Models;

namespace tapStoryWebApi.Files.Services
{
    public class FileService
    {

        private readonly AuditService _auditService;
        private readonly ApplicationDbContext _ctx;

        public FileService(ApplicationDbContext ctx, AuditService auditService)
        {
            _auditService = auditService;
            _ctx = ctx;
        }

        //Get File Groups Not Associated With User
        public IQueryable<FileGroup> GetBookFileGroups()
        {
            return _ctx.FileGroups.Where(fg => fg.FileGroupType == FileGroupType.Book);
        } 

        //Get File Groups For a User
        public IQueryable<FileGroup> GetUserStoryFileGroups(int userId)
        {
            return _ctx.UserFileGroups.Where(ufg => ufg.UserId == userId).Select(ufg => ufg.FileGroup);
        } 

        //Get Files For a File Group
        public IQueryable<File> GetFiles(int fileGroupId)
        {
            return _ctx.Files.Where(f => f.FileGroupId == fileGroupId);
        } 

        //Add File Group
        public void AddFileGroup(FileGroup f)
        {
            _ctx.FileGroups.Add(f);
            _auditService.AddAuditRecord(AuditTable.FileGroup, AuditRecordType.Created);
        }

        //Delete File Group
        public void DeleteFileGroup(FileGroup f)
        {
            //TODO: Remove Files from File Group

            _ctx.FileGroups.Remove(f);

        }

        //Add File To File Group
        public void AddFilesToFileGroup(int fileGroupId, IEnumerable<File> files)
        {

            var fileGroup = _ctx.FileGroups.FirstOrDefault(fg => fg.Id == fileGroupId);
            if (fileGroup == null) throw new DataNotFoundException(String.Format("No file group with id {0} was found", fileGroupId));
            foreach (var f in files)
            {
                //TODO: Upload File

                _ctx.Files.Add(f);                    
            }
        }

        //Remove File From File Group
        public void RemoveFileFromFileGroup(int fileGroupId, int fileId)
        {

            var fileGroup = _ctx.FileGroups.FirstOrDefault(fg => fg.Id == fileGroupId);
            if (fileGroup == null) throw new DataNotFoundException(String.Format("No file group with id {0} was found", fileGroupId));

            var file = _ctx.Files.FirstOrDefault(f => f.Id == fileId);
            if (file != null)
            {

                //TODO: Delete file
                _ctx.Files.Remove(file);
            }

        }

        //Associate File Group With User
        public void AssociateFileGroupWithUser(int userId, int fileGroupId)
        {
            var existing = _ctx.UserFileGroups.FirstOrDefault(ufg => ufg.UserId == userId && ufg.FileGroupId == fileGroupId);
            if (existing == null)
                _ctx.UserFileGroups.Add(new UserFileGroup() {FileGroupId = fileGroupId, UserId = userId});
        }

    }
}