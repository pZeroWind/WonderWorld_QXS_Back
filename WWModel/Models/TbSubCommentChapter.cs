using System;
using System.Collections.Generic;

namespace WWModel.Models
{
    public partial class TbSubCommentChapter
    {
        public TbSubCommentChapter()
        {
            InverseReply = new HashSet<TbSubCommentChapter>();
            TbReportSubCommentChapters = new HashSet<TbReportSubCommentChapter>();
            TbThumbsUpSubChapters = new HashSet<TbThumbsUpSubChapter>();
        }

        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? OtherId { get; set; }
        public int? ParentId { get; set; }
        public string? Content { get; set; }
        public long? Time { get; set; }
        public int? ReplyId { get; set; }

        public virtual TbUser? Other { get; set; }
        public virtual TbCommentChapter? Parent { get; set; }
        public virtual TbSubCommentChapter? Reply { get; set; }
        public virtual TbUser? User { get; set; }
        public virtual ICollection<TbSubCommentChapter> InverseReply { get; set; }
        public virtual ICollection<TbReportSubCommentChapter> TbReportSubCommentChapters { get; set; }
        public virtual ICollection<TbThumbsUpSubChapter> TbThumbsUpSubChapters { get; set; }
    }
}
