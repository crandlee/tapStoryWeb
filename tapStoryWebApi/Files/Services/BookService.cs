using System.Linq;
using AutoMapper;
using tapStoryWebApi.Common.Services;
using tapStoryWebApi.Files.DTO;
using tapStoryWebData.EF.Contexts;
using tapStoryWebData.EF.Models;

namespace tapStoryWebApi.Files.Services
{
    public class BookService : IDataService
    {

        private readonly AuditService _auditService;
        private readonly ApplicationDbContext _ctx;

        public BookService(ApplicationDbContext ctx, AuditService auditService)
        {
            _auditService = auditService;
            _ctx = ctx;
        }

        public IQueryable<BookModel> GetBookFileGroups()
        {
            var bfgs =  _ctx.FileGroups.Include("Files").Where(e => e.FileGroupType == FileGroupType.Book).
                ToArray().Select(CreateBookFileGroup).AsQueryable();
            return bfgs;
        } 


        //Create BookFileGroup ViewModel
        private static BookModel CreateBookFileGroup(FileGroup fg)
        {
            Mapper.CreateMap<FileGroup, BookModel>()
                .ForMember(dest => dest.BookName, opt => opt.MapFrom(src => src.GroupName));
            var bfg = Mapper.Map<BookModel>(fg);
            return bfg;
        }
    }
}