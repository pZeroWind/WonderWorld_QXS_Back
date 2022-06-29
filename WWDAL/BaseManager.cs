using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WWModel.Models;

namespace WWDAL
{
    public class BaseManager<T> where T : class
    {
        public WWDBContext _db { get; private set; }

        public BaseManager(WWDBContext db)
        {
            _db = db;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> Select()
        {
            return _db.Set<T>();
        }

        /// <summary>
        /// 查询列表-贪婪加载
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public IQueryable<T> Select(string[] table)
        {
            IQueryable<T> res = _db.Set<T>();
            foreach (string item in table)
            {
                res = res.Include(item);
            }
            return res;
        }

        /// <summary>
        /// 是否存在符号条件的数据
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public Task<bool> IsExist(Func<T,bool> where)
        {
            return Task.Run(() => _db.Set<T>().Where(where).Count() > 0);
        }

        /// <summary>
        /// 查询对象
        /// </summary>
        /// <typeparam name="S">ID</typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T? Find<S>(S id)
        {
            return _db.Set<T>().Find(id);
        }

        /// <summary>
        /// 查询对象 -- 异步
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T?> FindAsync<S>(S id)
        {
            return await  _db.Set<T>().FindAsync(id);
        }

        /// <summary>
        /// 获取第一个对象
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public T? First(Func<T,bool> where)
        {
            return _db.Set<T>().AsNoTracking().Where(where).FirstOrDefault();
        }

        /// <summary>
        /// 获取第一个对象 -- 异步
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public Task<T?> FirstAsync(Func<T, bool> where)
        {
            return Task.Run(() =>
            {
                return _db.Set<T>().AsNoTracking().Where(where).FirstOrDefault();
            });
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <typeparam name="S">ID</typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete<S>(S id)
        {
            T? model = _db.Set<T>().Find(id);
            if (model != null)
            {
                _db.Set<T>().Remove(model);
            }
            return _db.SaveChanges() > 0;
        }

        /// <summary>
        /// 删除所有
        /// </summary>
        /// <returns></returns>
        public Task DeleteAll()
        {
            return Task.Run(async () =>
            {
                _db.Set<T>().DeleteFromQuery();
                await _db.BulkSaveChangesAsync();
            });
        }

        

        public Task AppendRange(List<T> data)
        {
            return Task.Run(async () =>
            {
                await _db.Set<T>().AddRangeAsync(data);
                await _db.SaveChangesAsync();
            });
        }

        /// <summary>
        /// 删除对象 -- 异步
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<bool> DeleteAsync<S>(S id)
        {
            return Task.Run(async () =>
            {
                T? model = await _db.Set<T>().FindAsync(id);
                if (model != null)
                {
                    _db.Set<T>().Remove(model);
                }
                return await _db.SaveChangesAsync() > 0;
            });
        }

        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="model">对象模型</param>
        /// <returns></returns>
        public bool Append(T model)
        {
            _db.Set<T>().Add(model);
            return _db.SaveChanges() > 0;
        }

        /// <summary>
        /// 添加对象 -- 异步
        /// </summary>
        /// <param name="model">对象模型</param>
        /// <returns></returns>
        public Task<bool> AppendAsync(T model)
        {
            return Task.Run(async () =>
            {
                await _db.Set<T>().AddAsync(model);
                return await _db.SaveChangesAsync() > 0;
            });
        }

        /// <summary>
        /// 更新对象模型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update(T model)
        {
            _db.Set<T>().Update(model);
            return _db.SaveChanges() > 0;
        }

        /// <summary>
        /// 更新对象模型 -- 异步
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task<bool> UpdateAsync(T model)
        {
            return Task.Run(async () =>
            {
                _db.Set<T>().Update(model);
                return await _db.SaveChangesAsync() > 0;
            });
        }

        public async Task<bool> IsTransactionSuccess()
        {
            if (await _db.SaveChangesAsync() <= 0)
            {
                await _db.Database.RollbackTransactionAsync();
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
