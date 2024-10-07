using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication2022_Core8_WebApi_JWT.Controllers
{
    /// <summary>
    ///  .NET 4.x版的 WebAPI範例，轉到 .NET 8.0專案運作
    ///  原本的專案名稱是「WebApplication2017_WebAPI2_Starter」
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class WebAPI2Controller : ControllerBase
    {
        //參考來源：  https://docs.microsoft.com/zh-tw/aspnet/web-api/overview/getting-started-with-aspnet-web-api/tutorial-your-first-web-api
        [HttpGet]   // .NET Core比較嚴格，必須寫清楚
        public string GetHello()
        {
            return "Hello(1)! The World.....（沒有輸入值）";
        }

        // 跟上一個做比對，這個方法需要輸入一個參數
        // 錯誤學習法，如果修改參數名稱，不使用「預設的 id名稱」會怎麼樣？

        [HttpGet("{id}")]   // .NET Core比較嚴格，必須寫清楚  https://learn.microsoft.com/zh-tw/aspnet/core/mvc/controllers/routing?view=aspnetcore-8.0
        public string GetHelloWorld(int id)
        {
            return "Hello(2)! 你好，這個結果需搭配您的輸入值 --" + id.ToString();
        }


        // 輸入「多個參數」，不用修改「路由設定檔」
        [HttpGet("{a}/{b}")]   // .NET Core比較嚴格，必須寫清楚 https://learn.microsoft.com/zh-tw/aspnet/core/mvc/controllers/routing?view=aspnetcore-8.0
        public string GetComputeIT(int a, int b)
        {
            int result =a + b;
            return result.ToString();
        }
    }
}
