using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2022_Core8_WebApi_JWT.JwtServices
{
    public static class Settings   // 注意 關鍵字 static
    {
        public static string Secret = "MIS2000Lab20210605ABCDEFGHIJK1234567890";   // 建議修改這裡的數值，改成您自己的！
        // 注意 ***** 關鍵字 static

        // 也可以寫在 appsettings.json檔案。請參閱 https://www.cnblogs.com/nsky/p/10312101.html
    }
}
