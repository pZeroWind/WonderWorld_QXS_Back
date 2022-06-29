using System;
using System.Collections.Generic;

namespace WWModel.Models
{
    public partial class TbBookshelf
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public string? BookId { get; set; }
        public int? ChapterId { get; set; }

        public virtual TbBook? Book { get; set; }
        public virtual TbChapter? Chapter { get; set; }
        public virtual TbUser User { get; set; } = null!;
    }
}
