using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WWDAL;
using WWModel.input;
using WWModel.Models;
using WWModel.Result;

namespace WWBLL
{
    public enum TumpUp
    {
        Book,SubBook,Chapter,SubChapter
    }

    public class CommentService : BaseService<TbCommentBook>
    {
        private readonly BaseManager<TbCommentChapter> _commentChapterManager;
        private readonly BaseManager<TbSubCommentBook> _subCommentBookManager;
        private readonly BaseManager<TbSubCommentChapter> _subCommentChapterManager;
        private readonly BaseManager<TbThumbsUpBook> _tumpUpBook;
        private readonly BaseManager<TbThumbsUpSubBook> _tumpUpSubBook;
        private readonly BaseManager<TbThumbsUpChapter> _tumpUpChapter;
        private readonly BaseManager<TbThumbsUpSubChapter> _tumpUpSubChapter;
        private readonly UserDataManager _userDataManager;

        public CommentService(WWDBContext db) : base(db)
        {
            _commentChapterManager = new BaseManager<TbCommentChapter>(db);
            _subCommentBookManager = new BaseManager<TbSubCommentBook>(db);
            _subCommentChapterManager = new BaseManager<TbSubCommentChapter>(db);
            _tumpUpBook = new BaseManager<TbThumbsUpBook>(db);
            _tumpUpSubBook = new BaseManager<TbThumbsUpSubBook>(db);
            _tumpUpChapter = new BaseManager<TbThumbsUpChapter>(db);
            _tumpUpSubChapter = new BaseManager<TbThumbsUpSubChapter>(db);
            _userDataManager = new UserDataManager(db);
        }

        /// <summary>
        /// 添加小说评论
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Result<Comments>> SetBookComment(CommentModel model)
        {
            Result<Comments> result = new Result<Comments>();
            try
            {
                TbCommentBook book = new TbCommentBook()
                {
                    BookId = model.BookId,
                    Content = model.Content,
                    Time = model.Time,
                    UserId = model.UserId
                };
                await _manager.AppendAsync(book);
                result.data = new Comments(book);
                var user = await _userDataManager.FirstAsync(p=>p.Account == book.UserId);
                result.data.UserName = user!.NickName;
                result.data.ImgUrl = user!.ImgUrl;
            }
            catch (Exception ex)
            {
                result.code = 500;
                result.msg = ex.Message;
            }
            return result; 
        }

        /// <summary>
        /// 添加小说子评论
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Result<Comments>> SetBookSubComment(CommentModel model)
        {
            Result<Comments> result = new Result<Comments>();
            try
            {
                TbSubCommentBook book = new TbSubCommentBook()
                {
                    ParentId = model.ParentId,
                    Content = model.Content,
                    Time = model.Time,
                    UserId = model.UserId,
                    OtherId = model.OtherId,
                };
                await _subCommentBookManager.AppendAsync(book);
                result.data = new Comments(book);
                var user = await _userDataManager.FirstAsync(p => p.Account == book.UserId);
                var other = await _userDataManager.FirstAsync(p => p.Account == book.OtherId);
                result.data.UserName = user!.NickName;
                result.data.ImgUrl = user!.ImgUrl;
                result.data.OtherName = other!.NickName;
            }
            catch (Exception ex)
            {
                result.code = 500;
                result.msg = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 获取小说评论列表
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public Task<ResultList<List<Comments>>> GetBookComment(string bookId ,int page, int size,string? account = null)
        {
            return Task.Run(() =>
            {
                var result = new ResultList<List<Comments>>();
                result.page = page;
                result.size = size;
                try
                {
                    List<Comments> data = _manager.Select(new string[]
                    {
                        "TbThumbsUpBooks"
                    }).Where(p => p.BookId == bookId)
                    .OrderByDescending(p => p.Time)
                    .Skip((page - 1) * size)
                    .Take(size)
                    .Select(p => new Comments(p))
                    .ToList();
                    SetUserData(ref data);
                    if (account!=null)
                    {
                        data.ForEach(p =>
                        {
                            var thumb = _tumpUpBook.First(p2 => p2.CommentId == p.Id && p2.UserId == account);
                            p.thumb = thumb==null?null:thumb.Up;
                        });
                    }
                    result.total = _manager.Select().Where(p => p.BookId == bookId).Count();
                    result.data = data;
                    result.count = (int)Math.Ceiling(result.total * 1.0 / result.size);
                    
                }
                catch (Exception ex)
                {
                    result.code = 500;
                    result.msg = ex.Message;
                }
                return result;
            });
        }

        /// <summary>
        /// 获取小说子评论列表
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public Task<ResultList<List<Comments>>> GetBookSubComment(int parentId, int page, int size, string? account = null)
        {
            return Task.Run(() =>
            {
                var result = new ResultList<List<Comments>>();
                result.page = page;
                result.size = size;
                try
                {
                    List<Comments> data = _subCommentBookManager.Select(new string[]
                    {
                        "TbThumbsUpSubBooks"
                    }).Where(p => p.ParentId == parentId)
                    .OrderByDescending(p => p.Time)
                    .Skip((page - 1) * size)
                    .Take(size)
                    .Select(p => new Comments(p))
                    .ToList();
                    SetUserData(ref data);
                    SetOtherData(ref data);
                    if (account != null)
                    {
                        data.ForEach(p =>
                        {
                            var thumb = _tumpUpSubBook.First(p2 => p2.CommentId == p.Id && p2.UserId == account);
                            p.thumb = thumb == null ? null : thumb.Up;
                        });
                    }
                    result.total = _subCommentBookManager.Select().Where(p => p.ParentId == parentId).Count();
                    result.data = data;
                    result.count = (int)Math.Ceiling(result.total * 1.0 / result.size);
                }
                catch (Exception ex)
                {
                    result.code = 500;
                    result.msg = ex.Message;
                }
                return result;
            });
        }

        /// <summary>
        /// 添加章节段评
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Result<Comments>> SetChapterComment(CommentModel model)
        {
            Result<Comments> result = new Result<Comments>();
            try
            {
                TbCommentChapter book = new TbCommentChapter()
                {
                    ChapterId = model.ChapterId,
                    Content = model.Content,
                    Time = model.Time,
                    UserId = model.UserId,
                    Paragraph = model.p
                };
                await _commentChapterManager.AppendAsync(book);
                result.data = new Comments(book);
                var user = await _userDataManager.FirstAsync(p => p.Account == book.UserId);
                result.data.UserName = user!.NickName;
                result.data.ImgUrl = user!.ImgUrl;
            }
            catch (Exception ex)
            {
                result.code = 500;
                result.msg = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 添加章节段评回复
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Result<Comments>> SetChapterSubComment(CommentModel model)
        {
            Result<Comments> result = new Result<Comments>();
            try
            {
                TbSubCommentChapter book = new TbSubCommentChapter()
                {
                    ParentId = model.ParentId,
                    Content = model.Content,
                    Time = model.Time,
                    UserId = model.UserId,
                    OtherId = model.OtherId
                };
                await _subCommentChapterManager.AppendAsync(book);
                result.data = new Comments(book);
                var user = await _userDataManager.FirstAsync(p => p.Account == book.UserId);
                var other = await _userDataManager.FirstAsync(p => p.Account == book.OtherId);
                result.data.UserName = user!.NickName;
                result.data.ImgUrl = user!.ImgUrl;
                result.data.OtherName = other!.NickName;
            }
            catch (Exception ex)
            {
                result.code = 500;
                result.msg = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 获取章节段评列表
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public Task<ResultList<List<Comments>>> GetChapterComment(int chapterId,int paragraph, int page, int size, string? account = null)
        {
            return Task.Run(() =>
            {
                var result = new ResultList<List<Comments>>();
                result.page = page;
                result.size = size;
                try
                {
                    List<Comments> data = _commentChapterManager.Select(new string[]
                    {
                        "TbThumbsUpChapters"
                    }).Where(p => p.ChapterId == chapterId && p.Paragraph == paragraph)
                    .OrderByDescending(p => p.Time)
                    .Skip((page - 1) * size)
                    .Take(size)
                    .Select(p => new Comments(p))
                    .ToList();
                    SetUserData(ref data);
                    if (account != null)
                    {
                        data.ForEach(p =>
                        {
                            var thumb = _tumpUpChapter.First(p2 => p2.CommentId == p.Id && p2.UserId == account);
                            p.thumb = thumb == null ? null : thumb.Up;
                        });
                    }
                    result.total = _commentChapterManager.Select().Where(p => p.ChapterId == chapterId && p.Paragraph == paragraph).Count();
                    result.data = data;
                    result.count = (int)Math.Ceiling(result.total * 1.0 / result.size);
                }
                catch (Exception ex)
                {
                    result.code = 500;
                    result.msg = ex.Message;
                }
                return result;
            });
        }

        /// <summary>
        /// 获取章节段评子评论列表
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public Task<ResultList<List<Comments>>> GetChapterSubComment(int parentId, int page, int size, string? account = null)
        {
            return Task.Run(() =>
            {
                var result = new ResultList<List<Comments>>();
                result.page = page;
                result.size = size;
                try
                {
                    List<Comments> data = _subCommentChapterManager.Select(new string[]
                    {
                        "TbThumbsUpSubChapters"
                    }).Where(p => p.ParentId == parentId)
                    .OrderByDescending(p => p.Time)
                    .Skip((page - 1) * size)
                    .Take(size)
                    .Select(p => new Comments(p))
                    .ToList();
                    SetUserData(ref data);
                    SetOtherData(ref data);
                    if (account != null)
                    {
                        data.ForEach(p =>
                        {
                            var thumb = _tumpUpSubChapter.First(p2 => p2.CommentId == p.Id && p2.UserId == account);
                            p.thumb = thumb == null ? null : thumb.Up;
                        });
                    }
                    result.total = _subCommentChapterManager.Select().Where(p => p.ParentId == parentId).Count();
                    result.data = data;
                    result.count = (int)Math.Ceiling(result.total * 1.0 / result.size);
                }
                catch (Exception ex)
                {
                    result.code = 500;
                    result.msg = ex.Message;
                }
                return result;
            });
        }

        /// <summary>
        /// 获取评论
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public Task<Result<Comments>> GetComment(string accuont, int page, int size)
        {
            return Task.Run(() =>
            {
                var result = new Result<Comments>();
                try
                {
                    List<Comments> data = _manager.Select(new string[]
                    {
                        "TbThumbsUpBooks"
                    }).Where(p => p.UserId == accuont)
                    .OrderByDescending(p => p.Time)
                    .Skip((page - 1) * size)
                    .Take(size)
                    .Select(p => new Comments(p))
                    .ToList();
                    SetUserData(ref data);
                    result.data = data.FirstOrDefault();
                }
                catch (Exception ex)
                {
                    result.code = 500;
                    result.msg = ex.Message;
                }
                return result;
            });
        }

        /// <summary>
        /// 获取回复我的
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public Task<ResultList<List<Comments>>> GetMySubComment(string accuont, int page, int size)
        {
            return Task.Run(() =>
            {
                var result = new ResultList<List<Comments>>();
                result.page = page;
                result.size = size;
                try
                {
                    List<Comments> data = _subCommentBookManager.Select(new string[]
                    {
                        "TbThumbsUpSubBooks"
                    }).Where(p => p.OtherId == accuont)
                    .OrderByDescending(p => p.Time)
                    .Skip((page - 1) * size)
                    .Take(size)
                    .Select(p => new Comments(p))
                    .ToList();
                    SetUserData(ref data);
                    SetOtherData(ref data);
                    result.total = _subCommentBookManager.Select().Where(p => p.OtherId == accuont).Count();
                    result.data = data;
                    result.count = (int)Math.Ceiling(result.total * 0.1 / result.size);
                }
                catch (Exception ex)
                {
                    result.code = 500;
                    result.msg = ex.Message;
                }
                return result;
            });
        }

        /// <summary>
        /// 获取段评
        /// </summary>
        /// <returns></returns>
        public Task<Result<Comments>> GetChapterComment(int id)
        {
            return Task.Run(() =>
            {
                var result = new Result<Comments>();
                try
                {
                    List<Comments> data = _commentChapterManager.Select(new string[]
                    {
                        "TbThumbsUpChapters"
                    }).Where(p => p.Id == id)
                    .Select(p => new Comments(p))
                    .ToList();
                    SetUserData(ref data);
                    SetOtherData(ref data);
                    result.data = data.FirstOrDefault();
                }
                catch (Exception ex)
                {
                    result.code = 500;
                    result.msg = ex.Message;
                }
                return result;
            });
        }

        /// <summary>
        /// 获取回复我的段评
        /// </summary>
        /// <param name="accuont"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public Task<ResultList<List<Comments>>> GetMyChapterSubComment(string accuont, int page, int size)
        {
            return Task.Run(() =>
            {
                var result = new ResultList<List<Comments>>();
                result.page = page;
                result.size = size;
                try
                {
                    List<Comments> data = _subCommentChapterManager.Select(new string[]
                    {
                        "TbThumbsUpSubChapters"
                    }).Where(p => p.OtherId == accuont)
                    .OrderByDescending(p => p.Time)
                    .Skip((page - 1) * size)
                    .Take(size)
                    .Select(p => new Comments(p))
                    .ToList();
                    SetUserData(ref data);
                    SetOtherData(ref data);
                    result.total = _subCommentChapterManager.Select().Where(p => p.OtherId == accuont).Count();
                    result.data = data;
                    result.count = (int)Math.Ceiling(result.total * 0.1 / result.size);
                }
                catch (Exception ex)
                {
                    result.code = 500;
                    result.msg = ex.Message;
                }
                return result;
            });
        }

        /// <summary>
        /// 点赞
        /// </summary>
        /// <param name="id"></param>
        /// <param name="account"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public async Task<Result<bool?>> TumbUp(int id,string account,TumpUp mode)
        {
            var result = new Result<bool?>();
            try
            {
                switch (mode)
                {
                    case TumpUp.Book://评论
                        {
                            var data = _tumpUpBook.Select().Where(p => p.CommentId == id && p.UserId == account).FirstOrDefault();
                            if (data == null)
                            {
                                await _tumpUpBook.AppendAsync(new TbThumbsUpBook()
                                {
                                    CommentId = id,
                                    UserId = account,
                                    Up = true
                                });
                                result.data = true;
                            }
                            else
                            {
                                if (!(bool)data.Up!)
                                {
                                    data.Up = true;
                                    await _tumpUpBook.UpdateAsync(data);
                                    result.data = true;
                                }
                                else
                                {
                                    await _tumpUpBook.DeleteAsync(data.Id);
                                }
                            }
                        }
                        break;
                    case TumpUp.SubBook://子评论
                        {
                            var data = _tumpUpSubBook.Select().Where(p => p.CommentId == id && p.UserId == account).FirstOrDefault();
                            if (data == null)
                            {
                                await _tumpUpSubBook.AppendAsync(new TbThumbsUpSubBook()
                                {
                                    CommentId = id,
                                    UserId = account,
                                    Up = true
                                });
                                result.data = true;
                            }
                            else
                            {
                                if (!(bool)data.Up!)
                                {
                                    data.Up = true;
                                    await _tumpUpSubBook.UpdateAsync(data);
                                    result.data = true;
                                }
                                else
                                {
                                    await _tumpUpSubBook.DeleteAsync(data.Id);
                                }
                            }
                        }
                        break;
                    case TumpUp.Chapter://段评
                        {
                            var data = _tumpUpChapter.Select().Where(p => p.CommentId == id && p.UserId == account).FirstOrDefault();
                            if (data == null)
                            {
                                await _tumpUpChapter.AppendAsync(new TbThumbsUpChapter()
                                {
                                    CommentId = id,
                                    UserId = account,
                                    Up = true
                                });
                                result.data = true;
                            }
                            else
                            {
                                if (!(bool)data.Up!)
                                {
                                    data.Up = true;
                                    await _tumpUpChapter.UpdateAsync(data);
                                    result.data = true;
                                }
                                else
                                {
                                    await _tumpUpChapter.DeleteAsync(data.Id);
                                }
                            }
                        }
                        break;
                    case TumpUp.SubChapter://段评回复
                        {
                            var data = _tumpUpSubChapter.Select().Where(p => p.CommentId == id && p.UserId == account).FirstOrDefault();
                            if (data == null)
                            {
                                await _tumpUpSubChapter.AppendAsync(new TbThumbsUpSubChapter()
                                {
                                    CommentId = id,
                                    UserId = account,
                                    Up = true
                                });
                                result.data = true;
                            }
                            else
                            {
                                if (!(bool)data.Up!)
                                {
                                    data.Up = true;
                                    await _tumpUpSubChapter.UpdateAsync(data);
                                    result.data = true;
                                }
                                else
                                {
                                    await _tumpUpSubChapter.DeleteAsync(data.Id);
                                }
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                result.code = 500;
                result.msg = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 点踩
        /// </summary>
        /// <param name="id"></param>
        /// <param name="account"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public async Task<Result<bool?>> TumbDown(int id, string account, TumpUp mode)
        {
            var result = new Result<bool?>();
            try
            {
                switch (mode)
                {
                    case TumpUp.Book://评论
                        {
                            var data = _tumpUpBook.Select().Where(p => p.CommentId == id && p.UserId == account).FirstOrDefault();
                            if (data == null)
                            {
                                await _tumpUpBook.AppendAsync(new TbThumbsUpBook()
                                {
                                    CommentId = id,
                                    UserId = account,
                                    Up = false
                                });
                                result.data = false;
                            }
                            else
                            {
                                if ((bool)data.Up!)
                                {
                                    data.Up = false;
                                    await _tumpUpBook.UpdateAsync(data);
                                    result.data = false;
                                }
                                else
                                {
                                    await _tumpUpBook.DeleteAsync(data.Id);
                                }
                            }
                        }
                        break;
                    case TumpUp.SubBook://子评论
                        {
                            var data = _tumpUpSubBook.Select().Where(p => p.CommentId == id && p.UserId == account).FirstOrDefault();
                            if (data == null)
                            {
                                await _tumpUpSubBook.AppendAsync(new TbThumbsUpSubBook()
                                {
                                    CommentId = id,
                                    UserId = account,
                                    Up = false
                                });
                                result.data = false;
                            }
                            else
                            {
                                if ((bool)data.Up!)
                                {
                                    data.Up = false;
                                    await _tumpUpSubBook.UpdateAsync(data);
                                    result.data = false;
                                }
                                else
                                {
                                    await _tumpUpSubBook.DeleteAsync(data.Id);
                                }
                            }
                        }
                        break;
                    case TumpUp.Chapter://段评
                        {
                            var data = _tumpUpChapter.Select().Where(p => p.CommentId == id && p.UserId == account).FirstOrDefault();
                            if (data == null)
                            {
                                await _tumpUpChapter.AppendAsync(new TbThumbsUpChapter()
                                {
                                    CommentId = id,
                                    UserId = account,
                                    Up = false
                                });
                                result.data = false;
                            }
                            else
                            {
                                if ((bool)data.Up!)
                                {
                                    data.Up = false;
                                    await _tumpUpChapter.UpdateAsync(data);
                                    result.data = false;
                                }
                                else
                                {
                                    await _tumpUpChapter.DeleteAsync(data.Id);
                                }
                            }
                        }
                        break;
                    case TumpUp.SubChapter://段评回复
                        {
                            var data = _tumpUpSubChapter.Select().Where(p => p.CommentId == id && p.UserId == account).FirstOrDefault();
                            if (data == null)
                            {
                                await _tumpUpSubChapter.AppendAsync(new TbThumbsUpSubChapter()
                                {
                                    CommentId = id,
                                    UserId = account,
                                    Up = false
                                });
                                result.data = false;
                            }
                            else
                            {
                                if ((bool)data.Up!)
                                {
                                    data.Up = false;
                                    await _tumpUpSubChapter.UpdateAsync(data);
                                    result.data = false;
                                }
                                else
                                {
                                    await _tumpUpSubChapter.DeleteAsync(data.Id);
                                }
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                result.code = 500;
                result.msg = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 检测是否点赞
        /// </summary>
        /// <param name="id"></param>
        /// <param name="account"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public Task<Result<bool?>> Check(int id, string account, TumpUp mode)
        {
            return Task.Run(() =>
            {
                Result<bool?> result = new Result<bool?>();
                try
                {
                    switch (mode)
                    {
                        case TumpUp.Book://评论
                            {
                                var data = _tumpUpBook.Select().Where(p => p.CommentId == id && p.UserId == account).FirstOrDefault();
                                if (data != null)
                                {
                                    result.data = data.Up;
                                }
                            }
                            break;
                        case TumpUp.SubBook://子评论
                            {
                                var data = _tumpUpSubBook.Select().Where(p => p.CommentId == id && p.UserId == account).FirstOrDefault();
                                if (data != null)
                                {
                                    result.data = data.Up;
                                }
                            }
                            break;
                        case TumpUp.Chapter://段评
                            {
                                var data = _tumpUpChapter.Select().Where(p => p.CommentId == id && p.UserId == account).FirstOrDefault();
                                if (data != null)
                                {
                                    result.data = data.Up;
                                }
                            }
                            break;
                        case TumpUp.SubChapter://段评回复
                            {
                                var data = _tumpUpSubChapter.Select().Where(p => p.CommentId == id && p.UserId == account).FirstOrDefault();
                                if (data != null)
                                {
                                    result.data = data.Up;
                                }

                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    result.code = 500;
                    result.msg = ex.Message;
                }
                return result;
            });
        }


        private void SetUserData(ref List<Comments> list)
        {
            list.ForEach(p =>
            {
                var user = _userDataManager.First(i=>i.Account == p.UserId);
                p.UserName =user!.NickName ;
                p.ImgUrl = user!.ImgUrl ;
            });
        }

        private void SetOtherData(ref List<Comments> list)
        {
            list.ForEach(p =>
            {
                p.OtherName =_userDataManager.First(i => i.Account == p.OtherUserId)!.NickName;
            });
        }
    }
}
