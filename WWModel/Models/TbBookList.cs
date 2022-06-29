using System;
using System.Collections.Generic;

namespace WWModel.Models
{
    public partial class TbBookList
    {
        public TbBookList()
        {
            TbListDetails = new HashSet<TbListDetail>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public long? CreateTime { get; set; }
        public string? UserId { get; set; }

        public virtual TbUser? User { get; set; }
        public virtual ICollection<TbListDetail> TbListDetails { get; set; }
    }
}
