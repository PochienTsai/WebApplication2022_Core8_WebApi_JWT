//****************************************
// 連結資料庫才會用到這一段（DB連結字串）

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using WebApplication2022_Core8_WebApi_JWT.Models_MVC_UserDB; // （第二個範例才會用到）  連結資料庫
using WebApplication2022_Core8_WebApi_JWT.Models_MVC_UserLogin; // （[回家作業 HomeWork] 才會用到）  連結資料庫
//****************************************
// JWT (json web token) 才會用到這一段
using Microsoft.AspNetCore.Authentication.JwtBearer;
using WebApplication2022_Core8_WebApi_JWT.JwtServices; // 位於 /JwtServices目錄下，Token源自於此。
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebApplication2022_Core8_WebApi_JWT.Filter;
using WebApplication2022_Core8_WebApi_JWT.Models;
using WebApplication2022_Core8_WebApi_JWT.Models_MVC_UserDB;
using WebApplication2022_Core8_WebApi_JWT.Models_MVC_UserLogin;

//****************************************
// 資料來源：https://medium.com/the-innovation/asp-net-core-3-authorization-and-authentication-with-bearer-and-jwt-3041c47c8b1d
//         （跟上面這一篇相同）https://levelup.gitconnected.com/asp-net-5-authorization-and-authentication-with-bearer-and-jwt-2d0cef85dc5d
//                    https://www.cnblogs.com/nsky/p/10312101.html
//                    https://medium.com/@szaloki/jwt-authentication-between-asp-net-core-and-angular-part-1-asp-net-core-315af889fdce
//                    https://www.c-sharpcorner.com/article/authentication-and-authorization-in-asp-net-5-with-jwt-and-swagger/
//                    https://www.c-sharpcorner.com/article/how-to-use-jwt-authentication-with-web-api/

// 請先裝這些 Nuget套件 --
//(1) Microsoft.AspNetCore.Authentication
//(2) Microsoft.AspNetCore.Authentication.JwtBearer  // JWT會用到
//(3) Microsoft.EntityFrameworkCore               // 資料庫會用到
//(4) Microsoft.EntityFrameworkCore.Tools    // 資料庫會用到
//(5) Microsoft.EntityFrameworkCore.SqlServer  // SQL Server會用到
//(6) System.Security.Claims

// 本範例搭配 HomeController（注意！這是 API控制器！）


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// 添加 CORS 服務
//避免Client端寫JS時，遇見問題「跨原始來源要求 (CORS)  / Access - Control - Allow - Origin」
builder.Services.AddCors(options =>
{
    // 配置全局允許所有來源的CORS策略(預設）
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin() // 允許任何來源
            .AllowAnyMethod() // 允許所有HTTP方法（GET、POST、PUT、DELETE等）
            .AllowAnyHeader(); // 允許所有標頭
    });

    // 配置僅允許特定來源的CORS策略
    options.AddPolicy("AllowSpecificOrigins", builder =>
    {
        builder.WithOrigins("http://example.com", "http://anotherexample.com")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
builder.Services.AddControllers();

#region // （第一個範例）  JWT (json web token) 才會用到這一段

//********************************************************************
// 這裡需要新增 很多的命名空間，請使用「顯示可能的修正」讓系統自己加上。
var key = Encoding.UTF8.GetBytes(Settings.Secret); // 位於 /JwtServices目錄下的Settings類別（搭配 System.Text命名空間）

builder.Services.AddAuthentication(z =>
    {
        z.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        z.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;

        // 當驗證失敗時，回應標頭會包含 WWW-Authenticate 標頭，這裡會顯示失敗的詳細錯誤原因
        x.IncludeErrorDetails = true; // 預設值為 true

        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,

            // 底下三個設定屬性也可以寫在 appsettings.json檔案。https://www.cnblogs.com/nsky/p/10312101.html
            IssuerSigningKey = new SymmetricSecurityKey(key),
            // 或是寫成 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("您自己輸入的Secret Hash數值"))
            // (1) 用來雜湊 (Hash) 打亂的關鍵數值

            ValidateIssuer = false, // (2) 是誰核發的？  (false 不驗證)
            ValidateAudience = false // (3) 哪些客戶（Client）可以使用？  (false 不驗證)

            // === TokenValidationParameters的參數 (預設值) ====
            // https://docs.microsoft.com/zh-tw/dotnet/api/microsoft.identitymodel.tokens.tokenvalidationparameters?view=azure-dotnet
            // RequireSignedTokens = true,
            // SaveSigninToken = false,
            // ValidateActor = false,

            // 將下面兩個參數設置為false，就不會驗證 Issuer和 Audience，但是不建議這樣做。
            // ValidateAudience = true,     // 是誰核發的？
            // ValidateIssuer = true,            // 哪些客戶（Client）可以使用？
            // ValidateIssuerSigningKey = false,  // 如果token 中包含 key 才需要驗證，一般都只有簽章而已

            // 是否要求Token的Claims中必須包含Expires（過期時間）
            // RequireExpirationTime = true,

            // 允許的伺服器時間偏移量
            // ClockSkew = TimeSpan.FromSeconds(300),

            // 是否驗證 token有效期間，使用當前時間與 token的 Claims中的NotBefore和Expires對比
            // ValidateLifetime = true
        };
    });
// 完成後，底下還有一段 app.UseAuthentication();   需要自己動手加上
//********************************************************************

#endregion

#region // （第二個範例才會用到）  連結資料庫才會用到這一段（DB連結字串）

//********************************************************************
// 這裡的關鍵字<MVC_UserDBContext>，
// 請跟 /Models_MVC_UserDB目錄下「MVC_UserDBContext.cs」類別名稱一模一樣。

//**** 讀取 appsettings.json 設定檔裡面的資料（資料庫連結字串）****
////作法一：
builder.Services.AddDbContext<MVC_UserDBContext>(options =>
    options.UseSqlServer(
        "Server=.\\sqlexpress;Database=MVC_UserDB;Trusted_Connection=True;TrustServerCertificate=true;MultipleActiveResultSets=true"));
//// 這裡需要新增兩個命名空間，請使用「顯示可能的修正」讓系統自己加上。

////作法二： 讀取設定檔的內容
//// 資料來源  程式碼  https://github.com/CuriousDrive/EFCore.AllDatabasesConsidered/blob/main/MS%20SQL%20Server/Northwind.MSSQL/Program.cs
//builder.Services.AddDbContext<MVC_UserDBContext>(
//        options =>
//        {
//            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
//        });

////作法三： 讀取 appsetting.json設定檔的內容
//// (3-A) 可運作
//var configurationBuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
//IConfiguration config = configurationBuilder.Build();   // ConfigurationBuilder會用到 Microsoft.Extensions.Configuration命名空間
//builder.Services.AddDbContext<MVC_UserDB2Context>(options => options.UseSqlServer(config["ConnectionStrings:DefaultConnection"]));  // appsettings.josn檔裡面的「資料庫連結字串」

//// (3-B) 可運作
//IConfiguration config = builder.Configuration;   // ConfigurationBuilder會用到 Microsoft.Extensions.Configuration命名空間
//builder.Services.AddDbContext<MVC_UserDB2Context>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")));  // appsettings.josn檔裡面的「資料庫連結字串」


// (1)  這裡需要新增 Microsoft.EntityFrameworkCore 命名空間，請使用「顯示可能的修正」讓系統自己加上。
// (2)  請在 appsettings.json設定檔裡面，自己加上  "ConnectionStrings": { ...... }
// (3)  完成以後，請重新「建置專案」檢查是否還有錯誤？
//********************************************************************

#endregion

#region // （[回家作業 HomeWork] 才會用到）  連結資料庫才會用到這一段（DB連結字串）

//********************************************************************
// 這裡的關鍵字<MVC_UserLoginContext>，
// 請跟 /Models_MVC_UserLogin目錄下「MVC_UserLoginContext.cs」類別名稱一模一樣。

//**** 讀取 appsettings.json 設定檔裡面的資料（資料庫連結字串）****
////作法一：
builder.Services.AddDbContext<MVC_UserLoginContext>(options =>
    options.UseSqlServer(
        "Server=.\\sqlexpress;Database=MVC_UserLogin;Trusted_Connection=True;TrustServerCertificate=true;MultipleActiveResultSets=true"));
//// 這裡需要新增兩個命名空間，請使用「顯示可能的修正」讓系統自己加上。

////作法二： 讀取設定檔的內容
//// 資料來源  程式碼  https://github.com/CuriousDrive/EFCore.AllDatabasesConsidered/blob/main/MS%20SQL%20Server/Northwind.MSSQL/Program.cs
//builder.Services.AddDbContext<MVC_UserDBContext>(
//        options =>
//        {
//            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
//        });

////作法三： 讀取設定檔的內容
//var configurationBuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
//IConfiguration config = configurationBuilder.Build();   // ConfigurationBuilder會用到 Microsoft.Extensions.Configuration命名空間
//string DBconnectionString = config["ConnectionStrings:HomeLoginConnection"];  // appsettings.josn檔裡面的「資料庫連結字串」
//builder.Services.AddDbContext<MVC_UserLoginContext>(options => options.UseSqlServer(DBconnectionString));

// (1)  這裡需要新增 Microsoft.EntityFrameworkCore 命名空間，請使用「顯示可能的修正」讓系統自己加上。
// (2)  請在 appsettings.json設定檔裡面，自己加上  "ConnectionStrings": { ...... }
// (3)  完成以後，請重新「建置專案」檢查是否還有錯誤？
//********************************************************************

#endregion


// [補充範例]-- 微軟範例 WebAPI的 ToDoList（Api控制器，名為 TodoItemsController）
// 需要手動安裝  NuGet 套件 -- Microsoft.EntityFrameworkCore.InMemory
builder.Services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoList"));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // API 服務簡介
    options.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Version = "v1",
            Title = "ASP.NET Core Web API",
            Description = "ASP.NET Core Web API Sample" // 描述
            // TermsOfService = new Uri("https://igouist.github.io/"),
            // Contact = new OpenApiContact
            // {
            //     Name = "Brian",
            //     Email = string.Empty,
            //     Url = new Uri("https://igouist.github.io/about/"),
            // }
        }
    );
    //說明api如何受到保護
    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            //選擇類型，type選擇http時，透過swagger畫面做認證時可以省略Bearer前綴詞
            //Type使用SecuritySchemeType.ApiKey，需要打Bearer與空白以及文字描述會包含Name、In、Description
            Type = SecuritySchemeType.Http,
            //採用Bearer token方式
            Scheme = "Bearer",
            //bearer格式使用jwt
            BearerFormat = "JWT",
            //認證放在http request的header上
            In = ParameterLocation.Header,
            //描述
            Description = "JWT驗證描述"
        }
    );
    options.OperationFilter<AuthorizeCheckOperationFilter>();
    // 設定 Swagger 產生的 XML 檔案路徑 目的是為了可以讀取我們所寫的註解
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

//=== 分 隔 線 ===============================================================


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // ASP.NET Core 中的靜態檔案
// https://learn.microsoft.com/zh-tw/aspnet/core/fundamentals/static-files?view=aspnetcore-7.0

app.UseCors(); // 使用 CORS middleware(這將應用預設的CORS策略（允許所有來源))
//**************************************************************
// JWT (json web token) 才會用到這一段。必須放在 app.UseAuthorization();之前，順序不能錯！
app.UseAuthentication(); // ** JWT 請自己動手加上這一段 **
//***************************************************************
app.UseAuthorization(); // 順序不能錯！

//app.MapControllers();  // WebAPI專案預設的值
//為了執行 "JSON控制器"，我才從MVC專案複製了下面這一段設定
app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");


app.Run();