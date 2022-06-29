using Dapper;
using MySql.Data.MySqlClient;

namespace WWModel.Models
{
    public class DbLink
    {
        private readonly MySqlConnection _conn;

        public DbLink(string connStr)
        {
            _conn = new MySqlConnection(connStr);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> Select<T>(string sql)
        {
            return await _conn.QueryAsync<T>(sql);
        }
        public async Task<IEnumerable<T>> Select<T>(string sql,object model)
        {
            return await _conn.QueryAsync<T>(sql, model);
        }

        /// <summary>
        /// 查询第一个
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<T> Find<T>(string sql)
        {
            return await _conn.QueryFirstOrDefaultAsync<T>(sql);
        }
        public async Task<T> Find<T>(string sql, object model)
        {
            return await _conn.QueryFirstOrDefaultAsync<T>(sql, model);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<int> Update(string sql)
        {
            return await _conn.ExecuteAsync(sql);
        }
        public async Task<int> Update(string sql,object model)
        {
            return await _conn.ExecuteAsync(sql, model);
        }
    }
}
