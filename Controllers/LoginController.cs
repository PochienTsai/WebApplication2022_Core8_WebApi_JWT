using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication2022_Core8_WebApi_JWT.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    // 在一般的操作方法中無需特別標記，將應用預設的全局CORS策略（允許所有來源）。
    // 在特定的操作方法中使用[EnableCors("AllowSpecificOrigins")]特性明確應用僅允許特定來源的CORS策略。
    // 這個控制器中的所有操作方法允許所有來源
    // GET: api/<LoginController>
    [HttpGet]
    public IEnumerable<string> Get()
    {
        // 允許所有來源的操作
        return new string[] { "value1", "value2" };
    }

    // 這個操作方法僅允許特定來源
    [EnableCors("AllowSpecificOrigins")]
    // GET api/<LoginController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        // 僅允許特定來源的操作
        return "value";
    }

    // POST api/<LoginController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/<LoginController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<LoginController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}