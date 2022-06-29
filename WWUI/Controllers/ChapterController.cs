using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tools;
using WWBLL;
using WWModel.input;
using WWModel.Models;
using WWModel.Result;
using WWUI.Filter;

namespace WWUI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ChapterController : ControllerBase
    {
        private readonly ChapterService _service;
        private readonly CommentService _commentService;

        public ChapterController(WWDBContext db)
        {
            _service = new ChapterService(db);
            _commentService = new CommentService(db);

        }

        /// <summary>
        /// 获取书籍全部卷数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("scroll/{id}")]
        public async Task<IActionResult> GetScrolls(string id)
        {
            return Ok(await _service.GetScroll(id));
        }

        /// <summary>
        /// 获取书籍全部章节数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetList(int id, int? page = 1, int? size = 10)
        {
            return Ok(await _service.GetChapter(id, (int)page!, (int)size!));
        }

        /// <summary>
        /// 作者查看书籍内容
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet("writer/{id}")]
        public async Task<IActionResult> GetListWirter(int id, int? page = 1, int? size = 10)
        {
            var userId = HttpContext.Request.Headers["Authorization"].ToString().GetAccount();
            return Ok(await _service.GetChapter(id,userId,(int)page!, (int)size!));
        }

        /// <summary>
        /// 获取指定章节内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("get/{bookId}/{scrollId}/{id}")]
        public async Task<IActionResult> Get(int id,int scrollId,string bookId)
        {
            var userId = HttpContext.Request.Headers["Authorization"].ToString().GetAccount();
            return Ok(await _service.GetChapter(id, scrollId, bookId,userId));
        }

        /// <summary>
        /// 添加卷
        /// </summary>
        /// <param name="chapter"></param>
        /// <returns></returns>
        [HttpPost("scroll")]
        public async Task<IActionResult> addScroll(string bookId,string name)
        {
            return Ok(await _service.AddScroll(bookId,name));
        }

        /// <summary>
        /// 修改卷
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPut("scroll")]
        public async Task<IActionResult> UpdateScroll(int id, string name)
        {
            return Ok(await _service.ModifyScroll(id, name));
        }

        /// <summary>
        /// 新增章节
        /// </summary>
        /// <param name="chapter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Upload(ChapterModel chapter)
        {
            return Ok(await _service.UploadChapter(chapter));
        }

        /// <summary>
        /// 修改章节
        /// </summary>
        /// <param name="chapter"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateChapter(ChapterModel chapter)
        {
            return Ok(await _service.UpdateChapter(chapter));
        }

        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("comment")]
        [Authorize]
        public async Task<IActionResult> Comment(CommentModel model)
        {
            //判断评论长度
            if (model.Content!.Length > 150)
            {
                return Ok(new Result<string>()
                {
                    code = 400,
                    msg = "评论长度过长，应在150个字符以内",
                    data = "error"
                });
            }
            //获取账号
            string account = HttpContext.Request.Headers["Authorization"].ToString().GetAccount();
            model.UserId = account;
            if (model.ParentId == null)
            {
                return Ok(await _commentService.SetChapterComment(model));
            }
            else
            {
                return Ok(await _commentService.SetChapterSubComment(model));
            }

        }

        /// <summary>
        /// 获取评论
        /// </summary>
        /// <param name="id"></param>
        /// <param name="p"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet("comment/{id}/{p?}")]
        public async Task<IActionResult> CommentGet(int id,int? p,int? page = 1,int? size = 5)
        {
            var userId = HttpContext.Request.Headers["Authorization"].ToString().GetAccount();
            if (p == null)
            {
                return Ok(await _commentService.GetChapterSubComment(id, (int)page!, (int)size!, userId == "" ? null : userId));
            }
            else
            {
                return Ok(await _commentService.GetChapterComment(id, (int)p!, (int)page!, (int)size!, userId == "" ? null : userId));
            }
        }

        [HttpPut("Ban/{id}")]
        [Authorize]
        [Auth("BanChapter")]
        public async Task<IActionResult> Ban(int id)
        {
            return Ok(await _service.Ban(id));
        }
    }
}
