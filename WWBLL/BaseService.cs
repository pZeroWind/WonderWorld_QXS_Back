using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WWDAL;
using WWModel.Models;
using WWModel.Result;

namespace WWBLL
{
    public abstract class BaseService<T> where T : class
    {
        protected BaseManager<T> _manager { get; set; }

        public BaseService(WWDBContext db)
        {
            _manager = new BaseManager<T>(db); 
        }

        /// <summary>
        /// 查找对象
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Task<Result<T>> Find<S>(S id)
        {
            return Task.Run(async ()=>new Result<T>() { data = await _manager.FindAsync(id) });
        }

        /// <summary>
        /// 按条件查询第一个
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual Task<Result<T>> First(Func<T,bool> where)
        {
            return Task.Run(async ()=>new Result<T>() { data = await _manager.FirstAsync(where) }) ;
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual Task<Result<List<T>>> GetList(Func<T,bool> where)
        {
            return Task.Run(()=>new Result<List<T>>() { data = _manager.Select().AsNoTracking().Where(where).ToList() } ) ;
        }

        /// <summary>
        /// 排序查询
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="where"></param>
        /// <param name="orderDesc"></param>
        /// <returns></returns>
        public virtual Task<Result<List<T>>> GetList<S>(Func<T,bool> where,Func<T,S> orderDesc)
        {
            return Task.Run(()=> new Result<List<T>>() { data = _manager.Select().AsNoTracking().Where(where).OrderByDescending(orderDesc).ToList() }) ;
        }

        /// <summary>
        /// 分页排序查询
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="where"></param>
        /// <param name="orderDesc"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public virtual Task<Result<PageList<T>>> GetList<S>(Func<T,bool> where,Func<T,S> orderDesc,int page,int size)
        {
            return Task.Run(() =>
            {
                int total = _manager.Select().AsNoTracking().Count();
                PageList<T> result = new PageList<T>(_manager.Select().
                    AsNoTracking()
                    .Where(where)
                    .OrderByDescending(orderDesc)
                    .Skip((page - 1) * size)
                    .Take(size)
                    )
                {
                    total = total,
                    page = page,
                    size = size,
                    limt = (int)Math.Ceiling(total * 1.0 / size)
                };
                return new Result<PageList<T>>() { data = result };
            });
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual Task<Result<bool>> Append(T model)
        {
            return Task.Run(async ()=> new Result<bool>() { data = await _manager.AppendAsync(model) } ) ;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual Task<Result<bool>> Update(T model)
        {
            return Task.Run(async ()=> new Result<bool>() { data = await _manager.UpdateAsync(model) }) ;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual Task<Result<bool>> Remove<S>(S id)
        {
            return Task.Run(async ()=>new Result<bool>() { data = await _manager.DeleteAsync(id) }) ;
        }
    }
}
