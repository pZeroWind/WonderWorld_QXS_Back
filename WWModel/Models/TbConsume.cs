using System;
using System.Collections.Generic;

namespace WWModel.Models
{
    public partial class TbConsume
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? BookId { get; set; }
        public int? ConsumeModeId { get; set; }
        public int? Num { get; set; }
        public long? ConsumeTime { get; set; }

        public int? ChapterId { get; set; }

        public virtual TbBook? Book { get; set; }
        public virtual TbConsumeMode? ConsumeMode { get; set; }
        public virtual TbUser? User { get; set; }
    }
}
