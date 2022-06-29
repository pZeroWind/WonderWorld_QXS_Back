using System;
using System.Collections.Generic;

namespace WWModel.Models
{
    public partial class TbCommentChapter
    {
        public TbCommentChapter()
        {
            TbReportCommentChapters = new HashSet<TbReportCommentChapter>();
            TbSubCommentChapters = new HashSet<TbSubCommentChapter>();
            TbThumbsUpChapters = new HashSet<TbThumbsUpChapter>();
        }

        public int Id { get; set; }
        public string? UserId { get; set; }
        public int? ChapterId { get; set; }
        public int? Paragraph { get; set; }
        public string? Content { get; set; }
        public long? Time { get; set; }

        public virtual TbChapter? Chapter { get; set; }
        public virtual TbUser? User { get; set; }
        public virtual ICollection<TbReportCommentChapter> TbReportCommentChapters { get; set; }
        public virtual ICollection<TbSubCommentChapter> TbSubCommentChapters { get; set; }
        public virtual ICollection<TbThumbsUpChapter> TbThumbsUpChapters { get; set; }
    }
}
