using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WWModel.Models;

namespace WWModel.Result
{
    public class Auth
    {
        public Auth(TbAuth auth)
        {
            Id = auth.Id;
            Name = auth.Name;
            Remark = auth.Remark;
        }
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Remark { get; set; }
    }
}
