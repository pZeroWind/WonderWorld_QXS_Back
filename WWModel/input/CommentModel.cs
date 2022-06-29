using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WWModel.input
{
    public class CommentModel
    {
        public string? UserId { get; set; }
        public string? BookId { get; set; }
        public int? ChapterId { get; set; }

        [Required(ErrorMessage = "评论内容不得为空")]
        public string? Content { get; set; }
        public long? Time { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        /// <summary>
        /// 段落数
        /// </summary>
        public int? p { get; set; }
        
        /// <summary>
        /// 父评论id
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// 对方账号
        /// </summary>
        public string? OtherId { get; set; }
    }
}
