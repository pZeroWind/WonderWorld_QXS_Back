using System;
using System.Collections.Generic;

namespace WWModel.Models
{
    public partial class TbThumbsUpChapter
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public int? CommentId { get; set; }
        public bool? Up { get; set; }

        public virtual TbCommentChapter? Comment { get; set; }
        public virtual TbUser? User { get; set; }
    }
}
