using System;
using System.Collections.Generic;

namespace WWModel.Models
{
    public partial class TbLogin
    {
        public int Id { get; set; }
        public string? Ip { get; set; }
        public string? Address { get; set; }
        public string? UserId { get; set; }

        public virtual TbUser? User { get; set; }
    }
}
