using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WWModel.Models;

namespace WWModel.Result
{
    public class Chapter
    {
        public Chapter(TbChapter chapter)
        {
            Id = chapter.Id;
            Title = chapter.Title;
            Content = chapter.Content;
            ChargeState = chapter.ChargeState;
            UpdateTime = chapter.UpdateTime;
        }

        public Chapter() { }

        public int? Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public bool? ChargeState { get; set; }
        public long? UpdateTime { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();



    }

    public class ChapterDetails
    {
        public ChapterDetails(TbChapter chapter, string? forward, string? next)
        {
            Id = chapter.Id;
            Title = chapter.Title;
            Content = chapter.Content;
            ChargeState = chapter.ChargeState;
            UpdateTime = chapter.UpdateTime;
            Pass = chapter.Pass;
            this.forward = forward;
            this.next = next;
        }

        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? forward { get; set; }

        public string? next { get; set; }

        public bool? Pass { get; set; }

        public bool? ChargeState { get; set; }
        public long? UpdateTime { get; set; }

    }
}
