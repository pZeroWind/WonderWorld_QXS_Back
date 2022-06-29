using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WWModel.Models;
using WWModel.Result;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddSession();
//ģ����֤����
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = (context) =>
    {
        string error = string.Empty;
        foreach (var item in context.ModelState.Keys)
        {
            var state = context.ModelState[item];
            if (state!.Errors.Any())
            {
                error = state.Errors.First().ErrorMessage;
            }
        }
        return new JsonResult(new Result<string>()
        {
            code = 400,
            msg = error,
            data = "error"
        });
    };
});
//����Mysql����
string connStr = builder.Configuration.GetConnectionString("mysqlConn");
builder.Services.AddDbContext<WWDBContext>(i =>
{
    //�����ӳټ���
    i.UseLazyLoadingProxies();
    i.UseMySql(connStr, ServerVersion.AutoDetect(connStr), i =>
    {
        //ȫ�ֲ�ѯģʽ
         i.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
    });
});

//���ÿ���
builder.Services.AddCors(i =>
{
    i.AddDefaultPolicy(op =>
    {
        op.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin().Build();
    });
});

//����Token
var jwtConfig = builder.Configuration.GetSection("JwtConfig");
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtConfig["Issuer"],
        ValidAudience = jwtConfig["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["Key"]))
    };
});
var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
}
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();
