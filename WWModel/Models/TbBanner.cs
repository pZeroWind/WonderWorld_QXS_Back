using System;
using System.Collections.Generic;

namespace WWModel.Models
{
    public partial class TbBanner
    {
        public int Id { get; set; }
        public string? BookId { get; set; }
        public string? ImgUrl { get; set; }

        public virtual TbBook? Book { get; set; }
    }
}
