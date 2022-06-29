using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tools;
using WWBLL;
using WWModel.Models;
using WWModel.Result;

namespace WWUI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AnalysisController : ControllerBase
    {
        private readonly BookService _bookService;
        private readonly UsersService _userService;

        public AnalysisController(WWDBContext db)
        {
            _bookService = new BookService(db);
            _userService = new UsersService(db);
        }

        /// <summary>
        /// 后台主页资料获取
        /// </summary>
        /// <returns></returns>
        [HttpGet("backstage")]
        public async Task<IActionResult> BackStage ()
        {
            return await Task.Run(async () =>
            {
                long date = new DateTimeOffset(DateTime.Today).ToUnixTimeMilliseconds();
                return Ok(new Result<BackStage>()
                    {
                            data = new BackStage(){
                            AllBook = await _bookService.GetBookConut(_ => true),
                            ShelfBook = await _bookService.GetBookConut(p => p.StateId == 3),
                            TodayShelf = await _bookService.GetBookConut(p => p.ShelfTime > date),
                            AllUser = await _userService.GetCount(_ => true)
                        }
                   }
                );
            });
        }

        /// <summary>
        /// 获取专区后台资料
        /// </summary>
        /// <returns></returns>
        [HttpGet("TypesData")]
        public async Task<IActionResult> GetTypesData()
        {
            return Ok(new Result<List<TypeSysData>>() 
            {
                data = await _bookService.GetTypesSys()
            });
        }

        /// <summary>
        /// 获取作家中心数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("Writer")]
        public async Task<IActionResult> GetWriterData()
        {
            var account = HttpContext.Request.Headers["Authorization"].ToString().GetAccount();
            return Ok(await _bookService.GetWriterData(account));
        }
    }
}
