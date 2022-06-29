using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WWModel.input;
using WWModel.Models;
using Tools;

namespace WWDAL
{
    public class UserDataManager:BaseManager<TbUserData>
    {
        private readonly BaseManager<TbItem> _item;

        public UserDataManager(WWDBContext db) : base(db)
        {
        }

        /// <summary>
        /// 注册事务
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> Register(RegisterModel model, int role)
        {
            try
            {
                //开启事务
                await _db.Database.BeginTransactionAsync();
                string salt = Password.GetSalt();
                model.Password = (model.Password + salt).ToSHA256Encrypt();
                //保存账号信息
                await _db.TbUsers.AddAsync(new TbUser()
                {
                    Id = model.Account!,
                    Password = model.Password,
                    RoleId = role
                });
                //保存是否成功
                if (!await IsTransactionSuccess())
                {
                    return 500;
                }
                //添加盐信息
                await _db.TbSalts.AddAsync(new TbSalt()
                {
                    Salt = salt.Encrypt(),
                    UserId = model.Account!,
                });
                //保存是否成功
                if (!await IsTransactionSuccess())
                {
                    return 500;
                }
                //添加item信息
                await _db.TbItems.AddAsync(new TbItem()
                {
                    UserId = model.Account!,
                });
                //保存是否成功
                if (!await IsTransactionSuccess())
                {
                    return 500;
                }
                //添加个人信息
                await _db.TbUserData.AddAsync(new TbUserData()
                {
                    Account = model.Account!,
                    Email = model.Email,
                    Birthday = model.Birthday,
                    Gender = model.Gender,
                    ImgUrl = model.ImgUrl,
                    NickName = model.NickName,
                    RegisterTime = model.RegisterTime,
                    Tel = model.Tel
                });
                //保存是否成功
                if (!await IsTransactionSuccess())
                {
                    return 500;
                }
                await _db.Database.CommitTransactionAsync();
                return 200;
            }
            catch (Exception ex)
            {
                Log.Print(ex.Message, PrintMode.Error);
                return 500;
            }
        } 

        /// <summary>
        /// 忘记密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> ForgetPwd(PwdChangeModel model)
        {
            try
            {
                //开启事务
                await _db.Database.BeginTransactionAsync();
                //随机盐值
                string salt = Password.GetSalt();
                model.Password = (model.Password + salt).ToSHA256Encrypt();
                //获取模型
                var saltData = _db.TbSalts.Where(p => p.UserId == model.Account).FirstOrDefault();
                if (saltData == null)
                {
                    await _db.Database.RollbackTransactionAsync();
                    return 500;
                }
                //修改盐值
                saltData.Salt = salt.Encrypt();
                _db.TbSalts.Update(saltData);
                if (!await IsTransactionSuccess())
                {
                    return 500;
                }
                //获取用户
                var user = _db.TbUsers.Find(model.Account);
                if (user == null)
                {
                    await _db.Database.RollbackTransactionAsync();
                    return 500;
                }
                user.Password = model.Password;
                _db.TbUsers.Update(user);
                if (!await IsTransactionSuccess())
                {
                    return 500;
                }
                await _db.Database.CommitTransactionAsync();
                return 200;
            }
            catch (Exception ex)
            {
                Log.Print(ex.Message, PrintMode.Error);
                return 500;
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> ModifyPwd(PwdChangeModel model)
        {
            try
            {
                //开启事务
                await _db.Database.BeginTransactionAsync();
                //随机盐值
                string salt = Password.GetSalt();
                model.Password = (model.Password + salt).ToSHA256Encrypt();
                //获取模型
                var saltData = _db.TbSalts.Where(p => p.UserId == model.Account).FirstOrDefault();
                if (saltData == null)
                {
                    await _db.Database.RollbackTransactionAsync();
                    return 500;
                }
                //获取用户
                var user = _db.TbUsers.Find(model.Account);
                if (user == null&&(model.oldPassword+saltData.Salt!.Decrypt()).ToSHA256() == user!.Password!.Decrypt())
                {
                    await _db.Database.RollbackTransactionAsync();
                    return 501;
                }
                //修改盐值
                saltData.Salt = salt.Encrypt();
                _db.TbSalts.Update(saltData);
                if (!await IsTransactionSuccess())
                {
                    return 500;
                }
                
                user.Password = model.Password;
                _db.TbUsers.Update(user);
                if (!await IsTransactionSuccess())
                {
                    return 500;
                }
                await _db.Database.CommitTransactionAsync();
                return 200;
            }
            catch (Exception ex)
            {
                Log.Print(ex.Message, PrintMode.Error);
                return 500;
            }
        }
    }
}
