using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;
using WWDAL;
using WWModel.input;
using WWModel.Models;
using WWModel.Result;

namespace WWBLL
{
    public class ChapterService : BaseService<TbChapter>
    {
        private readonly BaseManager<TbScroll> _scrollManager;
        private readonly BookManager _bookManager;
        private readonly BaseManager<TbConsume> _consume;
        private readonly BaseManager<TbWord> _word;

        public ChapterService(WWDBContext db) : base(db)
        {
            _scrollManager = new BaseManager<TbScroll>(db);
            _bookManager = new BookManager(db);
            _consume = new BaseManager<TbConsume>(db);
            _word = new BaseManager<TbWord>(db);
        }

        /// <summary>
        /// 查询书籍卷数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Result<List<Scroll>>> GetScroll(string id)
        {
            return Task.Run(() => new Result<List<Scroll>>()
            {
                data = _scrollManager.Select()
                .Where(x => x.BookId == id)
                .Select(p=>new Scroll(p))
                .ToList()
            });
        }

        /// <summary>
        /// 查询该卷可浏览章节内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ResultList<List<ChapterData>>> GetChapter(int id, int page, int size)
        {
            return Task.Run(() =>
            {
                var result = new ResultList<List<ChapterData>>()
                {
                    data = _manager.Select()
                    .Where(x => x.ScrollId == id && (bool)x.Pass!)
                    .OrderBy(x => x.Id)
                    .Skip((page - 1) * size)
                    .Take(size)
                    .Select(p => new ChapterData(p))
                    .ToList(),
                    page = page,
                    size = size,
                    total = _manager.Select().Where(x => x.ScrollId == id && (bool)x.Pass!).Count()
                };
                result.count = (int)Math.Ceiling(result.total * 1.0 / size);
                return result;
            });
        }

        /// <summary>
        /// 作者查看该卷内容
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public Task<ResultList<List<ChapterData>>> GetChapter(int id,string account, int page, int size)
        {
            return Task.Run(async () =>
            {
                if (!await _scrollManager.IsExist(x=>x.Book!.UserId == account))
                {
                    return new ResultList<List<ChapterData>>()
                    {
                        code = 401,
                        msg = "您不是该书的作者，无法查看内容"
                    };
                }
                var result = new ResultList<List<ChapterData>>()
                {
                    data = _manager.Select()
                    .Where(x => x.ScrollId == id)
                    .OrderBy(x => x.Id)
                    .Skip((page - 1) * size)
                    .Take(size)
                    .Select(p => new ChapterData(p))
                    .ToList(),
                    page = page,
                    size = size,
                    total = _manager.Select().Where(x => x.ScrollId == id).Count()
                };
                result.count = (int)Math.Ceiling(result.total * 1.0 / size);
                return result;
            });
        }

        /// <summary>
        /// 寻找对应章节详情内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Result<ChapterDetails>> GetChapter(int id,int scrollId,string bookId, string? account)
        {

            var data = await _manager.FindAsync(id);
            
            var dataList = _manager.Select()
                .Where(x => x.ScrollId == scrollId)
                .OrderBy(x=>x.Id)
                .Select(p => p.Id).ToList();

            string? next = null;
            string? forward = null;
            for (int i = 0; i < dataList.Count; i++)
            {
                if (dataList[i] == id)
                {
                    forward = i > 0 ? scrollId + "/" + dataList[i - 1].ToString() : null;
                    next = i < dataList.Count - 1 ? scrollId + "/" + dataList[i + 1].ToString() : null;
                    break;
                }
            }
            if (next == null)
            {
                int? scr = _scrollManager.Select()
                    .Where(x => x.BookId == bookId && x.Id > scrollId)
                    .OrderBy(p => p.Id)
                    .Select(p => p.Id)
                    .FirstOrDefault();
                if (scr != null)
                {
                    next = _manager.Select()
                        .Where(x => x.ScrollId == (int)scr!)
                        .OrderByDescending(p => p.UpdateTime)
                        .Select(p => scr + "/" + p.Id).FirstOrDefault();
                }
            }
            if (forward == null)
            {
                int? scr = _scrollManager.Select()
                    .Where(x => x.BookId == bookId && x.Id < scrollId)
                    .OrderBy(p=>p.Id)
                    .Select(p => p.Id)
                    .LastOrDefault();
                if (scr != null)
                {
                    forward = _manager.Select()
                        .Where(x => x.ScrollId == (int)scr!)
                        .OrderByDescending(p => p.UpdateTime)
                        .Select(p => scr + "/" + p.Id).LastOrDefault();
                }
            }
            if (data == null)
            {
                return new Result<ChapterDetails>()
                {
                    code = 404,
                    data = null,
                    msg = "未找到对应资源"
                };
            }
            if ((bool)data!.ChargeState!)
            {
                if (data.Scroll!.Book!.UserId != account&& _consume.Select().Where(p => p.UserId == account && p.ChapterId == id).Count() == 0)
                {
                    data.Content = "<p>章节未解锁</p>";
                }
                else
                {
                    data.ChargeState = false;
                }
            }
            return new Result<ChapterDetails>()
            {
                data = new ChapterDetails(data, forward, next)
            };
        }

        /// <summary>
        /// 上传章节
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Result<bool>> UploadChapter(ChapterModel model)
        {
            try
            {
                var book = _scrollManager.Find(model.ScrollId)!.Book;
                if (book!.StateId != 3 && (bool)model.ChargeState!)
                {
                    return new Result<bool>
                    {
                        code = 500,
                        msg = "该书藉未上架，无法上传收费章节",
                    };
                }
                _word.Select().ToList().ForEach(w =>
                {
                    model.Content!.Replace(w.Word!, "**");
                });
                TbChapter chapter = new TbChapter()
                {
                    ChargeState = model.ChargeState,
                    Content = model.Content,
                    Title = model.Title,
                    ScrollId = model.ScrollId,
                    Pass = model.Pass,
                    UpdateTime = model.UpdateTime,
                };

                book.UpdateTime = chapter.UpdateTime;
                await _bookManager.UpdateAsync(book);
                await _manager.AppendAsync(chapter);
                return new Result<bool>
                {
                    data = true
                };
            }
            catch (Exception ex)
            {
                return new Result<bool>
                {
                    code = 500,
                    msg = ex.Message,
                };
            }

        }

        /// <summary>
        /// 修改章节内容
        /// </summary>
        public async Task<Result<bool>> UpdateChapter(ChapterModel model)
        {
            try
            {
                var chapter = await _manager.FindAsync(model.Id);
                var book = chapter!.Scroll!.Book!;
                if (book!.StateId != 3 && (bool)model.ChargeState!)
                {
                    return new Result<bool>
                    {
                        code = 500,
                        msg = "该书藉未上架，无法上传收费章节",
                    };
                }
                if (chapter == null)
                {
                    return new Result<bool>
                    {
                        code = 404,
                        msg = "资源不存在或已被删除",
                    };
                }
                _word.Select().ToList().ForEach(w =>
                {
                    model.Content!.Replace(w.Word!, "**");
                });
                chapter.Title = model.Title;
                chapter.Content = model.Content;
                chapter.ChargeState = model.ChargeState;
                chapter.UpdateTime = model.UpdateTime;
                chapter.Pass = model.Pass;
                book.UpdateTime = model.UpdateTime;
                await _bookManager.UpdateAsync(book);
                await _manager.UpdateAsync(chapter);
                return new Result<bool>
                {
                    data = true
                };
            }
            catch (Exception ex)
            {
                return new Result<bool>
                {
                    code = 500,
                    msg = ex.Message,
                };
            }
        }

        /// <summary>
        /// 添加新卷
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Result<Scroll>> AddScroll(string bookId, string name)
        {
            try
            {
                var s = new TbScroll()
                {
                    BookId = bookId,
                    Name = name,
                };
                await _scrollManager.AppendAsync(s);
                return new Result<Scroll> { data = new Scroll(s) };
            }
            catch (Exception ex)
            {
                return new Result<Scroll> { code = 500, msg = ex.Message, };
            }
        }

        /// <summary>
        /// 添加新卷
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Result<bool>> ModifyScroll(int id, string name)
        {
            try
            {
                var s = await _scrollManager.FindAsync(id);
                s!.Name = name;
                await _scrollManager.UpdateAsync(s);
                return new Result<bool> { data = true };
            }
            catch (Exception ex)
            {
                return new Result<bool> { code = 500, msg = ex.Message, };
            }
        }

        public async Task<Result<string>> Ban(int id)
        {
            try
            {
                TbChapter tbChapter = _manager.Find(id)!;
                var user = tbChapter.Scroll!.Book!.User!.TbUserData.First();
                EmailSend send = new EmailSend(user.Email!);
                tbChapter.Pass = false;
                return await send.toEmil("您有文章被封禁", $"尊敬的用户您好，您名为\"{tbChapter.Title}\"的章节有违规内容，请整理后重新发布。");
            }
            catch (Exception ex)
            {
                return new Result<string> { code = 500, msg = ex.Message, };
            }
            

        }
    }
}
