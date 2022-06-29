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
    public class AuthService : BaseService<TbGrant>
    {
        private readonly BaseManager<TbAuth> _auth;
        private readonly BaseManager<TbRole> _role;
        private readonly WWDBContext _db;

        public AuthService(WWDBContext db) : base(db)
        {
            _auth = new BaseManager<TbAuth>(db);
            _role = new BaseManager<TbRole>(db);
            _db = db;
        }

        public async Task<Result<List<Grant>>> GetAllGrant(int id)
        {
            try
            {
                return await Task.FromResult(new Result<List<Grant>>() {data= _manager.Select()
                    .Where(x => x.RoleId == id)
                    .Select(p=>new Grant(p))
                    .ToList()
                });
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new Result<List<Grant>>() { code = 500, msg = ex.Message});
            }
        }

        public async Task<Result<List<Auth>>> GetAllAuth()
        {
            try
            {
                return await Task.FromResult(new Result<List<Auth>>()
                {
                    data = _auth.Select()
                    .Select(p => new Auth(p))
                    .ToList()
                });
            }
            catch(Exception ex)
            {
                return await Task.FromResult(new Result<List<Auth>>() { code = 500, msg = ex.Message });
            }
        }

        public async Task<Result<List<Role>>> GetAllRole()
        {
            try
            {
                return await Task.FromResult(new Result<List<Role>>()
                {
                    data = _role.Select()
                    .Select(p => new Role(p))
                    .ToList()
                });
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new Result<List<Role>>() { code = 500, msg = ex.Message });
            }
        }

        public async Task<Result<bool>> DeleteRole(int id)
        {
            try
            {
                var r = await _role.FindAsync(id);
                if (r == null)
                {
                    return await Task.FromResult(new Result<bool>()
                    {
                        code = 404,
                        msg = "内容不存在"
                    });
                }
                else
                {
                    await _role.DeleteAsync(id);
                    return await Task.FromResult(new Result<bool>(){data = true});
                }
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new Result<bool>()
                {
                    code = 500,
                    msg = ex.Message
                });
            }
        }

        public async Task<Result<bool>> ChangeRole(GrantModel model)
        {
            try
            {
                await _db.TbGrants.Where(p=>p.RoleId == model.Id).DeleteFromQueryAsync();
                await _db.BulkSaveChangesAsync();
                await _manager.AppendRange(model.Items.Select(p=>new TbGrant()
                {
                    AuthId = p,
                    RoleId = model.Id,
                }).ToList());
                return await Task.FromResult(new Result<bool>()
                {
                    data = true
                });
            }
            catch(Exception ex)
            {
                return await Task.FromResult(new Result<bool>()
                {
                    code = 500,
                    msg = ex.Message
                });
            }
        }

        public async Task<Result<Role>> AddRole(string name)
        {
            try
            {
                var r = new TbRole()
                {
                    RoleName = name,
                };
                await _role.AppendAsync(r);
                return await Task.FromResult(new Result<Role>() { data = new Role(r) });
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new Result<Role>()
                {
                    code = 500,
                    msg = ex.Message
                });
            }
        }
    }
}
