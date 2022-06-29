using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tools;
using WWBLL;
using WWModel.Models;

namespace WWUI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ThumbController : ControllerBase
    {
        private readonly CommentService _commentService;

        public ThumbController(WWDBContext db)
        {
            _commentService = new CommentService(db);
        }

        /// <summary>
        /// 点赞
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        [HttpPost("up/{mode}/{id}")]
        [Authorize]
        public async Task<IActionResult> TumpUpAction(int id, int mode)
        {
            string account = HttpContext.Request.Headers["Authorization"].ToString().GetAccount();
            return Ok(await _commentService.TumbUp(id, account, (TumpUp)mode));
        }

        /// <summary>
        /// 点踩
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        [HttpPost("down/{mode}/{id}")]
        [Authorize]
        public async Task<IActionResult> TumpDownAction(int id, int mode)
        {
            string account = HttpContext.Request.Headers["Authorization"].ToString().GetAccount();
            return Ok(await _commentService.TumbDown(id, account, (TumpUp)mode));
        }
    }
}
