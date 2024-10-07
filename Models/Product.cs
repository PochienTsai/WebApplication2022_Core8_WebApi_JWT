using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2022_Core8_WebApi_JWT.Models
{
    /// <summary>
    /// 檢視畫面（UI網頁），放在 /wwwroot目錄底下，名為 index_Products.html
    /// 類別檔，放在 /Models/Product.cs
    /// WebAPI控制器 - /Controllers/ProductsController
    /// </summary>
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
    }
}