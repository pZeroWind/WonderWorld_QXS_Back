using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using WWBLL;
using WWModel.input;
using WWModel.Models;
using WWModel.Result;
using WWUI.Filter;
using Tools;
using System.Text.RegularExpressions;

namespace WWUI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly BookService _bookService;
        private readonly CommentService _commentService;
        private readonly ProfitService _profit;

        public BookController(WWDBContext db)
        {
            _bookService = new BookService(db);
            _commentService = new CommentService(db);
            _profit = new ProfitService(db);
        }

        /// <summary>
        /// 获取分类
        /// </summary>
        /// <returns></returns>
        [HttpGet("type")]
        public async Task<IActionResult> GetTypeData()
        {
            return Ok(new Result<List<TypeData>>()
            {
                data = await _bookService.GetTypes()
            }) ;
        }

        /// <summary>
        /// 获取标签
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet("tag")]
        public async Task<IActionResult> GetTag(int? page=1,int? size = 5)
        {
            return Ok(await _bookService.GetTags(page, size));
        }

        /// <summary>
        /// 获取状态
        /// </summary>
        /// <returns></returns>
        [HttpGet("state")]
        public async Task<IActionResult> GetState()
        {
            return Ok(new Result<List<States>>()
            {
                data = await _bookService.GetStates()
            }) ;
        }

        /// <summary>
        /// 获取书籍详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var account = HttpContext.Request.Headers["Authorization"].ToString().GetAccount();
            return Ok(await _bookService.GetBook(id, account));
        }

        /// <summary>
        /// 作者获取书籍详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("writer/{id}")]
        public async Task<IActionResult> Writer(string id)
        {
            var account = HttpContext.Request.Headers["Authorization"].ToString().GetAccount();
            return Ok(await _bookService.GetBook2(id, account));
        }

        /// <summary>
        /// 获取书籍列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("getList")]
        public async Task<IActionResult> GetBook(string? src = "",int? page = 1,int? size =5,int? mode = 0,int? type = 0,int? isPublish = 2,string? account = null,string? tags = null,int? kind = 0,int? maxWord = 0,int? minWord = 0)
        {
            if (src == null)
            {
                src = "";
            }
            if (Regex.IsMatch(src, "^[W]{2}"))
            {
                var result = await _bookService.GetBook(i => i.Id == src);
                if (result != null)
                {
                    return Ok(new ResultList<List<Book>>()
                    {
                        size = (int)size!,
                        count = 1,
                        total = 1,
                        page = (int)page!,
                        data =result
                    });
                }
            }
            List<Book> books;
            int c = 0;
            int t = 0;
            Expression<Func<TbBook, bool>> where = i=> i.Title!.Contains(src);
            //判断是否是按用户搜索
            if (account != null)
            {
                where = where.And(i => i.UserId == account);
            }
            //判断搜索的专区
            if(type != 0)
            {
                where = where.And(i => i.TypeId == type);
            }
            //按标签搜索
            if (tags!=null)
            {
                List<string> tagsArray = tags.Split(",").ToList();
                if (kind == 0)
                {
                    tagsArray.ForEach(i =>
                    {
                        where = where.And(p => p.TbTags.Select(p2 => p2.Name).Contains(i));
                    });
                }
                else
                {
                    Expression<Func<TbBook, bool>> ors = i => i.TbTags.Select(p => p.Name).Contains(tagsArray[0]);
                    foreach (var tag in tagsArray)
                    {
                        if (tag!=tagsArray[0])
                        {
                            ors = ors.Or(p => p.TbTags.Select(p2 => p2.Name).Contains(tag));
                        }
                    }
                }
            }
            //判断是否为搜索仅上架书籍
            if (mode == 3)
            {
                where = where.And(i => i.StateId == 3);
            }
            else
            {
                if (isPublish == 2)
                {
                    where = where.And(i => i.StateId != 1&& i.StateId != 0);
                }
                else if (isPublish == 5)
                {
                    where = where.And(i => i.StateId == 2);
                }
                else
                {
                    where = where.And(i => i.StateId == isPublish);
                }
            }
            if (minWord!=0&&maxWord!=0)
            {
                where = where.And(i => i.TbScrolls.Sum(p => p.TbChapters.Where(p3 => (bool)p3.Pass!).Sum(p2 => p2.Content!.Length)) <= maxWord && i.TbScrolls.Sum(p => p.TbChapters.Where(p3 => (bool)p3.Pass!).Sum(p2 => p2.Content!.Length)) >= minWord);
            }
            else if(maxWord == 0)
            {
                where = where.And(i => i.TbScrolls.Sum(p =>i.TbScrolls.Sum(p => p.TbChapters.Where(p3=>(bool)p3.Pass!).Sum(p2 => p2.Content!.Length))) >= minWord);
            }
            else
            {
                where = where.And(i => i.TbScrolls.Sum(p => i.TbScrolls.Sum(p => p.TbChapters.Where(p3 => (bool)p3.Pass!).Sum(p2 => p2.Content!.Length))) <= maxWord);
            }
            t = await _bookService.GetBookConut(where);
            switch (mode)
            {
                default://综合排序
                    books = await _bookService.GetHotBook(where, page, size);
                    break;
                case 2://最近更新
                    books = await _bookService.GetBook(where, page, size);
                    break;
                case 3://最新上架
                    books = await _bookService.GetBook(where, i => i.ShelfTime, page, size);
                    break;
                case 4://总点击
                    books = await _bookService.GetBook(where, i => i.ClickNum, page, size);
                    break;
                case 5://总月票
                    books = await _bookService.GetBook(where, i => i.TbProfits.Where(p=>p.ProfitModId == 1).Select(p=>p.Num).Sum(), page, size);
                    break;
                case 6://总刀片
                    books = await _bookService.GetBook(where, i => i.TbProfits.Where(p => p.ProfitModId == 2).Select(p => p.Num).Sum(), page, size);
                    break;
                case 7://总字数
                    books = await _bookService.GetBook(where, i => i.TbScrolls.Sum(p => p.TbChapters.Where(p3 => (bool)p3.Pass!).Sum(p2 => p2.Content!.Length)), page, size);
                    break;
            }
            c = (int)Math.Ceiling((double)(t * 1.0 / size)!);
            return Ok(new ResultList<List<Book>>()
            {
                size = (int)size!,
                count = c,
                total = t,
                page = (int)page!,
                data = books
            });
        }

        /// <summary>
        /// 获取轮播图
        /// </summary>
        /// <returns></returns>
        [HttpGet("banner")]
        public async Task<IActionResult> GetBanner()
        {
            return Ok(new Result<List<Banner>>()
            {
                data = await _bookService.GetBanners()
            });
        }

        /// <summary>
        /// 修改轮播图
        /// </summary>
        [HttpPut("banner")]
        [Authorize]
        [Auth("ModifyBanner")]
        public async Task<IActionResult> ChangeBanner(BannerListModel data)
        {
            return Ok(new Result<List<Banner>>()
            {
                data = await _bookService.ChangeBanners(data.data!)
            });
        }

        /// <summary>
        /// 封禁文章
        /// </summary>
        /// <returns></returns>
        [HttpGet("ban/{id}")]
        [Authorize]
        [Auth("ModifyBook")]
        public async Task<IActionResult> BanBook(string id)
        {
            //寻找对应id的书籍
            var book = await _bookService.Find(id);
            if (book == null)
            {
                return Ok(new Result<string>()
                {
                    code = 500,
                    data = "error",
                    msg = "未找到对应书籍"
                });
            }
            //修改状态
            if (book.data!.StateId != 0)
            {
                book.data!.StateId = 0;
            }
            else
            {
                book.data!.StateId = 1;
            }
            //是否成功
            if((await _bookService.Update(book.data)).data)
            {
                return Ok(new Result<string>()
                {
                    data = "success"
                });
            }
            return Ok(new Result<string>()
            {
                code = 500,
                data = "error",
                msg = "封禁失败"
            });
        }

        /// <summary>
        /// 上传书籍
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("upload")]
        [Authorize]
        public async Task<IActionResult> Upload(BookModel model)
        {
            return Ok(await _bookService.UploadBook(model, HttpContext.Request.Headers["Authorization"].ToString()));
        }

        /// <summary>
        /// 更新书籍
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> Update(BookModel model)
        {
            return Ok(await _bookService.UpdateBook(model, HttpContext.Request.Headers["Authorization"].ToString()));
        }

        /// <summary>
        /// 发布书籍
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("publish/{id}")]
        public async Task<IActionResult> Publish(string id)
        {
            return Ok(await _bookService.OnPublish(id));
        }

        /// <summary>
        /// 评论
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("comment")]
        [Authorize]
        public async Task<IActionResult> Comment(CommentModel model)
        {
            string account = HttpContext.Request.Headers["Authorization"].ToString().GetAccount();
            model.UserId = account;
            if (model.ParentId == null)
            {
                return Ok(await _commentService.SetBookComment(model));
            }
            else
            {
                return Ok(await _commentService.SetBookSubComment(model));
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
        public async Task<IActionResult> CommentGet(string id, int? p, int? page = 1, int? size = 5)
        {
            var userId = HttpContext.Request.Headers["Authorization"].ToString().GetAccount();
            if (p != null)
            {
                return Ok(await _commentService.GetBookSubComment((int)p, (int)page!, (int)size!,userId==""?null:userId));
            }
            else
            {
                return Ok(await _commentService.GetBookComment(id, (int)page!, (int)size!, userId == "" ? null : userId));
            }
        }

        /// <summary>
        /// 增加点击量
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("clickUp/{id}")]
        public async Task ClickUp(string id)
        {
            await _bookService.clickUp(id);
        }

        /// <summary>
        /// 查看书架
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet("shelf/{size?}/{page?}")]
        [Authorize]
        public async Task<IActionResult> GetShelf(int? page = 1,int? size = 5)
        {
            var account = HttpContext.Request.Headers["Authorization"].ToString().GetAccount();
            return Ok(await _bookService.GetMyLike(account, (int)page!, (int)size!));
        }

        /// <summary>
        /// 保存至书架
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="scrollId"></param>
        /// <param name="chapterId"></param>
        /// <returns></returns>
        [HttpPost("shelf")]
        [Authorize]
        public async Task<IActionResult> AddShelf(string bookId,int scrollId,int chapterId)
        {
            var account = HttpContext.Request.Headers["Authorization"].ToString().GetAccount();
            return Ok(await _bookService.AddShelf(account, bookId, scrollId, chapterId));
        }

        [HttpDelete("shelf/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteShelf(int id)
        {
            var account = HttpContext.Request.Headers["Authorization"].ToString().GetAccount();
            return Ok(await _bookService.DeleteShelf(account, id));
        }

        /// <summary>
        /// 投递月票
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        [HttpPost("tiket/{bookId}")]
        [Authorize]
        public async Task<IActionResult> GiveTiket(string bookId)
        {
            var account = HttpContext.Request.Headers["Authorization"].ToString().GetAccount();
            return Ok(await _profit.GiveTiket(bookId, account));
        }

        /// <summary>
        /// 投递刀片
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        [HttpPost("blade/{bookId}")]
        [Authorize]
        public async Task<IActionResult> GiveBlade(string bookId)
        {
            var account = HttpContext.Request.Headers["Authorization"].ToString().GetAccount();
            return Ok(await _profit.GiveBlade(bookId, account));
        }

        /// <summary>
        /// 购买章节
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        [HttpPost("buy/{bookId}/{chapterId}")]
        [Authorize]
        public async Task<IActionResult> BuyCapter(string bookId,int chapterId)
        {
            var account = HttpContext.Request.Headers["Authorization"].ToString().GetAccount();
            return Ok(await _profit.BuyChapter(bookId, account,chapterId));
        }

        /// <summary>
        /// 上架书籍
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        [HttpPost("onShelf/{bookId}")]
        [Authorize]
        public async Task<IActionResult> OnShelf(string bookId)
        {
            return Ok(await _bookService.OnShelf(bookId));
        }

        /// <summary>
        /// 完结书籍
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        [HttpPut("complete/{bookId}")]
        [Authorize]
        public async Task<IActionResult> OnComplete(string bookId)
        {
            return Ok(await _bookService.OnComplete(bookId));
        }
    }
}
