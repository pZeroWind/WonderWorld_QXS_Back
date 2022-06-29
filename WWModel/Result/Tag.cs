using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WWModel.Models;

namespace WWModel.Result
{
    public class Tag
    {
        public Tag(TbTag tag)
        {
            this.Name = tag.Name;
            this.Id = tag.Id;
        }
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
