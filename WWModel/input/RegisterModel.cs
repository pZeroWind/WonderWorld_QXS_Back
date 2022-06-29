using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WWModel.Models;

namespace WWModel.input
{
    public class RegisterModel
    {
        [Required(ErrorMessage ="密码不得为空")]
        [RegularExpression(@"(?=^.{8,16}$)(?=.*\d)(?=.*[A-Z])(?=.*[a-z])(?!.*\s).*$", ErrorMessage ="密码必须包含一个大写字母和一个小写字母，且为8~16位")]
        public string? Password { get; set; }

        [Required(ErrorMessage ="账号不得为空")]
        public string? Account { get; set; }

        public string? NickName { get; set; } = "未设置";
        public long? Birthday { get; set; } = 1012752000000;
        public string? ImgUrl { get; set; } = "/img/BraveDragon.png";
        public bool? Gender { get; set; } = true;

        [Required(ErrorMessage ="邮箱不得为空")]
        [RegularExpression(@"^(\w)+(\.\w)*@(\w)+((\.\w+)+)$",ErrorMessage ="邮箱格式不正确")]
        public string? Email { get; set; }

        [Required]
        public string? Code { get; set; }

        [Required(ErrorMessage ="联系电话不得为空")]
        [RegularExpression(@"^1[34578]\d{9}$", ErrorMessage ="联系电话格式不正确")]
        public string? Tel { get; set; }

        public long? RegisterTime { get; set; } = DateTimeOffset.Now.ToUnixTimeMilliseconds();


    }
}
