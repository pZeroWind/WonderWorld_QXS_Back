using System;
using System.Collections.Generic;

namespace WWModel.Models
{
    public partial class TbInfo
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? Content { get; set; }
        public long? SendTime { get; set; }
        public ulong? Readed { get; set; }

        public virtual TbUser? User { get; set; }
    }
}
