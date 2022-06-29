using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WWModel.Models;

namespace WWModel.Result
{
    public class TypeData
    {
        public TypeData(TbType tb)
        {
            this.Id = tb.Id;
            this.Name = tb.Name;
            this.Remark = tb.Remark;
        }
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Remark { get; set; }
    }
}
