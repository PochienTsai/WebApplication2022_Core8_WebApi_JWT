namespace WebApplication2022_Core8_WebApi_JWT.Models
{
    /// <summary>
    /// 微軟範例下載 https://github.com/dotnet/AspNetCore.Docs/tree/main/aspnetcore/tutorials/first-web-api/samples
    /// 官方說明文件 https://learn.microsoft.com/zh-tw/aspnet/core/tutorials/first-web-api?view=aspnetcore-7.0&tabs=visual-studio
    /// 搭配 /Models目錄。請搭配 wwwroot目錄下的 index.html 來使用。     
    /// </summary>
    public class TodoItem
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public bool IsComplete { get; set; }
    }
}