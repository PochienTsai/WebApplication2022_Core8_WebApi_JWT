using System;
using System.Collections.Generic;

namespace WebApplication2022_Core8_WebApi_JWT.Models_MVC_UserLogin
{
    public partial class DbUserRight
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string UserPassword { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? EmailMd5id { get; set; }
        public Guid? EmailGuid { get; set; }
        public string? OldUserPassword { get; set; }
        public DateTime? EnableTime { get; set; }
        public int? UserRank { get; set; }
        public string? UserApproved { get; set; }
        public string? UpdateRight { get; set; }
        public string? DeleteRight { get; set; }
    }
}
