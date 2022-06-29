using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WWDAL;
using WWModel.Result;
using WWModel.Models;
using Tools;
using WWModel.input;

namespace WWBLL
{
    

    public class UserDataService:BaseService<TbUserData>
    {
        private readonly BaseManager<TbItem> _item;
        private readonly BaseManager<TbUser> _user;

        public UserDataService(WWDBContext db) : base(db){
            _item = new BaseManager<TbItem>(db);
            _user = new BaseManager<TbUser>(db);
        }

        /// <summary>
        /// 个人资料修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Result<bool>> Update(ModifyModel model)
        {
            var data = await _manager.FirstAsync(p=>p.Account == model.Account);
            //不为空则修改
            if (data!=null)
            {
                data.Email = model.Email;
                data.NickName = model.NickName;
                data.Birthday = model.Birthday;
                data.Gender = model.Gender;
                data.ImgUrl = model.ImgUrl;
                data.Tel = model.Tel;
                return await Update(data);
            }
            else
            {
                return new Result<bool>
                {
                    code = 500,
                    data = false,
                    msg = "用户不存在"
                };
            }
            
        }

        /// <summary>
        /// 修改身份
        /// </summary>
        /// <returns></returns>
        public async Task<Result<bool>> RoleChange(string account,int role)
        {
            try
            {
                var u = await _user.FindAsync(account);
                if (u != null)
                {
                    u.RoleId = role;
                    return new Result<bool>()
                    {
                        data= await _user.UpdateAsync(u)
                    };
                }
                else
                {
                    return new Result<bool>() { code = 404, msg = "用户不存在" };
                }
            }
            catch(Exception ex)
            {
                return new Result<bool>(){ code = 500, msg = ex.Message };
            }
        }

        /// <summary>
        /// 查找用户资料
        /// </summary>
        /// <param name="db"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public override Task<Result<TbUserData>> Find<S>(S id)
        {
            return Task.Run(async () =>
            {
                var result = await base.Find(id);
                if (result.data == null)
                {
                    result.code = 500;
                    result.msg = "该用户不存在";
                }
                return result;
            });
        }

        /// <summary>
        /// 按账号查询用户资料
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public Task<Result<UserData>> GetData(Func<TbUserData, bool> where)
        {
            return Task.Run(async () =>
            {
                Result<UserData> result = new Result<UserData>()
                {
                    data = new UserData(await _manager.FirstAsync(where))
                };
                if (result.data != null)
                {
                    if (result.data.BankCard != null)
                    {
                        result.data.BankCard = result.data.BankCard.ToHide();
                    }
                    if (result.data.IdCard != null)
                    {
                        result.data.IdCard = result.data.IdCard.ToHide();
                    }
                    var item = await _item.FirstAsync(p=>p.UserId == result.data.Account);
                    result.data.Coin = item!.CoinNum;
                    result.data.Tiket = item!.TiketNum;
                    result.data.Blade = item!.BladeNum;
                }
                else
                {
                    result.code = 404;
                    result.msg = "用户资料不存在";
                }
                return result;
            });
        }

        /// <summary>
        /// 获取热门作家
        /// </summary>
        /// <returns></returns>
        public async Task<Result<List<UserData>>> GetHot()
        {
            var list = _item.Select()
                .Where(p => p.User!.RoleId != 4)
                .OrderByDescending(p => p.Money)
                .Take(5)
                .Select(p => p.UserId).ToList();
            var data = new List<UserData>();
            foreach (var item in list)
            {
                data.Add(new UserData((await _manager.FirstAsync(p => p.Account == item))!));
            }
            var result = new Result<List<UserData>>()
            {
                data = data
            };
            return result;
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="type"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResultList<List<UserDataPlus>>> GetHot(int type,int page,int size,string? id = null)
        {
            var lq = id != null ? _user.Select(new string[] { "Role" })
                .Where(p => p.RoleId == type && p.Id == id) : _user.Select(new string[] { "Role" })
                .Where(p => p.RoleId == type);
            var list = lq
                .OrderByDescending(p=>p.TbUserData.First().RegisterTime)
                .Select(p => new { p.Id,p.Role })
                .Skip((page - 1) * size)
                .Take(size)
                .ToList();
            var data = new List<UserDataPlus>();
            foreach (var item in list)
            {
                var dataPlus = new UserDataPlus((await _manager.FirstAsync(p => p.Account == item.Id))!);
                dataPlus.Role = item.Role!.RoleName;
                data.Add(dataPlus);
            }
            int t = lq.Count();
            var result = new ResultList<List<UserDataPlus>>()
            {
                page = page,
                size = size,
                total = t,
                count = (int)Math.Ceiling(t*1.0/size),
                data = data
            };
            return result;
        }
    }
}
