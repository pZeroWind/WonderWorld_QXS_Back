using System;
using System.Collections.Generic;

namespace WWModel.Models
{
    public partial class TbCommentBook
    {
        public TbCommentBook()
        {
            TbReportCommentBooks = new HashSet<TbReportCommentBook>();
            TbSubCommentBooks = new HashSet<TbSubCommentBook>();
            TbThumbsUpBooks = new HashSet<TbThumbsUpBook>();
        }

        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? BookId { get; set; }
        public string? Content { get; set; }
        public long? Time { get; set; }

        public virtual TbBook? Book { get; set; }
        public virtual TbUser? User { get; set; }
        public virtual ICollection<TbReportCommentBook> TbReportCommentBooks { get; set; }
        public virtual ICollection<TbSubCommentBook> TbSubCommentBooks { get; set; }
        public virtual ICollection<TbThumbsUpBook> TbThumbsUpBooks { get; set; }
    }
}
