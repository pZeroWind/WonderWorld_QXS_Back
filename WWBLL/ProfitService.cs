using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WWDAL;
using WWModel.Models;
using WWModel.Result;

namespace WWBLL
{
    public class ProfitService : BaseService<TbProfit>
    {
        private readonly BaseManager<TbProfitMod> _mod;
        private readonly BaseManager<TbItem> _item;
        private readonly BaseManager<TbBook> _book;
        private readonly BaseManager<TbChapter> _chapter;
        private readonly BaseManager<TbConsume> _consume;

        public ProfitService(WWDBContext db) : base(db)
        {
            _mod = new BaseManager<TbProfitMod>(db);
            _item = new BaseManager<TbItem>(db);
            _book = new BaseManager<TbBook>(db);
            _chapter = new BaseManager<TbChapter>(db);
            _consume = new BaseManager<TbConsume>(db);

        }

        /// <summary>
        /// 投递月票
        /// </summary>
        public async Task<Result<bool>> GiveTiket(string bookId, string account)
        {
            try
            {
                var data = _item.Select().Where(x => x.UserId == account).FirstOrDefault();
                if (data==null)
                {
                    return new Result<bool>()
                    {
                        code = 404,
                        msg = "用户不存在"
                    };
                }
                if (data.TiketNum == 0)
                {
                    return new Result<bool>()
                    {
                        code = 400,
                        msg = "你已经没有月票了"
                    };
                }
                data.TiketNum-=1;
                await _item.UpdateAsync(data);
                await _manager.AppendAsync(new TbProfit()
                {
                    BookId = bookId,
                    Num = 1,
                    ProfitModId = 1,
                    UserId = account,
                    Time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                });
                await _consume.AppendAsync(new TbConsume()
                {
                    BookId = bookId,
                    Num = 1,
                    ConsumeModeId = 1,
                    ConsumeTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    UserId = account,
                });
                return new Result<bool>() { data = true };
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
        /// 投递刀片
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task<Result<bool>> GiveBlade(string bookId, string account)
        {
            try
            {
                var data = _item.Select().Where(x => x.UserId == account).FirstOrDefault();
                if (data == null)
                {
                    return new Result<bool>()
                    {
                        code = 404,
                        msg = "用户不存在"
                    };
                }
                if (data.BladeNum == 0)
                {
                    return new Result<bool>()
                    {
                        code = 400,
                        msg = "你已经没有刀片了"
                    };
                }
                data.BladeNum -= 1;
                await _item.UpdateAsync(data);
                await _manager.AppendAsync(new TbProfit()
                {
                    BookId = bookId,
                    Num = 1,
                    ProfitModId = 2,
                    UserId = account,
                    Time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                });
                await _consume.AppendAsync(new TbConsume()
                {
                    BookId = bookId,
                    Num = 1,
                    ConsumeModeId = 2,
                    ConsumeTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    UserId = account,
                });
                return new Result<bool>() { data = true };
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
        /// 购买章节
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="account"></param>
        /// <param name="chapterId"></param>
        /// <returns></returns>
        public async Task<Result<bool>> BuyChapter(string bookId, string account ,int chapterId)
        {
            try
            {
                var data = _item.Select().Where(x => x.UserId == account).FirstOrDefault();
                if (data == null)
                {
                    return new Result<bool>()
                    {
                        code = 404,
                        msg = "用户不存在"
                    };
                }
                int coin = (int)Math.Ceiling(((await _chapter.FindAsync(chapterId))!.Content!.Length*1.0 / 100) * 0.5);
                if (data.CoinNum < coin)
                {
                    return new Result<bool>()
                    {
                        code = 400,
                        msg = "硬币余额不足"
                    };
                }
                data.CoinNum -= coin;
                await _item.UpdateAsync(data);
                await _manager.AppendAsync(new TbProfit()
                {
                    BookId = bookId,
                    Num = coin,
                    ProfitModId = 3,
                    UserId = account,
                    Time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                });
                var w = await _item.FirstAsync(p=>p.UserId == _book.Find(bookId)!.UserId);
                w.Money = (decimal)(coin * 0.008);
                await _item.UpdateAsync(w);
                await _consume.AppendAsync(new TbConsume()
                {
                    BookId = bookId,
                    Num = coin,
                    ConsumeModeId = 3,
                    ConsumeTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    UserId = account,
                    ChapterId = chapterId,
                });
                return new Result<bool>() { data = true };
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
