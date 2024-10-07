using Microsoft.AspNetCore.Mvc;
using System.Drawing;

//********************************************************
using WebApplication2022_Core8_WebApi_JWT.Models;
//********************************************************
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication2022_Core8_WebApi_JWT.Controllers
{
    /// <summary>
    ///  [FromBody]範例，必須搭配 /Models目錄下的 JSONViewModel.cs
    ///  
    ///  Client端，前端測試程式，可使用 /wwwroot目錄下的 index4_FromBody_JSON.html
    /// 搭配 Server端（WebAPI控制器，ValuesController）
    /// </summary>


    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET: api/<ValuesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }


        // [FromBody]只能使用一次。 https://docs.microsoft.com/zh-tw/aspnet/web-api/overview/formats-and-model-binding/parameter-binding-in-aspnet-web-api
        // 當參數具有 [FromBody] 時，Web API 會使用 Content-type 標頭來選取格式器。
        // 在此範例中，內容類型為 " application/json "  而且要求主體是原始的 json "字串"，"不是" json 物件。

        // 最多可以有一個參數從訊息主體讀取。 下面這種寫法 "無法運作"：
        // public HttpResponseMessage Post([FromBody] int id, [FromBody] string name) { ... }
        // 這項規則的原因是要求主體可能儲存在 "只能讀取一次" 的非緩衝資料流程中。

        [HttpPost]
        public IActionResult Put([FromBody] JSONViewModel jsonViewModel)
        {
            jsonViewModel.Name += "**Updated（經過修改）**";
            // 把 Client端傳來的JSON，裡面的某一數值修改。

            return Ok(jsonViewModel);
            //如果您要自己輸入JSON文件，請輸入下面內容： （注意「雙引號」必須寫得工整，不然會報錯！）       
            //                   {
            //                   "Name": "MIS2000Lab",
            //                   "Id": 123,
            //                   "Tags": [{ "Id": 12, "Code": "C" }, { "Id": 33, "Code": "Swift" }]
            //                   }

        }




        //// 預設的範例。
        // POST api/<ValuesController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<ValuesController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
