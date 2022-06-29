using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WWModel.Models;

namespace WWModel.Result
{
    public class Book
    {
        public Book()
        {

        }

        public Book(TbBook p)
        {
            Id = p.Id!;
            StateId = p.StateId!;
            State = p.StateId==2||p.StateId==3?"连载中":p.State!.Name;
            ShelfTime = p.ShelfTime;
            Cover = p.Cover;
            CreateTime = p.CreateTime;
            Title = p.Title;
            Type = p.Type!.Name;
            Account = p.UserId;
            ClickNum = p.ClickNum;
            SaveNum = p.TbListDetails.Count;
            Tags = p.TbTags.Select(i=>new Tag(i)).ToList();
            Intro = p.Intro;
            UpdateTime = p.UpdateTime;
            Profits = new Item
            {
                BladeNum = p.TbProfits.Where(p2 => p2.ProfitModId == 2).Sum(p2 => p2.Num),
                TiketNum = p.TbProfits.Where(p2 => p2.ProfitModId == 1).Sum(p2 => p2.Num),
                CoinNum = p.TbProfits.Where(p2 => p2.ProfitModId == 3).Sum(p2 => p2.Num)
            };
        }

        public string Id { get; set; } = null!;
        public string? Title { get; set; }
        public string? Intro { get; set; }
        public List<Tag>? Tags { get; set; }
        public long? ClickNum { get; set; }
        public int? TotalWord { get; set; }
        public int? SaveNum { get; set; }
        public Item? Profits { get; set; } 
        public string? Cover { get; set; }

        public int? StateId { get; set; }
        public string? State { get; set; }
        public string? Account { get; set; }
        public string? NickName { get; set; }
        public string? Type { get; set; }
        public long? CreateTime { get; set; }
        public long? ShelfTime { get; set; }
        public long? UpdateTime { get; set; }
        public string? NewChapter { get; set; }

        public string? History { get; set; }
    }
}
