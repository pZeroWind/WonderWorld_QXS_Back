using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WWModel.Models;
using WWModel.Result;

namespace WWBLL
{
    public class ReportService:BaseService<TbReportBook>
    {
        private readonly WWDBContext _db;

        public ReportService(WWDBContext db) : base(db)
        {
            _db = db;
        }

        /// <summary>
        /// 获取举报列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public Task<ResultList<List<ReportData>>> GetData(int page,int size)
        {
            try
            {
                var data = _db.TbReportBooks
                    .OrderByDescending(p=>p.Id)
                    .Skip((page-1)*size)
                    .Take(size)
                    .Select(p => new ReportData(p)).ToList();
                int count = _db.TbReportBooks.Count();
                return Task.FromResult(new ResultList<List<ReportData>>()
                { 
                    total = count,
                    page = page,
                    size = size,
                    count = (int)Math.Ceiling(count*1.0/size),
                    data = data
                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new ResultList<List<ReportData>>()
                {
                    code = 500,
                    msg = ex.Message,
                });
            }
        }

        /// <summary>
        /// 新增举报信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<Result<bool>> Add(ReportData data)
        {
            try
            {
                await _db.TbReportBooks.AddAsync(new TbReportBook()
                {
                    Details = data.Details,
                    ChapterId = data.ChapterId,
                    UserId = data.UserId,
                    State = 1
                });
                await _db.SaveChangesAsync();
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
                    msg = ex.Message,
                };
            }
        }

        /// <summary>
        /// 已受理举报信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<Result<bool>> Complete(int id)
        {
            try
            {
                var book = await _db.TbReportBooks.FindAsync(id);
                book!.State = 2;
                _db.Update(book);
                await _db.SaveChangesAsync();
                return new Result<bool>()
                {
                    data = true
                };
            }
            catch (Exception ex)
            {
                return new Result<bool>()
                {
                    code = 500,
                    msg = ex.Message,
                };
            }
        }
    }
}
