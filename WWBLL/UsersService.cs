using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WWDAL;
using WWModel.Result;
using WWModel.Models;
using WWModel.input;
using Tools;

namespace WWBLL
{
    public enum PwdMdf
    {
        Modify, Forget
    }
    /// <summary>
    /// 用户服务类
    /// </summary>
    public class UsersService:BaseService<TbUser>
    {
        private readonly UserDataManager _userDataManager;
        private readonly BaseManager<TbSalt> _saltManager;
        private readonly BaseManager<TbRole> _roleManager;
        private readonly BaseManager<TbSubCommentBook> _subCommentBook;
        private readonly BaseManager<TbSubCommentChapter> _subCommentChapter;
        private readonly BaseManager<TbCommentBook> _commentBook;
        private readonly BaseManager<TbCommentChapter> _commentChapter;
        private readonly BaseManager<TbBook> _bookManager;

        public UsersService(WWDBContext db) : base(db){
            _userDataManager = new UserDataManager(db);
            _saltManager = new BaseManager<TbSalt>(db);
            _roleManager = new BaseManager<TbRole>(db);
            _subCommentBook = new BaseManager<TbSubCommentBook>(db);
            _subCommentChapter = new BaseManager<TbSubCommentChapter>(db);
            _commentBook = new BaseManager<TbCommentBook>(db);
            _commentChapter = new BaseManager<TbCommentChapter>(db);
            _bookManager = new BaseManager<TbBook>(db);
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="id">账号</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public Task<Result<string>> Login(IConfiguration configuration, string id, string pwd)
        {
            return Task.Run(async () =>
            {
                Result<string> result = new Result<string>();
                TbUser? users = _manager.Find(id);
                if (users == null)
                {
                    result.code = 500;
                    result.msg = "用户不存在";
                }
                else if (users.Password!.Decrypt() != (pwd + (await _saltManager.FirstAsync(i => i.UserId == id))!.Salt!.Decrypt()).ToSHA256())
                {
                    result.code = 500;
                    result.msg = "密码错误";
                }
                else
                {
                    //创建Token
                    var claims = new List<Claim>
                    {
                        new Claim("id",users.Id!),
                        new Claim("data",_userDataManager.First(i=>i.Account == users.Id)!.Id.ToString()),
                        new Claim("role",users.RoleId.ToString()!)
                    };
                    var token = configuration.GetSection("JwtConfig");
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(token["Key"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var resToken = new JwtSecurityToken(
                        issuer: token["Issuer"],
                        audience: token["Audience"],
                        claims: claims,
                        expires: DateTime.Now.AddDays(30),
                        signingCredentials: creds
                        );
                    result.data = new JwtSecurityTokenHandler().WriteToken(resToken);
                }
                return result;
            });
        }

        /// <summary>
        /// 判断账号是否可用
        /// </summary>
        /// <param name="Account"></param>
        /// <returns></returns>
        public Task<Result<bool>> IsAccountExist(string Account)
        {
            return Task.Run(async () =>
            {
                if (await _manager.IsExist(i => i.Id == Account))
                {
                    return new Result<bool>()
                    {
                        code = 500,
                        data = false,
                        msg = "该账号已存在，请重新尝试"
                    };
                }
                else
                {
                    return new Result<bool>()
                    {
                        data = true,
                        msg = "账号可以使用"
                    };
                }
            });
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task<Result<string>> Register(RegisterModel model,int role)
        {
            return Task.Run(async () =>
            {
                if (await _manager.IsExist(i=>i.Id == model.Account))
                {
                    return new Result<string>()
                    {
                        code = 500,
                        data = "error",
                        msg = "该账号已存在，请重新尝试"
                    };
                }
                Result<string> result = new Result<string>();
                result.code = await _userDataManager.Register(model, role);
                if (result.code == 500)
                {
                    result.data = "error";
                    result.msg = "注册失败请重新尝试";
                }
                else
                {
                    result.data = "success";
                }
                return result;
            });
        }

        /// <summary>
        /// 按条件查询用户数量
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public Task<int> GetCount(Func<TbUser,bool> where)
        {
            return Task.Run(() =>
            {
                return _manager.Select()
                .Where(where)
                .Count();
            });
        }

        /// <summary>
        /// 获取可用操作方法
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public Task<List<string>> GetGrantAction(int role)
        {
            return Task.Run(() =>
            {
                return _roleManager.Find(role)!
                .TbGrants
                .Select(p => p.Auth.Action!)
                .Distinct()
                .ToList();
            });
        }

        /// <summary>
        /// 忘记密码
        /// </summary>
        /// <returns></returns>
        public Task<Result<string>> ForgetPassword(PwdChangeModel model)
        {
            return Task.Run(async () =>
            {
                
                var result = new Result<string>()
                {
                    code = await _userDataManager.ForgetPwd(model)
                };
                if (result.code == 500)
                {
                    result.data = "操作出错，请重新尝试";
                }
                return result;
            });
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task<Result<string>> ModifyPassword(PwdChangeModel model)
        {
            return Task.Run(async () =>
            {
                int code = await _userDataManager.ModifyPwd(model);
                var result = new Result<string>()
                {
                    code = code ==200?200:500
                };
                if (code == 501)
                {
                    result.data = "旧密码不正确，请重新尝试";
                }
                else if (code == 500)
                {
                    result.data = "操作出错，请重新尝试";
                }
                return result;
            });
        }

        /// <summary>
        /// 绑定身份证号
        /// </summary>
        /// <param name="account"></param>
        /// <param name="idCard"></param>
        /// <returns></returns>
        public async Task<Result<bool>> BindCard(string account, string idCard)
        {
            var result = new Result<bool>();
            var user = await _userDataManager.FindAsync(account);
            user!.IdCard = idCard;
            result.data = await _userDataManager.UpdateAsync(user);
            if (!result.data)
            {
                result.code = 500;
                result.msg = "绑定失败";
            }
            return result;
        }

        /// <summary>
        /// 绑定银行卡号
        /// </summary>
        /// <param name="account"></param>
        /// <param name="BankCard"></param>
        /// <returns></returns>
        public async Task<Result<bool>> BindBank(string account, string bankCard)
        {
            var result = new Result<bool>();
            var user = await _userDataManager.FindAsync(account);
            user!.BankCard = bankCard;
            result.data = await _userDataManager.UpdateAsync(user);
            if (!result.data)
            {
                result.code = 500;
                result.msg = "绑定失败";
            }
            return result;
        }

        /// <summary>
        /// 成为作家
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task<Result<string>> ApplyWirter(IConfiguration configuration, string account)
        {
            var user = await  _userDataManager.FirstAsync(p=>p.Account == account);
            if (user == null)
            {
                return new Result<string>()
                {
                    code = 500,
                    msg ="用户不存在"
                };
            }
            else if (user.IdCard == null)
            {
                return new Result<string>()
                {
                    code = 500,
                    msg = "请先绑定身份证号才可成为作家"
                };
            }
            else
            {
                var u = await _manager.FindAsync(account);
                u!.RoleId = 3;
                await _manager.UpdateAsync(u);
                var claims = new List<Claim>
                    {
                        new Claim("id",u.Id!),
                        new Claim("data",_userDataManager.First(i=>i.Account == u.Id)!.Id.ToString()),
                        new Claim("role",u.RoleId.ToString()!)
                    };
                var token = configuration.GetSection("JwtConfig");
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(token["Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var resToken = new JwtSecurityToken(
                    issuer: token["Issuer"],
                    audience: token["Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddDays(30),
                    signingCredentials: creds
                    );
                return new Result<string>()
                {
                    data = new JwtSecurityTokenHandler().WriteToken(resToken)
                };
            }
        }

        /// <summary>
        /// 获取关于我的回复
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task<ResultList<List<Comments>>> GetSubCommentsByMe(string account,int page, int size)
        {
            try
            {
                var lq = _subCommentBook.Select(new string[]
                {
                    "TbThumbsUpSubBooks"
                }).Where(p => p.OtherId == account && p.UserId != account);
                var data = new List<Comments>();
                var data1 = lq
                    .OrderByDescending(p => p.Time)
                    .Skip((page - 1) * size)
                    .Take(size)
                    .ToList();
                foreach (var item in data1)
                {
                    Comments d = new Comments(item);
                    d.BookId = item.Parent!.BookId;
                    d.BookName = item.Parent!.Book!.Title;
                    d.UserName = (await _userDataManager.FirstAsync(p => p.Account == d.UserId))!.NickName;
                    data.Add(d);
                }
                var lq2 = _subCommentChapter.Select(new string[]
                {
                    "TbThumbsUpSubChapters"
                }).Where(p => p.OtherId == account&&p.UserId != account);
                var data2 = lq2
                    .OrderByDescending(p => p.Time)
                    .Skip((page - 1) * size)
                    .Take(size)
                    .ToList();
                foreach (var item in data2)
                {
                    Comments d = new Comments(item);
                    d.ChapterId = item.Parent!.ChapterId;
                    d.ScrollId = item.Parent.Chapter!.ScrollId;
                    d.BookId = item.Parent.Chapter!.Scroll!.BookId;
                    d.BookName = item.Parent.Chapter!.Scroll!.Book!.Title;
                    d.UserName = (await _userDataManager.FirstAsync(p=>p.Account == d.UserId))!.NickName;
                    data.Add(d);
                }
                int total = lq.Count() +lq2.Count();
                int count = (int)Math.Ceiling(total * 1.0 / size);
                return new ResultList<List<Comments>>()
                {
                    data = data,
                    total = total,
                    count = count,
                    page = page,
                    size = size,
                };
            }
            catch (Exception ex)
            {
                return new ResultList<List<Comments>>()
                {
                    code = 500,
                    msg = ex.Message,
                };
            }
        }

        /// <summary>
        /// 获取关于我的回复
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task<ResultList<List<Comments>>> GetCommentsByMe(string account, int page, int size)
        {
            try
            {
                var lq = _commentBook.Select(new string[]
                {
                    "TbThumbsUpBooks"
                }).Where(p => p.UserId == account);
                var data = new List<Comments>();
                var data1 = lq
                    .OrderByDescending(p => p.Time)
                    .Skip((page - 1) * size)
                    .Take(size)
                    .ToList();
                foreach (var item in data1)
                {
                    Comments d = new Comments(item);
                    d.BookId = item.BookId;
                    d.BookName = item.Book!.Title;
                    d.UserName = (await _userDataManager.FirstAsync(p => p.Account == d.UserId))!.NickName;
                    data.Add(d);
                }
                var lq2 = _commentChapter.Select(new string[]
                {
                    "TbThumbsUpChapters"
                }).Where(p => p.UserId == account);
                var data2 = lq2
                    .OrderByDescending(p => p.Time)
                    .Skip((page - 1) * size)
                    .Take(size)
                    .ToList();
                foreach (var item in data2)
                {
                    Comments d = new Comments(item);
                    d.ChapterId = item.ChapterId;
                    d.ScrollId = item.Chapter!.ScrollId;
                    d.BookId = item.Chapter!.Scroll!.BookId;
                    d.BookName = item.Chapter!.Scroll!.Book!.Title;
                    d.UserName = (await _userDataManager.FirstAsync(p => p.Account == d.UserId))!.NickName;
                    data.Add(d);
                }
                int total = lq.Count() + lq2.Count();
                int count = (int)Math.Ceiling(total * 1.0 / size);
                return new ResultList<List<Comments>>()
                {
                    data = data,
                    total = total,
                    count = count,
                    page = page,
                    size = size,
                };
            }
            catch (Exception ex)
            {
                return new ResultList<List<Comments>>()
                {
                    code = 500,
                    msg = ex.Message,
                };
            }
        }

    }
}