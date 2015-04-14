﻿using Microsoft.AspNet.Identity.EntityFramework;

namespace tapStoryWebData.Identity.Models
{
    public class ApplicationUserRole : IdentityUserRole<int>
    {
        public virtual ApplicationRole Role { get; set; }
    }
}