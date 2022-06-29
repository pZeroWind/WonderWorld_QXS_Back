using System;
using System.Collections.Generic;

namespace WWModel.Models
{
    public partial class TbThumbsUpSubBook
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public int? CommentId { get; set; }
        public bool? Up { get; set; }

        public virtual TbSubCommentBook? Comment { get; set; }
        public virtual TbUser? User { get; set; }
    }
}
