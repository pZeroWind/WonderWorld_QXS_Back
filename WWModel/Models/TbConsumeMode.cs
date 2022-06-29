using System;
using System.Collections.Generic;

namespace WWModel.Models
{
    public partial class TbConsumeMode
    {
        public TbConsumeMode()
        {
            TbConsumes = new HashSet<TbConsume>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<TbConsume> TbConsumes { get; set; }
    }
}
