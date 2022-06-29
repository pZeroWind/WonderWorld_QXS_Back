using System;
using System.Collections.Generic;

namespace WWModel.Models
{
    public partial class TbFollow
    {
        public string UserId { get; set; } = null!;
        public string FollowUid { get; set; } = null!;
        public long? FollowTime { get; set; }

        public virtual TbUser FollowU { get; set; } = null!;
        public virtual TbUser User { get; set; } = null!;
    }
}
