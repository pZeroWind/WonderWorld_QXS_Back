using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WWModel.input
{
    public class BookModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "标题不得为空")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "简介不得为空")]
        public string? Intro { get; set; }

        [Required(ErrorMessage ="至少包含一个标签")]
        public List<string>? Tags { get; set; }

        [Required(ErrorMessage ="必须选择一个分区")]
        public int? TypeId { get; set; }

        public string? Cover { get; set; }
    }
}
