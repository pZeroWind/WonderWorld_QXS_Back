using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WWModel.Models;

namespace WWModel.Result
{
    public class ChapterData
    {
        public ChapterData(TbChapter chapter)
        {
            Id = chapter.Id;
            Title = chapter.Title;
            ChargeState = chapter.ChargeState;
            UpdateTime = chapter.UpdateTime;
        }

        public int Id { get; set; }
        public string? Title { get; set; }
        public bool? ChargeState { get; set; }
        public long? UpdateTime { get; set; }
    }
}
