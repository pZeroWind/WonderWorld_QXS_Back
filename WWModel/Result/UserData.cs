using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WWModel.Models;

namespace WWModel.Result
{
    public class UserData
    {
        public UserData() { }
        public UserData(TbUserData u)
        {
            this.Id = u.Id;
            this.Account = u.Account;
            this.IdCard=u.IdCard;
            this.BankCard = u.BankCard;
            this.Birthday = u.Birthday;
            this.Email = u.Email;
            this.Tel = u.Tel;
            this.RegisterTime = u.RegisterTime;
            this.ImgUrl = u.ImgUrl;
            this.NickName = u.NickName;
            this.Gender = u.Gender;
        }

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

        public int? Coin { get; set; } = 0;

        public int? Blade { get; set; } = 0;

        public int? Tiket { get; set; } = 0;

    }

    public class UserDataPlus
    {
        public UserDataPlus() { }
        public UserDataPlus(TbUserData u)
        {
            this.Id = u.Id;
            this.Account = u.Account;
            this.IdCard = u.IdCard;
            this.BankCard = u.BankCard;
            this.Birthday = u.Birthday;
            this.Email = u.Email;
            this.Tel = u.Tel;
            this.RegisterTime = u.RegisterTime;
            this.ImgUrl = u.ImgUrl;
            this.NickName = u.NickName;
            this.Gender = u.Gender;
        }

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

        public string? Role { get; set; }

    }
}
