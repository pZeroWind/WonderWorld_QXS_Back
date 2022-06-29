using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WWModel.Result
{
    public class WriterModel
    {
        public WriterData? WriterData { get; set; }

        public List<string> Days { get; set; } = new List<string>();

        public List<double> Datas { get; set; } = new List<double>();
    }
}
