using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WWModel.input
{
    public class PwdChangeModel
    {
        public string? Account { get; set; }

        public string? oldPassword { get; set; }

        [Required(ErrorMessage = "密码不得为空")]
        [RegularExpression(@"(?=^.{8,16}$)(?=.*\d)(?=.*[A-Z])(?=.*[a-z])(?!.*\s).*$", ErrorMessage = "密码必须包含一个大写字母和一个小写字母，且为8~16位")]
        public string? Password { get; set; }

        public string? Code { get; set; }
    }
}
