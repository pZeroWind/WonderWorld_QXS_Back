using System;
using System.Collections.Generic;

namespace WWModel.Models
{
    public partial class TbReportCommentChapter
    {
        public int Id { get; set; }
        public int? CId { get; set; }
        public string? Details { get; set; }
        public int? State { get; set; }
        public string? UserId { get; set; }

        public virtual TbCommentChapter? CIdNavigation { get; set; }
        public virtual TbReportState? StateNavigation { get; set; }
        public virtual TbUser? User { get; set; }
    }
}
