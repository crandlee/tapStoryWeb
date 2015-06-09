using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using tapStoryWebApi.Attributes;
using tapStoryWebApi.Common.Exceptions;
using tapStoryWebApi.Common.Extensions;
using tapStoryWebApi.Common.Services;
using tapStoryWebApi.Files.DTO;
using tapStoryWebData.EF.Contexts;
using tapStoryWebData.EF.Models;

namespace tapStoryWebApi.Files.Services
{
    public class FileDataService : IDataService
    {

        private readonly AuditService _auditService;
        private readonly ApplicationDbContext _ctx;

        private readonly Expression<Func<File, FileModel>> _getFileVmFromFile = file => new FileModel()
        {
            Id = file.Id,
            FileType = file.FileType,
            FileName = file.FileName,
            FileLocation = file.FileLocation,
            ServerId = file.ServerId,
            FileGroupId = file.FileGroupId
        };


        public FileDataService(ApplicationDbContext ctx, AuditService auditService)
        {
            _auditService = auditService;
            _ctx = ctx;
        }

        //Get File Group
        public IQueryable<FileGroup> GetFileGroup(int fileGroupId)
        {
            return _ctx.FileGroups.Include("Files").Where(e => e.Id == fileGroupId);
        }
        public IQueryable<FileGroup> GetFileGroup(string fileGroupServerId)
        {
            return _ctx.FileGroups.Include("Files").Where(e => e.ServerId == fileGroupServerId);
        } 

        //Get User File Groups
        public IQueryable<UserStoryModel> GetUserFileGroups(int? fileGroupId = null)
        {

            return from ufg in _ctx.UserFileGroups
                join fg in _ctx.FileGroups on ufg.FileGroupId equals fg.Id
                where fg.FileGroupType == FileGroupType.UserStory && (fg.Id == fileGroupId || fileGroupId == null)
                select
                    new UserStoryModel()
                    {
                        StoryName = fg.GroupName,
                        Id = fg.Id,
                        UserId = ufg.UserId,
                        OdFiles = fg.Files.AsQueryable().Select(_getFileVmFromFile).AsEnumerable()
                    };
        }

        //Get Book File Groups
        public IQueryable<BookModel> GetBookFileGroups()
        {
            return from fg in _ctx.FileGroups
                   where fg.FileGroupType == FileGroupType.Book
                   select new BookModel()
                   {
                       BookName = fg.GroupName,
                       Id = fg.Id,
                       OdFiles = fg.Files.AsQueryable().Select(_getFileVmFromFile).AsEnumerable()
                   };
        }

        //Get Files for File Group Id
        public IQueryable<FileModel> GetFileGroupFiles(int fileGroupId)
        {
            return _ctx.Files.Where(f => f.FileGroupId == fileGroupId).Select(_getFileVmFromFile);
        }


        public IQueryable<FileModel> GetFiles(int? fileId = null)
        {
            return _ctx.Files.Where(f => f.Id == fileId || fileId == null).Select(_getFileVmFromFile);
        }

        public void AddFileGroup(FileGroup fg)
        {
            _ctx.FileGroups.Add(fg);
            _auditService.AddAuditRecord(AuditTable.FileGroup, AuditRecordType.Created, fg.ServerId);
            if (fg.Files == null) return;
            foreach (var f in fg.Files)
            {
                _auditService.AddAuditRecord(AuditTable.Files, AuditRecordType.Created, f.ServerId);
            }
        }

        public FileGroup GetNewFileGroupFromContentFiles(string fileGroupServerId, string groupName, FileGroupType fileGroup, IEnumerable<HttpPostedFile> files)
        {
            ICollection<File> dataFiles = files.Select(addedFile => new File() { FileLocation = addedFile.Filename, FileType = FileType.Content, ServerId = addedFile.ServerId, FileName = addedFile.Name }).ToList();
            return new FileGroup() { FileGroupType = FileGroupType.UserStory, GroupName = groupName, ServerId = fileGroupServerId, Files = dataFiles };
   
        }

        public void UpdateFilesWithServerIds(FileGroup fg, IEnumerable<HttpPostedFile> files)
        {
            if (fg.Files == null || fg.Files.Count == 0) return;

            files.ToList().ForEach(file => { var matchFgFile = fg.Files.FirstOrDefault(fgFile => fgFile.FileLocation == file.Filename);
                                                             if (matchFgFile != null)
                                                                 file.ServerId = matchFgFile.ServerId;
            });
        } 

        public FileGroup RemoveFileGroupFiles(FileGroup fg, IEnumerable<HttpPostedFile> files)
        {
            if (fg.Files == null) return fg;
            fg.Files.ToList().ForEach(df => { _ctx.Entry(df).State = EntityState.Deleted; _auditService.AddAuditRecord(AuditTable.Files, AuditRecordType.Deleted, df.Id.ToString());  });
            return fg;
        }

        public FileGroup AddFileGroupFilesUsingContentFiles(FileGroup fg, IEnumerable<HttpPostedFile> files)
        {
            var filesNotInData = files.Where(af => (fg.Files == null) || !fg.Files.Select(df => df.ServerId).Contains(af.ServerId));
            if (fg.Files == null) fg.Files = new List<File>();
            foreach (var af in filesNotInData)
                {
                    fg.Files.Add(new File()
                    {
                        FileLocation = af.Filename,
                        FileType = FileType.Content,
                        ServerId = af.ServerId,
                        FileName = af.Name
                    });
                    _auditService.AddAuditRecord(AuditTable.Files, AuditRecordType.Created, af.ServerId);
                }
            return fg;
        }
        
        //Delete File Group
        public void DeleteFileGroup(FileGroup f)
        {
            _ctx.FileGroups.Remove(f);
            _auditService.AddAuditRecord(AuditTable.FileGroup, AuditRecordType.Deleted, f.Id.ToString());
        }


        //Remove File From File Group
        [ReferenceServiceFunction("FileGroups", "Files", ReferenceServiceFunctionType.Delete)]
        public void RemoveFileFromFileGroup(int fileGroupId, int fileId)
        {

            var fileGroup = _ctx.FileGroups.FirstOrDefault(fg => fg.Id == fileGroupId);
            if (fileGroup == null) throw new DataNotFoundException(String.Format("No file group with id {0} was found", fileGroupId));

            var file = _ctx.Files.FirstOrDefault(f => f.Id == fileId);
            if (file == null) return;
            _ctx.Files.Remove(file);
            _auditService.AddAuditRecord(AuditTable.Files, AuditRecordType.Deleted, file.Id.ToString());
        }


        //Associate File Group With User
        public void AssociateFileGroupWithUser(int userId, int fileGroupId)
        {
            var existing = _ctx.UserFileGroups.FirstOrDefault(ufg => ufg.UserId == userId && ufg.FileGroupId == fileGroupId);
            if (existing != null) return;
            _ctx.UserFileGroups.Add(new UserFileGroup() {FileGroupId = fileGroupId, UserId = userId});
            _auditService.AddAuditRecord(AuditTable.UserFileGroups, AuditRecordType.Created, String.Format("{0},{1}", userId, fileGroupId));
        }

        public void AssociateFileGroupWithUser(int userId, FileGroup fg)
        {
            var existing = _ctx.UserFileGroups.FirstOrDefault(ufg => ufg.UserId == userId && ufg.FileGroupId == fg.Id);
            if (existing != null) return;
            _ctx.UserFileGroups.Add(new UserFileGroup() { FileGroup = fg, UserId = userId });
            _auditService.AddAuditRecord(AuditTable.UserFileGroups, AuditRecordType.Created, String.Format("{0},{1}", userId, fg.ServerId));
        }

    }
}