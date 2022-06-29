﻿using System;
using System.Collections.Generic;

namespace WWModel.Models
{
    public partial class TbTag
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? BookId { get; set; }

        public virtual TbBook? Book { get; set; }
    }
}
