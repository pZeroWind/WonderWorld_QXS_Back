using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WWBLL;
using WWModel.Models;
using Tools;
using WWModel.input;
using Microsoft.AspNetCore.Authorization;
using WWModel.Result;

namespace WWUI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly UsersService _service;
        private readonly UserDataService _dataService;
        private readonly AuthService _authService;

        public AdminController(WWDBContext db, IConfiguration configuration)
        {
            _config = configuration;
            _service = new UsersService(db);
            _dataService = new UserDataService(db);
            _authService = new AuthService(db);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            return await Task.Run(async () => {
                var res = await _service.Login(_config, model.Account!, model.Password!);
                if (res.data != null &&(res.data!.GetRole()  == 3 || res.data!.GetRole()== 4))
                {
                    res.code = 401;
                    res.msg = "权限不足";
                }
                return Ok(res);
            });
        }

        [HttpGet("getGrant")]
        [Authorize]
        public async Task<IActionResult> GetGrant()
        {
            return Ok(new Result<List<string>>()
            {
                data = await _service.GetGrantAction(HttpContext.Request.Headers["Authorization"].ToString().GetRole())
            }) ;
        }

        //[HttpGet("D")]
        //public string D(string value)
        //{
        //    return value.Decrypt();
        //}

        //[HttpGet("E")]
        //public string E(string value)
        //{
        //    return value.Encrypt();
        //}

        [HttpGet("Role")]
        public async Task<IActionResult> GetAllRole()
        {
            return Ok(await _authService.GetAllRole());
        }

        [HttpGet("Auth")]
        public async Task<IActionResult> GetAllAuth()
        {
            return Ok(await _authService.GetAllAuth());
        }

        [HttpGet("Grant")]
        public async Task<IActionResult> GetAllGrant(int id)
        {
            return Ok(await _authService.GetAllGrant(id));
        }

        [HttpPost("Grant")]
        [Authorize]
        [WWUI.Filter.Auth("ModifyAuth")]
        public async Task<IActionResult> ChangeGrant(GrantModel model)
        {
            return Ok(await _authService.ChangeRole(model));
        }

        [HttpDelete("Role")]
        [Authorize]
        [WWUI.Filter.Auth("DeleteRole")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            return Ok(await _authService.DeleteRole(id));
        }

        [HttpPost("Role")]
        [Authorize]
        [WWUI.Filter.Auth("AddRole")]
        public async Task<IActionResult> AddRole(string name)
        {
            return Ok(await _authService.AddRole(name));
        }
    }
}
