using System;
using System.Collections.Generic;

namespace WWModel.Models
{
    public partial class TbChapter
    {
        public TbChapter()
        {
            TbBookshelves = new HashSet<TbBookshelf>();
            TbCommentChapters = new HashSet<TbCommentChapter>();
            TbReportBooks = new HashSet<TbReportBook>();
        }

        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public bool? ChargeState { get; set; }
        public long? UpdateTime { get; set; }
        public int? ScrollId { get; set; }
        public bool? Pass { get; set; }

        public virtual TbScroll? Scroll { get; set; }
        public virtual ICollection<TbBookshelf> TbBookshelves { get; set; }
        public virtual ICollection<TbCommentChapter> TbCommentChapters { get; set; }
        public virtual ICollection<TbReportBook> TbReportBooks { get; set; }
    }
}
