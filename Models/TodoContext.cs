using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace WebApplication2022_Core8_WebApi_JWT.Models
{
    /// <summary>
    /// 微軟範例下載 https://github.com/dotnet/AspNetCore.Docs/tree/main/aspnetcore/tutorials/first-web-api/samples
    /// 官方說明文件 https://learn.microsoft.com/zh-tw/aspnet/core/tutorials/first-web-api?view=aspnetcore-7.0&tabs=visual-studio
    /// 搭配 /Models目錄。請搭配 wwwroot目錄下的 index.html 來使用。
    /// </summary>
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }

        public DbSet<TodoItem> TodoItems { get; set; } = null!;
    }
}