using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WWModel.Result
{
    public class Item
    {
        public int? TiketNum { get; set; }
        public int? BladeNum { get; set; }
        public int? CoinNum { get; set; }
    }

    public class AllItem
    {
        public int Id { get; set; }
        public int? TiketNum { get; set; }
        public int? BladeNum { get; set; }
        public int? CoinNum { get; set; }
        public decimal? Money { get; set; }
    }
}
