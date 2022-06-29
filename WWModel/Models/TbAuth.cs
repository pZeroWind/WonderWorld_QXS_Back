using System;
using System.Collections.Generic;

namespace WWModel.Models
{
    public partial class TbAuth
    {
        public TbAuth()
        {
            TbGrants = new HashSet<TbGrant>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Remark { get; set; }
        public string? Action { get; set; }

        public virtual ICollection<TbGrant> TbGrants { get; set; }
    }
}
