using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    public static class Token
    {
        /// <summary>
        /// 获取身份
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public static int GetRole(this string header)
        {
            try
            {
                string token = header.Replace("Bearer ", "");
                var handler = new JwtSecurityTokenHandler();
                return int.Parse(handler.ReadJwtToken(token).Payload.Claims.Where(p=>p.Type=="role").First().Value);
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 获取数据编号
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public static int GetData(this string header)
        {
            try
            {
                string token = header.Replace("Bearer ", "");
                var handler = new JwtSecurityTokenHandler();
                return int.Parse(handler.ReadJwtToken(token).Payload.Claims.Where(p => p.Type == "data").First().Value);
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 获取账号
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public static string GetAccount(this string header)
        {
            try
            {
                string token = header.Replace("Bearer ", "");
                var handler = new JwtSecurityTokenHandler();
                return handler.ReadJwtToken(token).Payload.Claims.Where(p => p.Type == "id").First().Value;
            }
            catch
            {
                return "";
            }
        }
    }
}
