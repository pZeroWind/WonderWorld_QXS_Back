using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WWModel.input
{
    public class LoginModel
    {
        [Required(ErrorMessage ="账号不得为空")]
        public string? Account { get; set; }

        [Required(ErrorMessage ="密码不得为空")]
        public string? Password { get; set; }
    }
}
