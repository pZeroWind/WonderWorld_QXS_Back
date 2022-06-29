using System;
using System.Collections.Generic;

namespace WWModel.Models
{
    public partial class TbUser
    {
        public TbUser()
        {
            TbBookLists = new HashSet<TbBookList>();
            TbBooks = new HashSet<TbBook>();
            TbBookshelves = new HashSet<TbBookshelf>();
            TbCashOuts = new HashSet<TbCashOut>();
            TbCommentBooks = new HashSet<TbCommentBook>();
            TbCommentChapters = new HashSet<TbCommentChapter>();
            TbConsumes = new HashSet<TbConsume>();
            TbFollowFollowUs = new HashSet<TbFollow>();
            TbFollowUsers = new HashSet<TbFollow>();
            TbInfos = new HashSet<TbInfo>();
            TbItems = new HashSet<TbItem>();
            TbLogins = new HashSet<TbLogin>();
            TbProfits = new HashSet<TbProfit>();
            TbRecharges = new HashSet<TbRecharge>();
            TbReportBooks = new HashSet<TbReportBook>();
            TbReportCommentBooks = new HashSet<TbReportCommentBook>();
            TbReportCommentChapters = new HashSet<TbReportCommentChapter>();
            TbReportSubCommentBooks = new HashSet<TbReportSubCommentBook>();
            TbReportSubCommentChapters = new HashSet<TbReportSubCommentChapter>();
            TbSalts = new HashSet<TbSalt>();
            TbSubCommentBookOthers = new HashSet<TbSubCommentBook>();
            TbSubCommentBookUsers = new HashSet<TbSubCommentBook>();
            TbSubCommentChapterOthers = new HashSet<TbSubCommentChapter>();
            TbSubCommentChapterUsers = new HashSet<TbSubCommentChapter>();
            TbThumbsUpBooks = new HashSet<TbThumbsUpBook>();
            TbThumbsUpChapters = new HashSet<TbThumbsUpChapter>();
            TbThumbsUpSubBooks = new HashSet<TbThumbsUpSubBook>();
            TbThumbsUpSubChapters = new HashSet<TbThumbsUpSubChapter>();
            TbUserData = new HashSet<TbUserData>();
        }

        public string Id { get; set; } = null!;
        public string? Password { get; set; }
        public bool? BanState { get; set; }
        public int? RoleId { get; set; }

        public virtual TbRole? Role { get; set; }
        public virtual ICollection<TbBookList> TbBookLists { get; set; }
        public virtual ICollection<TbBook> TbBooks { get; set; }
        public virtual ICollection<TbBookshelf> TbBookshelves { get; set; }
        public virtual ICollection<TbCashOut> TbCashOuts { get; set; }
        public virtual ICollection<TbCommentBook> TbCommentBooks { get; set; }
        public virtual ICollection<TbCommentChapter> TbCommentChapters { get; set; }
        public virtual ICollection<TbConsume> TbConsumes { get; set; }
        public virtual ICollection<TbFollow> TbFollowFollowUs { get; set; }
        public virtual ICollection<TbFollow> TbFollowUsers { get; set; }
        public virtual ICollection<TbInfo> TbInfos { get; set; }
        public virtual ICollection<TbItem> TbItems { get; set; }
        public virtual ICollection<TbLogin> TbLogins { get; set; }
        public virtual ICollection<TbProfit> TbProfits { get; set; }
        public virtual ICollection<TbRecharge> TbRecharges { get; set; }
        public virtual ICollection<TbReportBook> TbReportBooks { get; set; }
        public virtual ICollection<TbReportCommentBook> TbReportCommentBooks { get; set; }
        public virtual ICollection<TbReportCommentChapter> TbReportCommentChapters { get; set; }
        public virtual ICollection<TbReportSubCommentBook> TbReportSubCommentBooks { get; set; }
        public virtual ICollection<TbReportSubCommentChapter> TbReportSubCommentChapters { get; set; }
        public virtual ICollection<TbSalt> TbSalts { get; set; }
        public virtual ICollection<TbSubCommentBook> TbSubCommentBookOthers { get; set; }
        public virtual ICollection<TbSubCommentBook> TbSubCommentBookUsers { get; set; }
        public virtual ICollection<TbSubCommentChapter> TbSubCommentChapterOthers { get; set; }
        public virtual ICollection<TbSubCommentChapter> TbSubCommentChapterUsers { get; set; }
        public virtual ICollection<TbThumbsUpBook> TbThumbsUpBooks { get; set; }
        public virtual ICollection<TbThumbsUpChapter> TbThumbsUpChapters { get; set; }
        public virtual ICollection<TbThumbsUpSubBook> TbThumbsUpSubBooks { get; set; }
        public virtual ICollection<TbThumbsUpSubChapter> TbThumbsUpSubChapters { get; set; }
        public virtual ICollection<TbUserData> TbUserData { get; set; }
    }
}
