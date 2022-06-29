using System;
using System.Collections.Generic;

namespace WWModel.Models
{
    public partial class TbSubCommentBook
    {
        public TbSubCommentBook()
        {
            InverseReply = new HashSet<TbSubCommentBook>();
            TbReportSubCommentBooks = new HashSet<TbReportSubCommentBook>();
            TbThumbsUpSubBooks = new HashSet<TbThumbsUpSubBook>();
        }

        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? OtherId { get; set; }
        public int? ParentId { get; set; }
        public string? Content { get; set; }
        public long? Time { get; set; }
        public int? ReplyId { get; set; }

        public virtual TbUser? Other { get; set; }
        public virtual TbCommentBook? Parent { get; set; }
        public virtual TbSubCommentBook? Reply { get; set; }
        public virtual TbUser? User { get; set; }
        public virtual ICollection<TbSubCommentBook> InverseReply { get; set; }
        public virtual ICollection<TbReportSubCommentBook> TbReportSubCommentBooks { get; set; }
        public virtual ICollection<TbThumbsUpSubBook> TbThumbsUpSubBooks { get; set; }
    }
}
