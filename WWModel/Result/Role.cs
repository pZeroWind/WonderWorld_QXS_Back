using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WWModel.Models;

namespace WWModel.Result
{
    public class Role
    {
        public Role(TbRole role)
        {
            this.Id = role.Id;
            this.Name = role.RoleName;
        }
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
