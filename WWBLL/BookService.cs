using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tools;
using WWDAL;
using WWModel.input;
using WWModel.Models;
using WWModel.Result;

namespace WWBLL
{
    public class BookService : BaseService<TbBook>
    {
        private readonly BookManager _bookManager;
        private readonly BaseManager<TbBanner> _bannerManager;
        private readonly BaseManager<TbType> _typeManager;
        private readonly BaseManager<TbState> _stateManager;
        private readonly BaseManager<TbTag> _tagManager;
        private readonly UserDataManager _userDataManager;
        private readonly BaseManager<TbBookshelf> _shelfManager;
        private readonly BaseManager<TbProfit> _profit;
        private readonly BaseManager<TbUser> _user;
        private readonly string[] bookForeign = new string[]{
                    "TbListDetails",
                    "TbScrolls",
                    "TbProfits",
                    "State",
                    "Type",
                    "TbTags"
                };

        public BookService(WWDBContext db) : base(db)
        {
            _bookManager = new BookManager(db);
            _bannerManager = new BaseManager<TbBanner>(db);
            _typeManager = new BaseManager<TbType>(db);
            _stateManager = new BaseManager<TbState>(db);
            _tagManager = new BaseManager<TbTag>(db);
            _userDataManager = new UserDataManager(db);
            _shelfManager = new BaseManager<TbBookshelf>(db);
            _profit = new BaseManager<TbProfit>(db);
            _user = new BaseManager<TbUser>(db);
        }

        public async Task<Result<WriterModel>> GetWriterData(string account)
        {
            try
            {
                WriterModel model = new WriterModel();
                WriterData data = new WriterData();
                data.money = (await _user.FindAsync(account))!.TbItems.FirstOrDefault()!.Money.ToString();
                data.TotalWord = _bookManager.Select()
                    .Where(p => p.UserId == account)
                    .Sum(p => p.TbScrolls
                    .Sum(p2 => p2.TbChapters
                    .Sum(p3 => p3.Content!.Length)));
                var nowtime = new DateTimeOffset(DateTime.Today).ToUnixTimeMilliseconds();
                data.TodayWord = _bookManager.Select()
                    .Where(p => p.UserId == account)
                    .Sum(p => p.TbScrolls
                    .Sum(p2 => p2.TbChapters.Where(p3=>p3.UpdateTime > nowtime)
                    .Sum(p3 => p3.Content!.Length)));
                data.BookNum = _bookManager.Select()
                    .Where(p => p.UserId == account).Count();
                model.WriterData = data;
                var week = new DateTimeOffset(DateTime.Today.AddDays(-7)).ToUnixTimeMilliseconds();
                var weekList = _profit.Select().Where(p => p.Time > week)
                    .OrderByDescending(p => p.Time).ToList()
                    .GroupBy(p => DateTimeOffset.FromUnixTimeMilliseconds((long)p.Time!).ToString("d"))
                    .Select(p => new
                    {
                        day = p.Key,
                        money = p.Where(p2=>p2.ProfitModId==4)
                        .Sum(p2 => p2.Num)
                    });
                foreach (var item in weekList)
                {
                    model.Days.Add(item.day);
                    model.Datas.Add((int)item.money!);
                }
                return new Result<WriterModel>()
                {
                    data = model
                };
            }
            catch(Exception ex)
            {
                return new Result<WriterModel>()
                {
                    code = 500,
                    msg = ex.Message
                };
            }
            
        }

        /// <summary>
        /// 修改轮播图
        /// </summary>
        /// <param name="banners"></param>
        /// <returns></returns>
        public Task<List<Banner>> ChangeBanners(List<Banner> banners)
        {
            return Task.Run(async () =>
            {
                await _bannerManager.DeleteAll();
                await _bannerManager.AppendRange(banners.Select(i => new TbBanner()
                {
                    ImgUrl = i.ImgUrl,
                    BookId = i.BookId
                }).ToList());
                return await GetBanners();
            });
        }

        /// <summary>
        /// 获取轮播图数据
        /// </summary>
        /// <returns></returns>
        public Task<List<Banner>> GetBanners()
        {
            return Task.Run(() =>
            {
                return _bannerManager.Select(new string[]
                {
                    "Book"
                }).Select(p => new Banner(p)).ToList();
            });
        }

        /// <summary>
        /// 获取热门帖
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public Task<List<Book>> GetHotBook(Expression<Func<TbBook, bool>> where, int? page = 1, int? size = 5)
        {
            return Task.Run(() =>
            {
                var res = _manager.Select(bookForeign)
                .Where(where)
                .OrderByDescending(p => p.UpdateTime)
                .OrderByDescending(p => p.TbListDetails.Count)
                .OrderByDescending(p => p.TbProfits.Where(p2 => p2.ProfitModId == 1).Sum(p2 => p2.Num))
                .OrderByDescending(p => p.TbProfits.Where(p2 => p2.ProfitModId == 2).Sum(p2 => p2.Num))
                .OrderByDescending(p => p.TbScrolls.Sum(p2=>p2.TbChapters!.Where(p4=>(bool)p4.Pass!).Sum(p3=>p3.Content!.Length)))
                .OrderByDescending(p => p.ClickNum)
                .Skip((int)((page - 1) * size)!).Take((int)size!)
                .Select(p => new Book(p)).ToList();
                res.ForEach(p =>
                {
                    p = SetBookData(p);
                });
                return res;
            });
        }

        /// <summary>
        /// 获取小说数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Result<Book>> GetBook(string id, string account)
        {
            Result<Book> result = new Result<Book>();
            try
            {
                var book = await _bookManager.FindAsync(id);
                if (book!=null&&book.StateId!=0)
                {
                    result.data = SetBookData(new Book(book), account);
                }
                else
                {
                    result.code = 404;
                    result.msg = "未找到相关资源，已被封禁或删除";
                }
            }
            catch(Exception e)
            {
                result.code = 500;
                result.msg = e.Message;
            }
            return result;
        }

        /// <summary>
        /// 获取小说数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Result<Book>> GetBook2(string id, string account)
        {
            Result<Book> result = new Result<Book>();
            try
            {
                var book = await _bookManager.FindAsync(id);
                if (book != null && book.UserId == account)
                {
                    result.data = SetBookData(new Book(book), account);
                }
                else
                {
                    result.code = 404;
                    result.msg = "未找到相关资源，已被封禁或删除";
                }
            }
            catch (Exception e)
            {
                result.code = 500;
                result.msg = e.Message;
            }
            return result;
        }

        /// <summary>
        /// 注入书本数据
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private Book SetBookData(Book p,string? account = null)
        {
            //计算小说总字数
            int? num = null;
            //计算卷数
            var scroll = _manager.Find(p.Id)!
            .TbScrolls.ToList();
            string? newChapter = null;
            num = scroll
                .Sum(i2 => i2.TbChapters
                .Where(p2 => (bool)p2.Pass!)
                .Sum(i3 => i3.Content!.Length));
            if (scroll.Any())
            {
                newChapter = scroll.Last()
                .TbChapters
                .Where(p2 => (bool)p2.Pass!)
                .OrderByDescending(i => i.UpdateTime)
                .Select(i => i.Title).FirstOrDefault();
            }
            if (account != "" && account != null)
            {
                var c = _shelfManager.Select(new string[] { "Chapter" }).Where(p2 => p2.UserId == account && p2.BookId == p.Id).Select(p2 => new { p2.ChapterId,p2.Chapter!.ScrollId }).FirstOrDefault();
                if (c != null)
                {
                    p.History = c.ScrollId + "/" + c.ChapterId;
                }
            }
            //若总值不为null则返回总字数
            p.TotalWord = num == null ? 0 : num;
            //若最新章节为空则返回暂无
            p.NewChapter = newChapter == null ? "暂无章节" : newChapter;
            //获取作者姓名
            p.NickName = _userDataManager.Select().Where(i => i.Account == p.Account).First().NickName;
            return p;
        }

        /// <summary>
        /// 获取书本总数--条件查询
        /// </summary>
        /// <param name="where"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public Task<int> GetBookConut(Expression<Func<TbBook, bool>> where)
        {
            return Task.Run(() =>
            {
                return _manager.Select()
                .Where(where)
                .Count();
            });
        }

        /// <summary>
        /// 获取分类
        /// </summary>
        /// <returns></returns>
        public Task<List<TypeData>> GetTypes()
        {
            return Task.Run(() =>
            {
                List<TypeData> list = _typeManager.Select().Select(p => new TypeData(p)).ToList();
                return list;
            });
        }

        /// <summary>
        /// 获取状态
        /// </summary>
        /// <returns></returns>
        public Task<List<States>> GetStates()
        {
            return Task.Run(() =>
            {
                return _stateManager.Select().Select(i => new States(i)).ToList();
            });
        }

        /// <summary>
        /// 获取专区后台资料
        /// </summary>
        /// <returns></returns>
        public Task<List<TypeSysData>> GetTypesSys()
        {
            return Task.Run(() =>
            {
                long today = new DateTimeOffset(DateTime.Today).ToUnixTimeMilliseconds();
                return _typeManager.Select().Select(p => new TypeSysData()
                {
                    Id = p.Id,
                    Name = p.Name,
                    AllBook = _manager.Select().Where(i => i.TypeId == p.Id).Count(),
                    PublishBook = _manager.Select().Where(i => i.TypeId == p.Id && i.StateId > 1).Count(),
                    BanBook = _manager.Select().Where(i => i.TypeId == p.Id && i.StateId == 0).Count(),
                    AllWriter = _manager.Select().Where(i => i.TypeId == p.Id).Select(i => i.User!.Id).Distinct().Count(),
                    ShelfBook = _manager.Select().Where(i => i.TypeId == p.Id && i.StateId == 3).Count(),
                    TodayShelf = _manager.Select().Where(i => i.TypeId == p.Id && i.StateId == 3 && i.ShelfTime > today).Count()
                }).ToList();
            });
        }

        /// <summary>
        /// 获取书藉列表-条件查询
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public Task<List<Book>> GetBook(Expression<Func<TbBook, bool>> where, int? page = 1, int? size = 5)
        {
            return Task.Run(() =>
            {
                var res = _manager.Select(bookForeign)
                .Where(where)
                .OrderByDescending((p) => p.UpdateTime)
                .Skip((int)((page - 1) * size)!).Take((int)size!)
                .Select(p => new Book(p)).ToList();
                res.ForEach(p =>
                {
                    p = SetBookData(p);
                });
                return res;
            });
        }

        /// <summary>
        /// 获取书藉列表-条件查询+排序
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="where">条件</param>
        /// <param name="order">排序</param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public Task<List<Book>> GetBook<S>(Expression<Func<TbBook, bool>> where, Expression<Func<TbBook, S>> order, int? page = 1, int? size = 5)
        {
            return Task.Run(() =>
            {
                var res = _manager.Select(bookForeign)
                .Where(where)
                .OrderByDescending((p) => p.UpdateTime)
                .OrderByDescending(order)
                .Skip((int)((page - 1) * size)!)
                .Take((int)size!)
                .Select(p => new Book(p))
                .ToList();
                res.ForEach(p =>
                {
                    p = SetBookData(p);
                });
                return res;

            });
        }

        /// <summary>
        /// 上传书籍
        /// </summary>
        /// <param name="model"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<Result<string>> UploadBook(BookModel model, string token)
        {
            int role = token.GetRole();
            if (role == 4)
            {
                return new Result<string>()
                {
                    code = 401,
                    msg = "你还不是作家，无法上传书籍",
                    data = "error"
                };
            }
            var data = new Result<string>()
            {
                code = await _bookManager.UploadBook(model, token),
                data = "success"
            };
            if (data.code == 500)
            {
                data.data = "error";
            }
            return data;
        }

        /// <summary>
        /// 修改书籍
        /// </summary>
        /// <param name="model"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<Result<string>> UpdateBook(BookModel model, string token)
        {
            int role = token.GetRole();
            if (role == 4)
            {
                return new Result<string>()
                {
                    code = 401,
                    msg = "你还不是作家，无法修改书籍",
                    data = "error"
                };
            }
            var data = new Result<string>()
            {
                code = await _bookManager.UpdateBook(model),
                data = "success"
            };
            if (data.code == 500)
            {
                data.data = "error";
            }
            return data;
        }

        /// <summary>
        /// 获取最热标签列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public Task<ResultList<List<string>>> GetTags(int? page = 1, int? size = 5)
        {
            return Task.Run(() =>
            {
                var res = new ResultList<List<string>>();
                res.data = _tagManager.Select()
                .GroupBy(p=>p.Name)
                .OrderByDescending(p=>p.Count())
                .Skip((int)((page - 1) * size)!)
                .Take((int)size!)
                .Select(p=>p.Key)
                .ToList()!;
                res.total = _tagManager.Select()
                .GroupBy(p => p.Name).Count();
                res.count = (int)Math.Ceiling((double)(res.total * 1.0 / size));
                res.page = (int)page!;
                res.size = (int)size!;
                return res;
            });
        }

        /// <summary>
        /// 获取个人收藏中的所有标签
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public Task<List<string>> GetMyLikeTags(string account)
        {
            return Task.Run(() =>
            {
                List<string> list = _shelfManager.Select()
                   .Where(p => p.UserId == account)
                   .Select(p => p.BookId!).ToList();
                List<string> result = new List<string>();
                list.ForEach(i =>
                {
                    result.AddRange(_bookManager.Find(i)!.TbTags.Select(p => p.Name!));
                });
                return result.GroupBy(p => p)
                .OrderByDescending(p => p.Count())
                .Select(p => p.Key).ToList();
            });
        }

        /// <summary>
        /// 获取我的书架
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task<ResultList<List<Book>>> GetMyLike(string account,int page,int size)
        {
            try
            {
                var linQ = _shelfManager.Select(new string[] { "Book" })
                    .Where(p => p.UserId == account);
                var bookIdList = linQ
                    .OrderByDescending(p => p.Id)
                    .Skip((page - 1) * size)
                    .Take(size)
                    .Select(p => p.BookId)
                    .ToList();
                var bookList = new List<Book>();
                foreach (var item in bookIdList)
                {
                    bookList.Add(new Book(await _bookManager.FindAsync(item)));
                }
                bookList.ForEach(p =>
                {
                    p = SetBookData(p, account);
                });
                int t = linQ.Count();
                return new ResultList<List<Book>>()
                {
                    data = bookList,
                    total = t,
                    page = page,
                    size = size,
                    count = (int)Math.Ceiling(t * 1.0 / size)
                };
            }
            catch (Exception ex)
            {
                return new ResultList<List<Book>>()
                {
                    code = 500,
                    msg = ex.Message
                };
            }
        }

        /// <summary>
        /// 保存书至书架
        /// </summary>
        /// <param name="account"></param>
        /// <param name="bookId"></param>
        /// <param name="scrollId"></param>
        /// <param name="chapterId"></param>
        /// <returns></returns>
        public async Task<Result<bool>> AddShelf(string account, string bookId, int scrollId, int chapterId)
        {
            try
            {
                var data = await _shelfManager.FirstAsync(p=>p.UserId == account && p.BookId == bookId);
                if (data != null)
                {
                    await _shelfManager.DeleteAsync(data.Id);
                }
                await _shelfManager.AppendAsync(new TbBookshelf()
                {
                    BookId = bookId,
                    UserId = account,
                    ChapterId = chapterId,
                });
                return new Result<bool>()
                {
                    data = true
                };
            }
            catch(Exception ex)
            {
                return new Result<bool>()
                {
                    code = 500,
                    msg = ex.Message
                };
            }
        }

        /// <summary>
        /// 删除书架记录
        /// </summary>
        /// <param name="account"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Result<bool>> DeleteShelf(string account,int id)
        {
            try
            {
                var data = await _shelfManager.FirstAsync(p => p.UserId == account && p.Id == id);
                if (data != null)
                {
                    await _shelfManager.DeleteAsync(data.Id);
                }
                return new Result<bool>()
                {
                    data = true
                };
            }
            catch(Exception ex)
            {
                return new Result<bool>()
                {
                    code = 500,
                    msg = ex.Message
                };
            }
        }
        
        /// <summary>
        /// 点击量UP
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task clickUp(string id)
        {
            var book = await _bookManager.FindAsync(id);
            if (book != null)
            {
                book.ClickNum++;
                await _bookManager.UpdateAsync(book);
            }
        }

        /// <summary>
        /// 书籍上架
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Result<bool>> OnShelf(string id)
        {
            try
            {
                var data = await _bookManager.FindAsync(id);
                if (data != null)
                {
                    int allContent = data.TbScrolls.Sum(p => p.TbChapters.Sum(p2 => p2.Content!.Length));
                    if (allContent > 15 * 10000)
                    {
                        data.StateId = 3;
                        await _bookManager.UpdateAsync(data);
                        return new Result<bool> { data = true };
                    }
                    else
                    {
                        return new Result<bool>()
                        {
                            code = 400,
                            msg = "当前书籍尚不满足上架条件"
                        };
                    }
                }
                else
                {
                    return new Result<bool>()
                    {
                        code = 404,
                        msg = "指定资源不存在"
                    };
                }
            }
            catch (Exception ex)
            {
                return new Result<bool>()
                {
                    code = 500,
                    msg = ex.Message
                };
            }
        }

        /// <summary>
        /// 书籍发布
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Result<bool>> OnPublish(string id)
        {
            try
            {
                var data = await _bookManager.FindAsync(id);
                if (data != null)
                {
                    int allContent = data.TbScrolls.Sum(p => p.TbChapters.Sum(p2 => p2.Content!.Length));
                    data.StateId = 2;
                    await _bookManager.UpdateAsync(data);
                    return new Result<bool> { data = true };
                }
                else
                {
                    return new Result<bool>()
                    {
                        code = 404,
                        msg = "指定资源不存在"
                    };
                }
            }
            catch (Exception ex)
            {
                return new Result<bool>()
                {
                    code = 500,
                    msg = ex.Message
                };
            }
        }

        /// <summary>
        /// 书籍完结
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Result<bool>> OnComplete(string id)
        {
            try
            {
                var data = await _bookManager.FindAsync(id);
                if (data != null)
                {
                    int allContent = data.TbScrolls.Sum(p => p.TbChapters.Sum(p2 => p2.Content!.Length));
                    data.StateId = 4;
                    await _bookManager.UpdateAsync(data);
                    return new Result<bool> { data = true };
                }
                else
                {
                    return new Result<bool>()
                    {
                        code = 404,
                        msg = "指定资源不存在"
                    };
                }
            }
            catch (Exception ex)
            {
                return new Result<bool>()
                {
                    code = 500,
                    msg = ex.Message
                };
            }
        }


    }
}
