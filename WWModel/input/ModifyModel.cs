using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WWModel.Models;

namespace WWModel.input
{
    public class ModifyModel
    {
        [Required(ErrorMessage ="账号不得为空")]
        public string? Account { get; set; }

        public string? NickName { get; set; }
        public long? Birthday { get; set; }
        public string? ImgUrl { get; set; }
        public bool? Gender { get; set; }

        [Required(ErrorMessage ="邮箱不得为空")]
        [RegularExpression(@"^(\w)+(\.\w)*@(\w)+((\.\w+)+)$",ErrorMessage ="邮箱格式不正确")]
        public string? Email { get; set; }

        [Required(ErrorMessage ="联系电话不得为空")]
        [RegularExpression(@"^1[34578]\d{9}$", ErrorMessage ="联系电话格式不正确")]
        public string? Tel { get; set; }
    }
}
