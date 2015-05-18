using System;
using tapStoryWebData.EF.Contexts;
using tapStoryWebData.EF.Models;

namespace tapStoryWebApi.Common.Services
{
    public class AuditService
    {

        private readonly ApplicationUser _user;
        private readonly ApplicationDbContext _ctx;

        public AuditService(ApplicationDbContext ctx, ApplicationUser user)
        {
            _ctx = ctx;
            _user = user;
        }

        public void AddAuditRecord(AuditTable table, AuditRecordType recordType,
            string additionalInformation = null, bool saveContextAfter = false)
        {
            var userId = (_user != null) ? _user.Id : 0;
           
            _ctx.AuditRecords.Add(new Audit()
            {
                AdditionalInformation = additionalInformation,
                RecordType = recordType,                
                AuditTime = DateTime.Now,
                AuditUser = userId,
                TableName = table
            });

            if (saveContextAfter)
                _ctx.SaveChanges();
        }

        public async void AddAuditRecordAsync(AuditTable table, AuditRecordType recordType,
            string additionalInformation = null)
        {

            var userId = (_user != null) ? _user.Id : 0;
            _ctx.AuditRecords.Add(new Audit()
            {
                AdditionalInformation = additionalInformation,
                RecordType = recordType,
                AuditTime = DateTime.Now,
                AuditUser = userId,
                TableName = table
            });
            await _ctx.SaveChangesAsync();
        }
    }
}