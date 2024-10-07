using System;
using System.Collections.Generic;

namespace WebApplication2022_Core8_WebApi_JWT.Models_MVC_UserLogin
{
    public partial class DbUser
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string UserPassword { get; set; } = null!;
        public int? UserRank { get; set; }
        public string? UserApproved { get; set; }
    }
}
