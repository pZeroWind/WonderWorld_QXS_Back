using System;
using System.Collections.Generic;

namespace WWModel.Models
{
    public partial class TbBook
    {
        public TbBook()
        {
            TbBanners = new HashSet<TbBanner>();
            TbBookshelves = new HashSet<TbBookshelf>();
            TbCommentBooks = new HashSet<TbCommentBook>();
            TbConsumes = new HashSet<TbConsume>();
            TbListDetails = new HashSet<TbListDetail>();
            TbProfits = new HashSet<TbProfit>();
            TbScrolls = new HashSet<TbScroll>();
            TbTags = new HashSet<TbTag>();
        }

        public string Id { get; set; } = null!;
        public string? Title { get; set; }
        public string? Intro { get; set; }
        public long? ClickNum { get; set; }
        public string? Cover { get; set; }
        public int? StateId { get; set; }
        public string? UserId { get; set; }
        public int? TypeId { get; set; }
        public long? CreateTime { get; set; }
        public long? ShelfTime { get; set; }
        public long? UpdateTime { get; set; }

        public virtual TbState? State { get; set; }
        public virtual TbType? Type { get; set; }
        public virtual TbUser? User { get; set; }
        public virtual ICollection<TbBanner> TbBanners { get; set; }
        public virtual ICollection<TbBookshelf> TbBookshelves { get; set; }
        public virtual ICollection<TbCommentBook> TbCommentBooks { get; set; }
        public virtual ICollection<TbConsume> TbConsumes { get; set; }
        public virtual ICollection<TbListDetail> TbListDetails { get; set; }
        public virtual ICollection<TbProfit> TbProfits { get; set; }
        public virtual ICollection<TbScroll> TbScrolls { get; set; }
        public virtual ICollection<TbTag> TbTags { get; set; }
    }
}
