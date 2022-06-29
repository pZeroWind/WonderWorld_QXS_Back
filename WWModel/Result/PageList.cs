using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WWModel.Result
{
    public class PageList<T>
    {
        public PageList(IEnumerable<T> list)
        {
            List.AddRange(list);
        }

        public List<T> List { get; set; } = new List<T>();

        public int limt { get; set; }

        public int size { get; set; }

        public int total { get; set; }

        public int page { get; set; }
    }
}
