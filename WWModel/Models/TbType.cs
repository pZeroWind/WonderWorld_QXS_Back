using System;
using System.Collections.Generic;

namespace WWModel.Models
{
    public partial class TbType
    {
        public TbType()
        {
            TbBooks = new HashSet<TbBook>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Remark { get; set; }

        public virtual ICollection<TbBook> TbBooks { get; set; }
    }
}
