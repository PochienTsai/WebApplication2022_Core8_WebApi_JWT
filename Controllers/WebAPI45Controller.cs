using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//***** 請自己加入這些 NameSpace *****
using WebApplication2022_Core8_WebApi_JWT.Models_MVC_UserDB;     // 連結資料庫，使用UserTable
//*****************************************

namespace WebApplication2022_Core8_WebApi_JWT.Controllers
{
    /// <summary>
    ///  .NET 4.x版的 WebAPI範例，轉到 .NET 8.0專案運作
    ///  原本的專案名稱是「WebApplication2017_WebAPI2_Starter」
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class WebAPI45Controller : ControllerBase
    {

        #region    //*****  連結 MVC_UserDB 資料庫 ***** (自己手動添加、宣告的。)
        private readonly MVC_UserDBContext _db;

        public WebAPI45Controller(MVC_UserDBContext context)   // *** 記得自己修改一下，修改成「控制器名稱」
        {
            _db = context;
        }
        #endregion



        ////===================================
        ////== 列表（Master） ==  無分頁功能。
        ////===================================
        ////[HttpGet]
        ////public IEnumerable<UserTable> GetList()
        ////{   // 傳統寫法
        ////    IQueryable<UserTable> ListAll = from _userTable in _db.UserTables
        ////                                                    select _userTable;
        ////    // 或是寫成
        ////    //var ListAll = from m in _db.UserTables
        ////    //                   select m;

        ////    return ListAll.ToList();
        ////    // 直到程式的最後，把查詢結果 IQueryable呼叫.ToList()時，上面那一段LINQ才會真正被執行！
        ////}


        // .NET 4.x版的寫法
        // public IHttpActionResult GetList()
        [HttpGet]
        public IActionResult GetList()
        {   // 傳回值--(3)  IHttpActionResult
            IQueryable<UserTable> ListAll = from _userTable in _db.UserTables
                                            select _userTable;
            return Ok(ListAll.ToList());
            // returns an HttpStatusCode.OK   // https://docs.microsoft.com/zh-tw/previous-versions/aspnet/dn308866%28v%3dvs.118%29
        }



        ////===================================
        ////== 列出一筆記錄的明細（Details） ==
        ////===================================
        ////[HttpGet]
        ////public UserTable GetDetails(int id)  
        ////{   // 傳統寫法                                              
        ////    UserTable ut = _db.UserTables.Find(id);

        ////    return ut;
        ////}

        // .NET 4.x版的寫法
        // public IHttpActionResult GetProduct(int id)
        [HttpGet("{id}")]  //****雖然也是 HttpGet，但必須輸入 id
        public IActionResult GetProduct(int id)
        {   // 傳回值 -- (3)  IHttpActionResult
            UserTable ut = _db.UserTables.Find(id);
            if (ut == null)
            {
                return NotFound();  // 找不到。這也是 Http.Results的一種，需搭配 IHttpActionResult使用
            }
            return Ok(ut);
            // returns an HttpStatusCode.OK   // https://docs.microsoft.com/zh-tw/previous-versions/aspnet/dn308866%28v%3dvs.118%29
        }


    }
}
