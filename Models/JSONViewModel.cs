using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2022_Core8_WebApi_JWT.Models
{
    // Client端傳來JSON格式（Server端註明 [FromBody]）
    // 資料來源 https://stackoverflow.com/questions/20226169/how-to-pass-json-post-data-to-web-api-method-as-an-object
    // 搭配 Client端（/wwwroort目錄下的 index4_FromBody_JSON.html）
    // 搭配 Server端（控制器，ValuesController）

    public class JSONViewModel    // 複雜的 ViewModel。
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public List<TagViewModel> Tags { set; get; }
    }

    public class TagViewModel
    {
        public int Id { set; get; }
        public string Code { set; get; }
    }
}