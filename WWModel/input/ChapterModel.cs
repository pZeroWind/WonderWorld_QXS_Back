using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WWModel.input
{
    public class ChapterModel
    {

        public int? Id { get; set; }

        [Required(ErrorMessage = "章节名必须填写")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "内容必须填写")]
        public string? Content { get; set; }
        public bool? ChargeState { get; set; }

        [Required(ErrorMessage = "未指定上传卷")]
        public int ScrollId { get; set; }

        
        public bool Pass { get; set; } = false;
        public long? UpdateTime { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }
}
