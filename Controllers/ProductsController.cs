using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//********************************************************
using WebApplication2022_Core8_WebApi_JWT.Models;
//********************************************************

/// <summary>
/// 檢視畫面（UI網頁），放在 /wwwroot目錄底下，名為 index_Products.html
/// 類別檔，放在 /Models/Product.cs
/// </summary>
namespace WebApplication2022_Core8_WebApi_JWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        #region  // 加入預設值（基本資料）
        // 程式沒有防呆，ID數字請勿重複！
        Product[] products = new Product[]
        {
            new Product { Id = 1, Name = "Tomato Soup", Category = "Groceries", Price = 1 },
            new Product { Id = 2, Name = "Yo-yo", Category = "Toys", Price = 3.75M },
            new Product { Id = 3, Name = "Hammer", Category = "Hardware", Price = 16.99M },
            new Product { Id = 4, Name = "張中文", Category = "Toys", Price = 5 },
            new Product { Id = 5, Name = "李法文", Category = "Hardware", Price = 6M },
            new Product { Id = 6, Name = "許功蓋(PHP)", Category = "Groceries", Price = 7 }
        };
        #endregion


        // 沒有輸入值。列出全部的資料。
        //[HttpGet]   // .NET Core嚴格要求，必填！
        //public IEnumerable<Product> GetAllProducts()
        //{   // 原廠範例    // 傳回值 -- (4) 其他類型
        //    return products;  
        //}

        [HttpGet]   // .NET Core嚴格要求，必填！
        public IActionResult GetAllProducts()
        {   // 傳回值 -- (3)  IActionResult ( .NET Core的 WebAPI有變化）
            return Ok(products);
            // returns an HttpStatusCode.OK   // https://docs.microsoft.com/zh-tw/previous-versions/aspnet/dn308866%28v%3dvs.118%29
        }

        //===========================================================

        // 需要一個輸入值。只列出「單一筆」資料。
        // 傳回值 -- (3)  IActionResult ( .NET Core的 WebAPI有變化）
        //[HttpGet("{id}")]   // .NET Core嚴格要求，必填！
        //public IActionResult GetProduct(int id)
        //{   // 原廠範例  // 傳回值 -- (3)  IHttpActionResult
        //    var product = products.FirstOrDefault((p) => p.Id == id);
        //    if (product == null)   {
        //        return NotFound();  // 找不到。這也是 Http.Results的一種，需搭配 IHttpActionResult使用
        //    }
        //    return Ok(product);
        //    // returns an HttpStatusCode.OK   // https://docs.microsoft.com/zh-tw/previous-versions/aspnet/dn308866%28v%3dvs.118%29
        //}

        [HttpGet("{id}")]   // .NET Core嚴格要求，必填！
        public Product GetProduct(int id)
        {
            var pd = products.FirstOrDefault((p) => p.Id == id);
            if (pd == null)
            {
                Product pdx = new Product()
                {
                    Name = "找不到",
                    Price = 0
                };
                return pdx;
            }
            return pd;
        }

        //===========================================================
        // https://docs.microsoft.com/zh-tw/aspnet/web-api/overview/getting-started-with-aspnet-web-api/action-results
        // Web API 控制器動作可以傳回：
        //  (1)  void -- 傳回 "空"。204 （沒有內容）
        //  (2)  HttpResponseMessage -- 直接將轉換的 HTTP 回應訊息。
        //
        //  (3)  IActionResult -- 呼叫 .ExecuteAsync()來建立HttpResponseMessage，然後將轉換為 HTTP 回應訊息。
        //         ( .NET Core的 WebAPI有變化）
        //         
        //  (4)  其他類型 -- 將 序列化 的傳回值寫入至回應主體中; 傳回 200 （確定）。
        //                            缺點是您不能直接傳回錯誤碼 404 等。 不過，您可以擲回HttpResponseException錯誤代碼
        //===========================================================


    }
}
