using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Tools;
using WWModel.Models;
using WWModel.Result;

namespace WWUI.Filter
{
    public class AuthAttribute : Attribute, IAsyncActionFilter
    {
        private readonly string? _auth;
        private readonly IConfigurationRoot _Configuration;

        public AuthAttribute(string? auth)
        {
            _auth = auth;
            _Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .Build();
        }

        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            return Task.Run(async () =>
            {
                //获取token
                string? token = context.HttpContext.Request.Headers["Authorization"].ToString();
                //判断token是否为空
                if (token != null && token != "")
                {
                    //不为空则查询角色权限
                    DbLink db = new DbLink(_Configuration.GetConnectionString("mysqlConn"));
                    int roleId = token.GetRole();
                    //为普通用户时权限不足，否则查询角色权限
                    if (roleId == 3 || roleId == 4)
                    {
                        context.Result = new JsonResult(new Result<string>()
                        {
                            code = 401,
                            data = "error",
                            msg = "权限不足"
                        });
                    }
                    else
                    {
                        //查询获取角色所有权限
                        //var pers = db.TbRoles.Find(roleId)!.TbGrants.Select(i => i.Auth.Name);
                        var pers = await db.Select<string>("SELECT ta.name FROM tb_auth ta ,tb_grant tg WHERE ta.id = tg.authId and tg.roleid = @RoleID", new { RoleID = roleId });
                        //不包含该权限时权限不足
                        if (!pers.Contains(_auth!))
                        {
                            context.Result = new JsonResult(new Result<string>()
                            {
                                code = 401,
                                data = "error",
                                msg = "权限不足"
                            });
                        }
                        else
                        {
                            await next();
                        }
                    }
                }
                else
                {
                    context.Result = new JsonResult(new Result<string>()
                    {
                        code = 401,
                        data = "error",
                        msg = "权限不足"
                    });
                }
            });
         }
    }
}
