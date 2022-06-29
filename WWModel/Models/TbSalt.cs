using System;
using System.Collections.Generic;

namespace WWModel.Models
{
    public partial class TbSalt
    {
        public int Id { get; set; }
        public string? Salt { get; set; }
        public string? UserId { get; set; }

        public virtual TbUser? User { get; set; }
    }
}
