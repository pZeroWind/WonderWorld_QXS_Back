using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WWModel.Models;

namespace WWModel.Result
{
    /// <summary>
    /// 评论-返回类型
    /// </summary>
    public class Comments
    {
        public Comments(TbCommentBook comments)
        {
            Id = comments.Id;
            UserId = comments.UserId;
            Content = comments.Content;
            Time = comments.Time;
            tumbUp = comments.TbThumbsUpBooks.Where(p=>(bool)p.Up!).Count();
            tumbDown = comments.TbThumbsUpBooks.Where(p => !(bool)p.Up!).Count();
        }

        public Comments(TbCommentChapter comments)
        {
            Id = comments.Id;
            UserId = comments.UserId;
            Content = comments.Content;
            Time = comments.Time;
            tumbUp = comments.TbThumbsUpChapters.Where(p => (bool)p.Up!).Count();
            tumbDown = comments.TbThumbsUpChapters.Where(p => !(bool)p.Up!).Count();
        }

        public Comments(TbSubCommentBook comments)
        {
            Id = comments.Id;
            UserId = comments.UserId;
            OtherUserId = comments.OtherId;
            Content = comments.Content;
            Time = comments.Time;
            tumbUp = comments.TbThumbsUpSubBooks.Where(p => (bool)p.Up!).Count();
            tumbDown = comments.TbThumbsUpSubBooks.Where(p => !(bool)p.Up!).Count();
        }

        public Comments(TbSubCommentChapter comments)
        {
            Id = comments.Id;
            UserId = comments.UserId;
            OtherUserId = comments.OtherId;
            Content = comments.Content;
            Time = comments.Time;
            tumbUp = comments.TbThumbsUpSubChapters.Where(p => (bool)p.Up!).Count();
            tumbDown = comments.TbThumbsUpSubChapters.Where(p => !(bool)p.Up!).Count();
        }

        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? ImgUrl { get; set; }
        public string? OtherUserId { get; set; }
        public string? OtherName { get; set; }
        public string? BookId { get; set; }
        public int? ChapterId { get; set; }

        public int? ScrollId { get; set; }

        public string? BookName { get; set; }
        public string? Content { get; set; }
        public int? tumbUp { get; set; } = 0;
        public int? tumbDown { get; set; } = 0; 
        public long? Time { get; set; }
        public bool? thumb { get; set; } = null;
    }
}
