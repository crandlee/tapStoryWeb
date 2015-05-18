using System;
using System.Linq;
using tapStoryWebApi.Attributes;
using tapStoryWebApi.Common.Services;
using tapStoryWebApi.Exceptions;
using tapStoryWebData.EF.Contexts;
using tapStoryWebData.EF.Models;

namespace tapStoryWebApi.Files.Services
{
    public class FileService : IDataService
    {

        private readonly AuditService _auditService;
        private readonly ApplicationDbContext _ctx;

        public FileService(ApplicationDbContext ctx, AuditService auditService)
        {
            _auditService = auditService;
            _ctx = ctx;
        }

        //Get File Group
        public IQueryable<FileGroup> GetFileGroup(int fileGroupId)
        {
            return _ctx.FileGroups.Include("Files").Where(e => e.Id == fileGroupId);
        } 

        //Get Files For a File Group
        public IQueryable<File> GetFiles(int fileId)
        {
            return _ctx.Files.Where(f => f.Id == fileId);
        }

        //Add File Group
        public void AddFileGroup(FileGroup f)
        {
            _ctx.FileGroups.Add(f);

            //TODO: Add files to file group

            _auditService.AddAuditRecord(AuditTable.FileGroup, AuditRecordType.Created);
        }

        //Delete File Group
        public void DeleteFileGroup(FileGroup f)
        {
            //TODO: Remove Files from File Group

            _ctx.FileGroups.Remove(f);

        }


        //Remove File From File Group
        [ReferenceServiceFunction("FileGroups", "Files", ReferenceServiceFunctionType.Delete)]
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