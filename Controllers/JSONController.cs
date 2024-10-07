using Microsoft.AspNetCore.Mvc;

using WebApplication2022_Core8_WebApi_JWT.Models_MVC_UserDB;  // 自己動手寫上命名空間 -- 「專案名稱.Models」。
//*********************************自己加寫（宣告）的NameSpace  ***務必先安裝 NuGet
//using System.Web.Script.Serialization;   // 舊版 .NET 4.x版使用。
// .NET Core 已經改成 System.Text.Json，請自行從 NuGet安裝。https://learn.microsoft.com/zh-tw/dotnet/standard/serialization/system-text-json/how-to
using System.Text.Json;   //** 自己宣告、加入。JSON會用到。JavaScriptSerializer

using Newtonsoft.Json;   //** 自己宣告、加入。JSON.NET會用到。請自行從 NuGet安裝


// ADO.NET會用到的命名空間 ***務必先安裝 NuGet
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;    // ConfigurationBuilder會用到這個命名空間
using System.IO;                                            // Directory會用到這個命名空間
using System.Data;   // DataSet會用到。

using System.Text.Encodings.Web;
//*********************************


namespace WebApplication2022_Core8_WebApi_JWT.Controllers
{
    /// <summary>
    /// 這是一個 MVC控制器，並非 WebAPI
    /// </summary>
    /// 

    public class JSONController : Controller
    {
        // GET: JSON
        // 線上格式化JSON的網站  http://jsoneditoronline.org/
        // JSON格式進行驗證的網站  http://zaach.github.io/jsonlint/

        #region    //*****  連結 MVC_UserDB 資料庫 ***** (自己手動添加、宣告的。)
        private readonly MVC_UserDBContext _db;

        public JSONController(MVC_UserDBContext context)   // *** 記得自己修改一下，修改成「控制器名稱」
        {
            _db = context;
        }
        #endregion


        public IActionResult Index()
        {
            return View();
        }



        /// <summary>
        /// LINQ & JSON。搭配 /Models/UserTable.cs。  缺點：輸出的「日期格式」怪怪的。
        /// 資料來源  https://stackoverflow.com/questions/6126151/can-i-get-javascriptserializer-to-serialize-a-linq-result-hierarchically
        /// </summary>
        /// <returns></returns>
        public IActionResult Index_JSON1()
        {
            var data = from u in _db.UserTables
                       select new
                       {
                           UserId = u.UserId,
                           UserName = u.UserName,
                           UserMobilePhone = u.UserMobilePhone
                       };

            // .NET Core改用 System.Text.Json。詳見 https://learn.microsoft.com/zh-tw/dotnet/standard/serialization/system-text-json/how-to
            string result = System.Text.Json.JsonSerializer.Serialize(data);

            return Content(result);
        }


        /// <summary>
        /// ADO.NET -- DataSet & JSON。缺點：輸出的「日期格式」怪怪的。
        /// </summary>
        /// <returns></returns>
        public IActionResult Index_JSON2()
        {
            //== 方法一 ==
            //string connectionString = @"Server=.\SqlExpress;Database=資料庫的名稱;Trusted_Connection=True;TrustServerCertificate=true;MultipleActiveResultSets=true";
            ////== 方法二 == 改成appsettings.json    http://stackoverflow.com/questions/38282652/asp-net-core-1-0-configurationbuilder-addjsonfileappsettings-json-not-fin
            var configurationBuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            // 讀取設定檔的內容

            IConfiguration config = configurationBuilder.Build();
            string connectionString = config["ConnectionStrings:DefaultConnection"];

            var Conn = new SqlConnection(connectionString);
            Conn.Open();

            SqlDataAdapter myAdapter = new SqlDataAdapter("SELECT [UserId],[UserName],[UserSex],[UserBirthDay],[UserMobilePhone] FROM [UserTable]", Conn);
            DataSet ds = new DataSet();   // System.Data命名空間

            try  //==== 以下程式，只放「執行期間」的指令！====
            {
                //Conn.Open();  //---- 這一列註解掉，不用寫，DataAdapter會自動開啟
                myAdapter.Fill(ds, "UserTable");    // 這時候執行SQL指令。取出資料，放進 DataSet。
                //---- DataSet是由許多 DataTable組成的，我們目前只放進一個名為 test的 DataTable而已。

                //******************************************************************************(start)
                //資料來源 http://www.codeproject.com/Tips/624888/Converting-DataTable-to-JSON-Format-in-Csharp-and
                List<Dictionary<string, object>> result_rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row = null;

                foreach (DataRow drow in ds.Tables[0].Rows)
                {
                    row = new Dictionary<string, object>();    // 一筆記錄
                    foreach (DataColumn col in ds.Tables[0].Columns)
                    {
                        row.Add(col.ColumnName.Trim(), drow[col]);   // 加入一個欄位 與 值
                    }
                    result_rows.Add(row);
                }

                // .NET Core改用 System.Text.Json。詳見 https://learn.microsoft.com/zh-tw/dotnet/standard/serialization/system-text-json/how-to
                string result = System.Text.Json.JsonSerializer.Serialize(result_rows);
                return Content(result);
                //******************************************************************************(end)
            }
            catch (Exception ex)
            {
                return Content("<hr /> Exception Error Message----  " + ex.ToString());
            }
            finally
            {  //---- 不用寫，DataAdapter會自動關閉
            }
            //return View();
        }


        /// <summary>
        /// ADO.NET -- DataSet & JSON。缺點：輸出的「日期格式」怪怪的。
        /// </summary>
        /// <returns></returns>
        public IActionResult Index_JSON_DateTime()
        {
            string result = "";
            // 兩種 JSON的日期時間格式。
            string[] jsonDates = { "/Date(1242357713797+0800)/", "/Date(1027008000000)/" };

            foreach (string jsonDate in jsonDates)
            {
                //                         // **************  
                DateTime dtResult = JsonToDateTime(jsonDate);   // 這段程式放在最下方，請自行參考
                // 別人寫的function   *********** 放在本程式（控制器）最末端。

                result += "<hr />原始格式：" + jsonDate.ToString() + "<br />";
                result += String.Format("DateTime: {0}", dtResult.ToString("yyyy-MM-dd hh:mm:ss ffffff"));
            }

            return Content(result);
        }


        /// <summary>
        /// ADO.NET -- DataSet & JSON。
        /// 搭配 DLL類別庫 (WindoswDesktopClassLibrary1)裡面的「JsonHelper.cs」。
        /// </summary>
        /// <returns></returns>
        //public IActionResult JSON_3_AdoNet_JsonHelper()
        //{
        //string result = "";
        //SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MVC_UserDB"].ConnectionString);
        //SqlDataAdapter myAdapter = new SqlDataAdapter("SELECT [UserId],[UserName],[UserSex],[UserBirthDay],[UserMobilePhone] FROM [UserTable]", Conn);

        //DataSet ds = new DataSet();

        //try  //==== 以下程式，只放「執行期間」的指令！====
        //{
        //    // Conn.Open();  //---- 這一列註解掉，不用寫，DataAdapter會自動開啟
        //    myAdapter.Fill(ds, "UserTable");    //這時候執行SQL指令。取出資料，放進 DataSet。
        //    //---- DataSet是由許多 DataTable組成的，我們目前只放進一個名為 UserTable的 DataTable而已。

        //    //*****************************************************************(start)
        //    //** 搭配 DLL類別庫 WindoswDesktopClassLibrary1 裡面的「JsonHelper.cs」
        //    WindoswDesktopClassLibrary1.JsonHelper JH = new WindoswDesktopClassLibrary1.JsonHelper();

        //    // 產生三種結果（三種輸出）：
        //    result += "<hr />**** DataSet ****<br />" + JH.ToJson(ds) + "<br /><br />";
        //    result += "<hr />*** DataTable ***<br />" + JH.ToJson(ds.Tables[0]) + "<br /><br />";
        //    result += "<hr />*** 自己添加的功能，可產生「欄位名稱」與「值」 ***<br />" + JH.ToJson2Column(ds.Tables[0]);
        //    //*****************************************************************(end)

        //    return Content(result);
        //}
        //catch (Exception ex)
        //{
        //    return Content("<hr /> Exception Error Message----  " + ex.ToString());
        //}
        //finally
        //{   //---- 不用寫，DataAdapter會自動關閉
        //    //    if (Conn.State == ConnectionState.Open)  {
        //    //          Conn.Close();
        //    //          Conn.Dispose();
        //    //    }  //使用SqlDataAdapter的時候，不需要寫程式去控制Conn.Open()與 Conn.Close()。
        //}
        //}


        /// <summary>
        /// ADO.NET -- DataSet & JSON。
        /// 搭配 Newtonsoft.Json。
        /// </summary>
        /// <returns></returns>
        public IActionResult JSON_4_JsonNET()
        {
            //== 方法一 ==
            //string connectionString = @"Server=.\SqlExpress;Database=資料庫的名稱;Trusted_Connection=True;TrustServerCertificate=true;MultipleActiveResultSets=true";
            ////== 方法二 == 改成appsettings.json    http://stackoverflow.com/questions/38282652/asp-net-core-1-0-configurationbuilder-addjsonfileappsettings-json-not-fin
            var configurationBuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            // 讀取設定檔的內容

            IConfiguration config = configurationBuilder.Build();
            string connectionString = config["ConnectionStrings:DefaultConnection"];

            var Conn = new SqlConnection(connectionString);
            Conn.Open();

            string result = "";
            SqlDataAdapter myAdapter = new SqlDataAdapter("SELECT [UserId],[UserName],[UserSex],[UserBirthDay],[UserMobilePhone] FROM [UserTable]", Conn);
            DataSet ds = new DataSet();   // System.Data命名空間
                        
            try  //==== 以下程式，只放「執行期間」的指令！====
            {
                //Conn.Open();  //---- 這一行註解掉，不用寫，DataAdapter會自動開啟
                myAdapter.Fill(ds, "UserTable");    //這時候執行SQL指令。取出資料，放進 DataSet。
                                                    //---- DataSet是由許多 DataTable組成的，我們目前只放進一個名為 UserTable的 DataTable而已。

                //***********************************************************************(start)
                //資料來源： http://james.newtonking.com/json/help/index.html?topic=html/SerializeDataSet.htm
                // 跟上一個範例的差異在此：
                result += JsonConvert.SerializeObject(ds, Formatting.Indented);
                result += JsonConvert.SerializeObject(ds, Formatting.None);
                //***********************************************************************(end)
                return Content(result);
            }
            catch (Exception ex)
            {
                return Content("<hr /> Exception Error Message----  " + ex.ToString());
            }
            finally
            {  //---- 不用寫，DataAdapter會自動關閉
            }
        }



        //**********************************************************************
        //*** 把JSON的日期格式轉回來DateTime ****************************
        // 資料來源： http://www.cnblogs.com/coolcode/archive/2009/05/22/1487254.html
        //**********************************************************************
        public static DateTime JsonToDateTime(string jsonDate)
        {
            string value = jsonDate.Substring(6, jsonDate.Length - 8);
            DateTimeKind kind = DateTimeKind.Utc;
            int index = value.IndexOf('+', 1);
            if (index == -1)
                index = value.IndexOf('-', 1);
            if (index != -1)
            {
                kind = DateTimeKind.Local;
                value = value.Substring(0, index);
            }
            long javaScriptTicks = long.Parse(value, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture);
            long InitialJavaScriptDateTicks = (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Ticks;
            DateTime utcDateTime = new DateTime((javaScriptTicks * 10000) + InitialJavaScriptDateTicks, DateTimeKind.Utc);
            DateTime dateTime;
            switch (kind)
            {
                case DateTimeKind.Unspecified:
                    dateTime = DateTime.SpecifyKind(utcDateTime.ToLocalTime(), DateTimeKind.Unspecified);
                    break;
                case DateTimeKind.Local:
                    dateTime = utcDateTime.ToLocalTime();
                    break;
                default:
                    dateTime = utcDateTime;
                    break;
            }
            return dateTime;
        }




    }
}
