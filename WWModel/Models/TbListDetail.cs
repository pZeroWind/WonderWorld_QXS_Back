using System;
using System.Collections.Generic;

namespace WWModel.Models
{
    public partial class TbListDetail
    {
        public int Id { get; set; }
        public int? ListId { get; set; }
        public string? BookId { get; set; }

        public virtual TbBook? Book { get; set; }
        public virtual TbBookList? List { get; set; }
    }
}
