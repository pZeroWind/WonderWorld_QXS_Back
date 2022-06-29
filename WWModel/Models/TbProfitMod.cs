using System;
using System.Collections.Generic;

namespace WWModel.Models
{
    public partial class TbProfitMod
    {
        public TbProfitMod()
        {
            TbProfits = new HashSet<TbProfit>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<TbProfit> TbProfits { get; set; }
    }
}
