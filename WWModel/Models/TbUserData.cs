using System;
using System.Collections.Generic;

namespace WWModel.Models
{
    public partial class TbUserData
    {
        public int Id { get; set; }
        public string? Account { get; set; }
        public string? NickName { get; set; }
        public long? Birthday { get; set; }
        public string? ImgUrl { get; set; }
        public bool? Gender { get; set; }
        public string? Email { get; set; }
        public string? Tel { get; set; }
        public string? BankCard { get; set; }
        public string? IdCard { get; set; }
        public long? RegisterTime { get; set; }

        public virtual TbUser? AccountNavigation { get; set; }
    }
}
