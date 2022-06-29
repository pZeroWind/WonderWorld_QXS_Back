using System;
using System.Collections.Generic;

namespace WWModel.Models
{
    public partial class TbReportBook
    {
        public int Id { get; set; }
        public int? ChapterId { get; set; }
        public string? Details { get; set; }
        public int? State { get; set; }
        public string? UserId { get; set; }

        public virtual TbChapter? Chapter { get; set; }
        public virtual TbReportState? StateNavigation { get; set; }
        public virtual TbUser? User { get; set; }
    }
}
