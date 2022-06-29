using System;
using System.Collections.Generic;

namespace WWModel.Models
{
    public partial class TbProfit
    {
        public int Id { get; set; }
        public int? Num { get; set; }
        public long? Time { get; set; }
        public int? ProfitModId { get; set; }
        public string? UserId { get; set; }
        public string? BookId { get; set; }

        public virtual TbBook? Book { get; set; }
        public virtual TbProfitMod? ProfitMod { get; set; }
        public virtual TbUser? User { get; set; }
    }
}
