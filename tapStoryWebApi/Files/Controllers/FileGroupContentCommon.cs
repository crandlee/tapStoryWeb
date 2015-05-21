using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using tapStoryWebApi.Common.ActionResults;
using tapStoryWebApi.Extensions;
using tapStoryWebApi.Files.Services;
using tapStoryWebData.EF.Models;

namespace tapStoryWebApi.Files.Controllers
{
    public class FileGroupContentCommon
    {

        public enum FileGroupCreateMode
        {
            Create = 0,
            Edit = 1,
            Overwrite = 2
        }

        private readonly ApiController _ctrl;
        private readonly DbContext _ctx;
        private readonly IPhysicalFileService _fileContentService;
        private readonly FileDataService _fileDataService;
        private readonly bool _hasAssociatedUser;
        private readonly FileGroupType _fileGroupType;

        public FileGroupContentCommon(ApiController ctrl, DbContext ctx, IPhysicalFileService fileContentService,
            FileDataService fileDataService, bool hasAssociatedUser, FileGroupType fileGroupType)
        {
            _ctrl = ctrl;
            _ctx = ctx;
            _fileContentService = fileContentService;
            _fileDataService = fileDataService;
            _hasAssociatedUser = hasAssociatedUser;
            _fileGroupType = fileGroupType;
        }

        public async Task<IHttpActionResult> SaveFileGroup(FileGroupCreateMode mode)
        {
            //Parse and Validate the incoming multipart fields
            var data = await _ctrl.Request.Content.ParseMultipartAsync();
            if (data.Files.Count <= 0) return new NoContentActionResult();
            if (!data.Fields.ContainsKey("GroupName") || !data.Fields.ContainsKey("UserId") || !data.Fields["UserId"].Value.IsNumeric(NumberStyles.Integer)) return new BadRequestResult(_ctrl);

            var groupName = data.Fields["GroupName"].Value;
            var userId = Convert.ToInt32(data.Fields["UserId"].Value);
            String fileGroupServerId = null;
            if (data.Fields.ContainsKey("FileGroupServerId"))
                fileGroupServerId = data.Fields["FileGroupServerId"].Value;

            //Create the directory file group
            var files = data.Files.Values;
            IEnumerable<HttpPostedFile> addedFiles;
            FileGroup fg;
            switch (mode)
            {
                case FileGroupCreateMode.Create:
                    fileGroupServerId = _fileContentService.CreateFileGroup();
                    addedFiles = _fileContentService.AddFiles(fileGroupServerId, files);
                    fg = _fileDataService.GetNewFileGroupFromContentFiles(fileGroupServerId, groupName, _fileGroupType, addedFiles);
                    _fileDataService.AddFileGroup(fg);
                    if (_hasAssociatedUser) _fileDataService.AssociateFileGroupWithUser(userId, fg);
                    await _ctx.SaveChangesAsync();
                    return new CreatedActionResult<FileGroup>(_ctrl.Request, _ctrl.Request.RequestUri + "/" + fg.Id, fg);
                case FileGroupCreateMode.Overwrite:
                case FileGroupCreateMode.Edit:
                    fg = _fileDataService.GetFileGroup(fileGroupServerId).FirstOrDefault();
                    _fileDataService.UpdateFilesWithServerIds(fg, files);
                    if (fg == null) return new BadRequestResult(_ctrl);
                    addedFiles = mode == FileGroupCreateMode.Overwrite
                        ? _fileContentService.OverwriteFileGroup(fileGroupServerId, files).ToList()
                        : _fileContentService.AddFiles(fileGroupServerId, files).ToList();

                    if (mode == FileGroupCreateMode.Overwrite)
                    {
                        fg = _fileDataService.RemoveFileGroupFiles(fg, addedFiles);
                    }
                    fg = _fileDataService.AddFileGroupFilesUsingContentFiles(fg, addedFiles);
                    await _ctx.SaveChangesAsync();
                    return new OkActionResult<FileGroup>(_ctrl.Request, fg);
                default:
                    return new BadRequestResult(_ctrl);
            }

        }


        public async Task<IHttpActionResult> DeleteFileGroup(string fileGroupServerId)
        {
            var fg = _fileDataService.GetFileGroup(fileGroupServerId).FirstOrDefault();
            if (fg == null) return new NotFoundResult(_ctrl);
            var serverId = fg.ServerId;
            _fileDataService.DeleteFileGroup(fg);
            _fileContentService.RemoveFileGroup(serverId);
            await _ctx.SaveChangesAsync();
            return new OkResult(_ctrl);
        }

        public IHttpActionResult GetFileGroup(string fileGroupServerId)
        {
            var fg = _fileDataService.GetFileGroup(fileGroupServerId).FirstOrDefault();
            if (fg == null) return new NotFoundResult(_ctrl);
            return new OkActionResult<FileGroup>(_ctrl.Request, fg);
        }
    }
}