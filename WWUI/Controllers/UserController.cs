using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WWModel.Models;
using WWBLL;
using WWModel.input;
using WWModel.Result;
using Microsoft.AspNetCore.Authorization;
using Tools;
using System.Text.RegularExpressions;
using WWUI.Filter;

namespace WWUI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly UsersService _service;
        private readonly UserDataService _dataService;
        //注册验证码储存字典
        private static readonly Dictionary<string, string> EmailCode = new Dictionary<string, string>();
        //忘记密码验证储存字典
        private static readonly Dictionary<string, string> ForgetCode = new Dictionary<string, string>();

        public UserController(WWDBContext db,IConfiguration configuration)
        {
            _config = configuration;
            _service = new UsersService(db);
            _dataService = new UserDataService(db);
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            return Ok(await _service.Login(_config, model.Account!, model.Password!));
        }

        /// <summary>
        /// 获取用户资料
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpGet("{account?}")]
        public async Task<IActionResult> GetUserData(string? account)
        {
            if (account==""||account==null)
            {
                account = HttpContext.Request.Headers["Authorization"].ToString().GetAccount();
            }
            return Ok(await _dataService.GetData(i => i.Account == account));
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            //尝试获取验证码
            if(EmailCode.TryGetValue(model.Email!,out string? code))
            {
                Console.WriteLine(code);
                //判断验证码是否正确
                if (model.Code != code)
                {
                    return Ok(new Result<string>()
                    {
                        code = 500,
                        msg = "验证码错误，请重新输入",
                        data = "error"
                    });
                }
            }
            else
            {
                return Ok(new Result<string>()
                {
                    code = 500,
                    msg = "验证码无效，请重新尝试获取",
                    data = "error"
                });
            }
            return Ok(await _service.Register(model, 4));
        }

        [HttpGet("account/{account}")]
        public async Task<IActionResult> AccountIsExist(string account)
        {
            return Ok(await _service.IsAccountExist(account));
        }

        /// <summary>
        /// 获取邮箱验证码
        /// </summary>
        [HttpGet("email/get/{email?}")]
        public async Task<IActionResult> RegisterEmail(string? email)
        {
            if (email!=null&&email.IndexOf("@") != -1)
            {
                //构建邮箱发送对象
                EmailSend send = new EmailSend(email);
                //获取验证码
                string code = send.GetPass();
                if(EmailCode.TryGetValue(email,out string? code2))
                {
                    EmailCode.Remove(email);
                }
                EmailCode.Add(email, code);
                //发送邮件
                _ = Task.Run(() =>
                {
                    //等待5分钟后执行
                    Thread.Sleep(1000 * 60 * 5);
                    if (EmailCode.TryGetValue(email, out string? code))
                    {
                        //删除验证码记录
                        EmailCode.Remove(email);
                    }
                });
                return Ok(await send.toEmil("WonderWorld邮箱验证", $"你的验证码为{code}，请及时使用，5分钟后将失效"));
            }
            else
            {
                return Ok(new Result<string>()
                {
                    code = 500,
                    msg = "邮箱格式错误"
                });
            }
        }

        /// <summary>
        /// 忘记密码邮箱验证码获取
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpGet("forget/get/{account?}")]
        public async Task<IActionResult> ForgetEmail(string? account)
        {
            if (account==null)
            {
                return Ok(new Result<string>()
                {
                    code = 400,
                    msg = "账号不得为空"
                });
            }
            var user = (await _dataService.First(p=>p.Account == account)).data;
            if (user != null)
            {
                //构建邮箱发送对象
                EmailSend send = new EmailSend(user.Email!);
                //获取验证码
                string code = send.GetPass();
                if (ForgetCode.TryGetValue(account, out string? code2))
                {
                    ForgetCode.Remove(account);
                }
                ForgetCode.Add(account, code);
                //发送邮件
                _ = Task.Run(() =>
                {
                    //等待5分钟后执行
                    Thread.Sleep(1000 * 60 * 5);
                    if (ForgetCode.TryGetValue(account, out string? code))
                    {
                        //删除验证码记录
                        ForgetCode.Remove(account);
                    }
                });
                return Ok(await send.toEmil("WonderWorld邮箱验证", $"你的验证码为{code}，请及时使用，5分钟后将失效"));
            }
            else
            {
                return Ok(new Result<string>()
                {
                    code = 404,
                    msg = "用户不存在"
                });
            }
        }

        /// <summary>
        /// 忘记密码
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPost("forget")]
        public async Task<IActionResult> ForgetPassword(PwdChangeModel model)
        {
            return Ok(await _service.ForgetPassword(model));
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPost("pwdModify")]
        [Authorize]
        public async Task<IActionResult> ModifyPassword(PwdChangeModel model)
        {
            if (model.Account == HttpContext.Request.Headers["Authorization"].ToString().GetAccount())
            {
                return Ok(new Result<bool>()
                {
                    code = 500,
                    data = false,
                    msg = "您无法修改出自己外的用户密码"
                });
            }
            return Ok(await _service.ModifyPassword(model));
        }

        /// <summary>
        /// 修改资料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("modify")]
        [Authorize]
        public async Task<IActionResult> ModifyData(ModifyModel model)
        {

            if (model.Account != HttpContext.Request.Headers["Authorization"].ToString().GetAccount())
            {
                return Ok(new Result<bool>()
                {
                    code = 500,
                    data = false,
                    msg = "您无法修改除自己外的用户资料"
                });
            }
            return Ok(await _dataService.Update(model));
        }

        /// <summary>
        /// 绑定身份证
        /// </summary>
        /// <param name="idCard"></param>
        /// <returns></returns>
        [HttpPost("bind/idCard")]
        [Authorize]
        public async Task<IActionResult> idCard(string idCard)
        {
            if (!Regex.IsMatch(idCard, @"^(^\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$"))
            {
                return Ok(new Result<string>
                {
                    code = 500,
                    msg = "身份证格式不正确",
                    data = "未绑定"
                });
            }
            return Ok(await _service.BindCard(HttpContext.Request.Headers["Authorization"].ToString().GetAccount(), idCard));
        }

        /// <summary>
        /// 绑定银行卡
        /// </summary>
        /// <param name="bankCard"></param>
        /// <returns></returns>
        [HttpPost("bind/bankCard")]
        [Authorize]
        public async Task<IActionResult> bankCard(string bankCard)
        {
            if (bankCard.Length == 19)
            {
                return Ok(new Result<string>
                {
                    code = 500,
                    msg = "银行卡格式不正确",
                    data = "未绑定"
                });
            }
            return Ok(await _service.BindBank(HttpContext.Request.Headers["Authorization"].ToString().GetAccount(), bankCard));
        }

        /// <summary>
        /// 成为作家
        /// </summary>
        /// <returns></returns>
        [HttpPost("applyWriter")]
        [Authorize]
        public async Task<IActionResult> ApplyWriter()
        {
            return Ok(await _service.ApplyWirter(_config, HttpContext.Request.Headers["Authorization"].ToString().GetAccount()));
        }

        [HttpGet("hot")]
        public async Task<IActionResult> Hot()
        {
            return Ok(await _dataService.GetHot());
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetUsers(int type, int? page = 1, int? size = 5, string? src = null)
        {
            return Ok(await _dataService.GetHot(type,(int)page!,(int)size!,src));
        }


        [HttpGet("getSubCommentsByMe")]
        [Authorize]
        public async Task<IActionResult> GetSubCommentsByMe(int? page = 1,int? size = 5)
        {
            return Ok(await _service.GetSubCommentsByMe(HttpContext.Request.Headers["Authorization"].ToString().GetAccount(),(int)page!,(int)size!));
        }

        [HttpGet("getCommentsByMe")]
        [Authorize]
        public async Task<IActionResult> GetCommentsByMe(int? page = 1, int? size = 5)
        {
            return Ok(await _service.GetCommentsByMe(HttpContext.Request.Headers["Authorization"].ToString().GetAccount(), (int)page!, (int)size!));
        }

        [HttpPost("ChangeRole")]
        [Authorize]
        [Auth("ModifyRole")]
        public async Task<IActionResult> ChangeRole(int role,string account)
        {
            return Ok(await _dataService.RoleChange(account, role));
        }
    }
}
