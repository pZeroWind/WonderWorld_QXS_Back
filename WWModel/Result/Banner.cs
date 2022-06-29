using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WWModel.Models;

namespace WWModel.Result
{
    public class Banner
    {
        public Banner() { }

        public Banner(TbBanner b)
        {
            Id = b.Id;
            Title = b.Book!.Title;
            BookId = b.BookId;
            ImgUrl = b.ImgUrl;
        }
        public int? Id { get; set; }
        public string? BookId { get; set; }
        public string? Title { get; set; }
        public string? ImgUrl { get; set; }
    }
}
