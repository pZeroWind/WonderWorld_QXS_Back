using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WWModel.Models;

namespace WWModel.Result
{
    public class Grant
    {
        public Grant(TbGrant grant)
        {
            this.Id = grant.Id;
            this.AuthId = grant.AuthId;
            this.RoleId = grant.RoleId;
        }
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int AuthId { get; set; }
    }
}
