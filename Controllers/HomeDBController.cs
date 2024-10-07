using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//***** 請自己加入這些 NameSpace *****
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using WebApplication2022_Core8_WebApi_JWT.JwtServices;     // 放在 /JwtServices 目錄底下
using WebApplication2022_Core8_WebApi_JWT.Models_MVC_UserDB;     // 連結資料庫，使用UserTable
using Microsoft.AspNetCore.Cors;
//**************************************


namespace WebApplication2022_Core8_WebApi_JWT.Controllers
{
    // 資料來源：https://medium.com/the-innovation/asp-net-core-3-authorization-and-authentication-with-bearer-and-jwt-3041c47c8b1d

    // 請先裝這些 Nuget套件 -- 
    //(1) Microsoft.AspNetCore.Authentication
    //(2) Microsoft.AspNetCore.Authentication.JwtBearer  // JWT會用到
    //(3) Microsoft.EntityFrameworkCore               // 資料庫會用到
    //(4) Microsoft.EntityFrameworkCore.Tools    // 資料庫會用到
    //(5) Microsoft.EntityFrameworkCore.SqlServer  // SQL Server會用到
    //(6) System.Security.Claims

    [Route("api/[controller]")]    // （注意！這是 API控制器！）
    [ApiController]                        // （注意！這是 API控制器！）
    public class HomeDBController : ControllerBase
    {

        #region    //*****  連結 MVC_UserDB 資料庫 ***** (自己手動添加、宣告的。)
        private readonly MVC_UserDBContext _db;

        public HomeDBController(MVC_UserDBContext context)   // *** 記得自己修改一下，修改成「控制器名稱」
        {
            _db = context;
        }
        #endregion


        [HttpPost]
        //[Route("login")]
        [AllowAnonymous]
        public IActionResult Post([FromBody] UserTable model)
        {
            // 連結DB的 UserTable資料表，請參考 （MVC 入門 第三天課程）的 Details動作，共有四種寫法。
            // 請輸入UserName = MIS2000 Lab.  與 UserId = 1
            var ListOne = from m in _db.UserTables
                          where m.UserName == model.UserName && m.UserId == model.UserId
                          select m;
            UserTable? user = ListOne.FirstOrDefault();  // 執行上面的查詢句，得到 "第一筆" 結果。

            if (user == null)
                return NotFound(new { message = "查無此人，可能帳號或密碼有錯！" });
            else
            {
                string token = TokenService.CreateToken(user);   // 程式放在 /JwtServices 目錄底下
                return Ok(new
                {
                    //message,
                    user,
                    token     // OK代表成功，Http Status = 200
                });
            }
            // (1) Postman - 請自己動手輸入 Body => raw => JSON的內容
            //            {
            //                "UserName":"mis2000lab",
            //                "UserId": "100"
            //            }

            //            // Postman - 傳回成果
            //            {
            //                "message": "查無此人，可能帳號或密碼有錯！"
            //            }


            // (2) Postman - 請自己動手輸入 Body => raw => JSON的內容
            //必須是真正在 UserTable資料表裡面的紀錄
            //        {
            //            "UserName":"MIS2000 Lab.",
            //            "UserId": "1"
            //        }
            //        //            // Postman - 傳回成果  ( 請參閱圖片  JWT_Postman.jpg 與 JWT_Debug.jpg )
            //        {
            //            "user": {
            //                "userId": 1,
            //                "userName": "MIS2000 Lab.",
            //                "userSex": "M",
            //                "userBirthDay": "1990-01-01T00:00:00",
            //                "userMobilePhone": "0933123456"
            //            },
            //           "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6Ik1JUzIwMDAgTGFiLiIsInJvbGUiOiIxIiwibmJmIjoxNjIyOTA2ODk3LCJleHAiOjE2MjI5MTA0OTcsImlhdCI6MTYyMjkwNjg5N30.4spXAxMXG8pis7nfblyXlRyNJswoxpFH2aIMigOMX2s"
            //        }
        }

        [HttpGet]
        //[Route("anonymous")]    // 資料來源 (REST Api 的屬性路由)  https://docs.microsoft.com/zh-tw/aspnet/core/mvc/controllers/routing?view=aspnetcore-5.0
        [AllowAnonymous]
        //public string Anonymous()
        //{
        //    return "You are Anonymous（匿名登入）.  -- 透過[HttpGet]";
        //}
        public IActionResult Anonymous()
        {
            // 連結DB的 UserTable資料表，請參考 （MVC 入門 第三天課程）的 Details動作，共有四種寫法。
            // 請輸入UserName = MIS2000 Lab.  與 UserId = 1
            var ListOne = from m in _db.UserTables
                                  where m.UserName == "MIS2000 Lab." && m.UserId == 1
                                  select m;
            UserTable? user = ListOne.FirstOrDefault();  // 執行上面的查詢句，得到 "第一筆" 結果。


                string token = TokenService.CreateToken(user);   // 程式放在 /JwtServices 目錄底下
                return Ok(new
                {
                    //message,
                    user,
                    token     // OK代表成功，Http Status = 200
                });

        }


        //==========================================================

        [HttpGet]
        [Route("authenticated")] 
        [Authorize]       // *** 取得 token 的人才能使用這裡的方法 ***     以前 MVC5 / .NET Core MVC「會員登入」就學過的東西！一模一樣。
        public string Authenticated() => String.Format("Authenticated - 歡迎光臨，尊貴的 {0} 閣下", User.Identity.Name);

        // ****** Postman 測試步驟（一）：
        //    搭配圖片 Authorize_1-1.jpg
        //    (1) 別忘了改成 GET 再送出喔。因為您上面的程式寫著 [HttpGet]。
        //    (2) 確認上面 Authrozation 寫著「NoAuth」再送出。您會被阻擋下來，因為沒有取得token，算是非法登入。

        // ***** Postman 測測試步驟（二）：
        //    搭配圖片 Authorize_1-1.jpg
        //    (1) 別忘了改成 GET 再送出喔。因為您上面的程式寫著 [HttpGet]。
        //    (2) 用Postman先輸入JSON，並取得 token
        //    //        {
        //    //            "UserName":"神雕大俠",
        //    //            "UserId": "4"
        //    //        }
        //    (3) 把 token 的「值」先複製下來，等一下會用到。
        //
        //    搭配圖片 Authorize_2-1.jpg的三張圖片
        //    (4) 到Postman輸入網址 ，最後加上 authenticated 字樣
        //    (5) 把 Authrozation 的「NoAuth」改成「BearerToken」，輸入Key為「token」，Value為「.....」（你在第二步驟抄下來的值）。
        //    (6) 別忘了改成 GET 再送出喔。因為您上面的程式寫著 [HttpGet]。


        //========== 以下是角色、群組、權限的判定，回家作業（HomeWork）會示範給您看。 =================
        //[HttpGet]
        ////[Route("tester")]
        //[Authorize(Roles = "tester")]
        //public string Tester()
        //{
        //    return "You are a Tester";
        //}

        //[HttpGet]
        ////[Route("employee")]
        //[Authorize(Roles = "employee,manager")]
        //public string Employee() => "Employee";

        //[HttpGet]
        ////[Route("manager")]
        //[Authorize(Roles = "manager")]
        //public string Manager() => "Manager";

    }
}
