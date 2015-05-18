using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tapStoryWebData.EF.Models
{
    public enum AuditTable
    {
        Users = 0,
        Roles = 1,
        FileGroup = 2,
        Files = 3,
        UserFileGroups = 4,
        UserRelationship = 5
    }
}
