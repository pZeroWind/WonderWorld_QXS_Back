using System;
using System.Collections.Generic;

namespace WWModel.Models
{
    public partial class TbState
    {
        public TbState()
        {
            TbBooks = new HashSet<TbBook>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<TbBook> TbBooks { get; set; }
    }
}
