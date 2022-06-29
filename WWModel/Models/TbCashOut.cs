using System;
using System.Collections.Generic;

namespace WWModel.Models
{
    public partial class TbCashOut
    {
        public int Id { get; set; }
        public decimal? Money { get; set; }
        public string? UserId { get; set; }
        public bool? Complete { get; set; }
        public long? CreateTime { get; set; }
        public long? CompleteTime { get; set; }

        public virtual TbUser? User { get; set; }
    }
}
