using System;
using System.Collections.Generic;

namespace WWModel.Models
{
    public partial class TbRole
    {
        public TbRole()
        {
            TbGrants = new HashSet<TbGrant>();
            TbUsers = new HashSet<TbUser>();
        }

        public int Id { get; set; }
        public string? RoleName { get; set; }

        public virtual ICollection<TbGrant> TbGrants { get; set; }
        public virtual ICollection<TbUser> TbUsers { get; set; }
    }
}
