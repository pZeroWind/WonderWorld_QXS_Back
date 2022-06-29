using System;
using System.Collections.Generic;

namespace WWModel.Models
{
    public partial class TbReportState
    {
        public TbReportState()
        {
            TbReportBooks = new HashSet<TbReportBook>();
            TbReportCommentBooks = new HashSet<TbReportCommentBook>();
            TbReportCommentChapters = new HashSet<TbReportCommentChapter>();
            TbReportSubCommentBooks = new HashSet<TbReportSubCommentBook>();
            TbReportSubCommentChapters = new HashSet<TbReportSubCommentChapter>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<TbReportBook> TbReportBooks { get; set; }
        public virtual ICollection<TbReportCommentBook> TbReportCommentBooks { get; set; }
        public virtual ICollection<TbReportCommentChapter> TbReportCommentChapters { get; set; }
        public virtual ICollection<TbReportSubCommentBook> TbReportSubCommentBooks { get; set; }
        public virtual ICollection<TbReportSubCommentChapter> TbReportSubCommentChapters { get; set; }
    }
}
