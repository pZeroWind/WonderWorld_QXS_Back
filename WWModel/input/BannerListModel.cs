using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WWModel.Result;

namespace WWModel.input
{
    public class BannerListModel
    {
        [Required(ErrorMessage = "所保存的轮播图列表为空")]
        [MinLength(1,ErrorMessage = "所保存的轮播图列表为空")]
        public List<Banner>? data { get; set; }
    }
}
