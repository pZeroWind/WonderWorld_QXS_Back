using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WWModel.Models;

namespace WWModel.Result
{
    public class States
    {
        public States(TbState state)
        {
            Id = state.Id;
            Name = state.Name;
        }
        public int? Id { get; set; }

        public string? Name { get; set; }
    }
}
