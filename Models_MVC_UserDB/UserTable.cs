using System;
using System.Collections.Generic;

namespace WebApplication2022_Core8_WebApi_JWT.Models_MVC_UserDB
{
    public partial class UserTable
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserSex { get; set; }
        public DateTime? UserBirthDay { get; set; }
        public string? UserMobilePhone { get; set; }
    }
}
