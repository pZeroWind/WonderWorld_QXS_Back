using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WWModel.Models;

namespace WWModel.Result
{
    public class Scroll
    {
        public Scroll(TbScroll scroll)
        {
            Id = scroll.Id;
            Name = scroll.Name;
        }

        public int Id { get; set; }
        public string? Name { get; set; }

    }
}
