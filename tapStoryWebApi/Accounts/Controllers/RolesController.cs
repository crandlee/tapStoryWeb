﻿using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;
using Microsoft.AspNet.Identity.Owin;
using tapStoryWebApi.Accounts.Services;
using tapStoryWebData.Identity.Contexts;
using tapStoryWebData.Identity.Models;

namespace tapStoryWebApi.Accounts.Controllers
{
    [Authorize]
    public class RolesController : ODataController
    {

        private ApplicationDbContext GetDbContext()
        {
            return Request.GetOwinContext().Get<ApplicationDbContext>();
        }

        [EnableQuery]
        public IQueryable<ApplicationRole> Get()
        {
            return RoleService.GetRoles(GetDbContext());
        }

        [EnableQuery]
        public SingleResult<ApplicationRole> Get([FromODataUri] int key)
        {
            return SingleResult.Create(RoleService.GetRole(GetDbContext(), key));
        }

        [EnableQuery]
        public IQueryable<ApplicationUserRole> GetUsers([FromODataUri] int key)
        {
            var ur = RoleService.GetUserRolesForRole(GetDbContext(), key);
            return ur;
        }
    }
}