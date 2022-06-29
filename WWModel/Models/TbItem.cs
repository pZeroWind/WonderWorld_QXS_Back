using System;
using System.Collections.Generic;

namespace WWModel.Models
{
    public partial class TbItem
    {
        public int Id { get; set; }
        public int? TiketNum { get; set; }
        public int? BladeNum { get; set; }
        public int? CoinNum { get; set; }
        public decimal? Money { get; set; }
        public string? UserId { get; set; }

        public virtual TbUser? User { get; set; }
    }
}
