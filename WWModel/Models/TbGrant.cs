using System;
using System.Collections.Generic;

namespace WWModel.Models
{
    public partial class TbGrant
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int AuthId { get; set; }

        public virtual TbAuth Auth { get; set; } = null!;
        public virtual TbRole Role { get; set; } = null!;
    }
}
