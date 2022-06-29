using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WWModel.input
{
    public class GrantModel
    {
        public int Id { get; set; }

        [Required]
        public List<int>? Items { get; set; }
    }
}
