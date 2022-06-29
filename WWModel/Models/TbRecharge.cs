using System;
using System.Collections.Generic;

namespace WWModel.Models
{
    public partial class TbRecharge
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public decimal? Money { get; set; }
        public int? CoinNum { get; set; }
        public long? RechargeTime { get; set; }

        public virtual TbUser? User { get; set; }
    }
}
