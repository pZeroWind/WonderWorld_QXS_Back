using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WWModel.Models
{
    public partial class WWDBContext : DbContext
    {
        public WWDBContext()
        {
        }

        public WWDBContext(DbContextOptions<WWDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TbAuth> TbAuths { get; set; } = null!;
        public virtual DbSet<TbBanner> TbBanners { get; set; } = null!;
        public virtual DbSet<TbBook> TbBooks { get; set; } = null!;
        public virtual DbSet<TbBookList> TbBookLists { get; set; } = null!;
        public virtual DbSet<TbBookshelf> TbBookshelves { get; set; } = null!;
        public virtual DbSet<TbCashOut> TbCashOuts { get; set; } = null!;
        public virtual DbSet<TbChapter> TbChapters { get; set; } = null!;
        public virtual DbSet<TbCommentBook> TbCommentBooks { get; set; } = null!;
        public virtual DbSet<TbCommentChapter> TbCommentChapters { get; set; } = null!;
        public virtual DbSet<TbConsume> TbConsumes { get; set; } = null!;
        public virtual DbSet<TbConsumeMode> TbConsumeModes { get; set; } = null!;
        public virtual DbSet<TbFollow> TbFollows { get; set; } = null!;
        public virtual DbSet<TbGrant> TbGrants { get; set; } = null!;
        public virtual DbSet<TbInfo> TbInfos { get; set; } = null!;
        public virtual DbSet<TbItem> TbItems { get; set; } = null!;
        public virtual DbSet<TbListDetail> TbListDetails { get; set; } = null!;
        public virtual DbSet<TbLogin> TbLogins { get; set; } = null!;
        public virtual DbSet<TbProfit> TbProfits { get; set; } = null!;
        public virtual DbSet<TbProfitMod> TbProfitMods { get; set; } = null!;
        public virtual DbSet<TbRecharge> TbRecharges { get; set; } = null!;
        public virtual DbSet<TbReportBook> TbReportBooks { get; set; } = null!;
        public virtual DbSet<TbReportCommentBook> TbReportCommentBooks { get; set; } = null!;
        public virtual DbSet<TbReportCommentChapter> TbReportCommentChapters { get; set; } = null!;
        public virtual DbSet<TbReportState> TbReportStates { get; set; } = null!;
        public virtual DbSet<TbReportSubCommentBook> TbReportSubCommentBooks { get; set; } = null!;
        public virtual DbSet<TbReportSubCommentChapter> TbReportSubCommentChapters { get; set; } = null!;
        public virtual DbSet<TbRole> TbRoles { get; set; } = null!;
        public virtual DbSet<TbSalt> TbSalts { get; set; } = null!;
        public virtual DbSet<TbScroll> TbScrolls { get; set; } = null!;
        public virtual DbSet<TbState> TbStates { get; set; } = null!;
        public virtual DbSet<TbSubCommentBook> TbSubCommentBooks { get; set; } = null!;
        public virtual DbSet<TbSubCommentChapter> TbSubCommentChapters { get; set; } = null!;
        public virtual DbSet<TbTag> TbTags { get; set; } = null!;
        public virtual DbSet<TbThumbsUpBook> TbThumbsUpBooks { get; set; } = null!;
        public virtual DbSet<TbThumbsUpChapter> TbThumbsUpChapters { get; set; } = null!;
        public virtual DbSet<TbThumbsUpSubBook> TbThumbsUpSubBooks { get; set; } = null!;
        public virtual DbSet<TbThumbsUpSubChapter> TbThumbsUpSubChapters { get; set; } = null!;
        public virtual DbSet<TbType> TbTypes { get; set; } = null!;
        public virtual DbSet<TbUser> TbUsers { get; set; } = null!;
        public virtual DbSet<TbUserData> TbUserData { get; set; } = null!;
        public virtual DbSet<TbWord> TbWords { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=121.40.199.38;database=WWDB;port=3306;user=pzw;password=Pu.123456", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.29-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<TbAuth>(entity =>
            {
                entity.ToTable("tb_auth");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Action)
                    .HasMaxLength(100)
                    .HasColumnName("action");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.Remark)
                    .HasMaxLength(100)
                    .HasColumnName("remark");
            });

            modelBuilder.Entity<TbBanner>(entity =>
            {
                entity.ToTable("tb_banner");

                entity.HasIndex(e => e.BookId, "tb_banner_FK");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BookId)
                    .HasMaxLength(100)
                    .HasColumnName("bookId");

                entity.Property(e => e.ImgUrl)
                    .HasMaxLength(255)
                    .HasColumnName("imgUrl");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.TbBanners)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("tb_banner_FK");
            });

            modelBuilder.Entity<TbBook>(entity =>
            {
                entity.ToTable("tb_book");

                entity.HasIndex(e => e.TypeId, "tb_book_FK");

                entity.HasIndex(e => e.UserId, "tb_book_FK_1");

                entity.HasIndex(e => e.StateId, "tb_book_FK_2");

                entity.Property(e => e.Id)
                    .HasMaxLength(100)
                    .HasColumnName("id");

                entity.Property(e => e.ClickNum)
                    .HasColumnName("clickNum")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Cover)
                    .HasMaxLength(100)
                    .HasColumnName("cover");

                entity.Property(e => e.CreateTime).HasColumnName("createTime");

                entity.Property(e => e.Intro)
                    .HasColumnType("text")
                    .HasColumnName("intro");

                entity.Property(e => e.ShelfTime).HasColumnName("shelfTime");

                entity.Property(e => e.StateId)
                    .HasColumnName("stateId")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .HasColumnName("title");

                entity.Property(e => e.TypeId).HasColumnName("typeId");

                entity.Property(e => e.UpdateTime).HasColumnName("updateTime");

                entity.Property(e => e.UserId)
                    .HasMaxLength(100)
                    .HasColumnName("userId");

                entity.HasOne(d => d.State)
                    .WithMany(p => p.TbBooks)
                    .HasForeignKey(d => d.StateId)
                    .HasConstraintName("tb_book_FK_2");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.TbBooks)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("tb_book_FK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TbBooks)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("tb_book_FK_1");
            });

            modelBuilder.Entity<TbBookList>(entity =>
            {
                entity.ToTable("tb_bookList");

                entity.HasIndex(e => e.UserId, "userId");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateTime).HasColumnName("createTime");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TbBookLists)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("tb_bookList_ibfk_1");
            });

            modelBuilder.Entity<TbBookshelf>(entity =>
            {
                entity.ToTable("tb_bookshelf");

                entity.HasIndex(e => e.BookId, "tb_bookshelf_FK");

                entity.HasIndex(e => e.UserId, "tb_bookshelf_FK_1");

                entity.HasIndex(e => e.ChapterId, "tb_bookshelf_FK_2");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BookId).HasColumnName("bookId");

                entity.Property(e => e.ChapterId).HasColumnName("chapterId");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.TbBookshelves)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("tb_bookshelf_FK");

                entity.HasOne(d => d.Chapter)
                    .WithMany(p => p.TbBookshelves)
                    .HasForeignKey(d => d.ChapterId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("tb_bookshelf_FK_2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TbBookshelves)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tb_bookshelf_FK_1");
            });

            modelBuilder.Entity<TbCashOut>(entity =>
            {
                entity.ToTable("tb_cashOut");

                entity.HasIndex(e => e.UserId, "userId");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Complete)
                    .HasColumnName("complete")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.CompleteTime).HasColumnName("completeTime");

                entity.Property(e => e.CreateTime).HasColumnName("createTime");

                entity.Property(e => e.Money)
                    .HasPrecision(10, 2)
                    .HasColumnName("money");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TbCashOuts)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("tb_cashOut_ibfk_1");
            });

            modelBuilder.Entity<TbChapter>(entity =>
            {
                entity.ToTable("tb_chapter");

                entity.HasIndex(e => e.ScrollId, "tb_chapter_FK");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ChargeState)
                    .HasColumnName("chargeState")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Content)
                    .HasColumnType("text")
                    .HasColumnName("content");

                entity.Property(e => e.Pass)
                    .HasColumnName("pass")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.ScrollId).HasColumnName("scrollId");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .HasColumnName("title");

                entity.Property(e => e.UpdateTime).HasColumnName("updateTime");

                entity.HasOne(d => d.Scroll)
                    .WithMany(p => p.TbChapters)
                    .HasForeignKey(d => d.ScrollId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("tb_chapter_FK");
            });

            modelBuilder.Entity<TbCommentBook>(entity =>
            {
                entity.ToTable("tb_commentBook");

                entity.HasIndex(e => e.BookId, "tb_commentBook_ibfk_2");

                entity.HasIndex(e => e.UserId, "userId");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BookId).HasColumnName("bookId");

                entity.Property(e => e.Content)
                    .HasColumnType("text")
                    .HasColumnName("content");

                entity.Property(e => e.Time).HasColumnName("time");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.TbCommentBooks)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("tb_commentBook_ibfk_2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TbCommentBooks)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("tb_commentBook_ibfk_1");
            });

            modelBuilder.Entity<TbCommentChapter>(entity =>
            {
                entity.ToTable("tb_commentChapter");

                entity.HasIndex(e => e.ChapterId, "tb_commentChapter_ibfk_2");

                entity.HasIndex(e => e.UserId, "userId");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ChapterId).HasColumnName("chapterId");

                entity.Property(e => e.Content)
                    .HasColumnType("text")
                    .HasColumnName("content");

                entity.Property(e => e.Paragraph).HasColumnName("paragraph");

                entity.Property(e => e.Time).HasColumnName("time");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.Chapter)
                    .WithMany(p => p.TbCommentChapters)
                    .HasForeignKey(d => d.ChapterId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("tb_commentChapter_ibfk_2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TbCommentChapters)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("tb_commentChapter_ibfk_1");
            });

            modelBuilder.Entity<TbConsume>(entity =>
            {
                entity.ToTable("tb_consume");

                entity.HasIndex(e => e.ConsumeModeId, "consumeModeId");

                entity.HasIndex(e => e.BookId, "tb_consume_ibfk_2");

                entity.HasIndex(e => e.UserId, "userId");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BookId).HasColumnName("bookId");

                entity.Property(e => e.ConsumeModeId).HasColumnName("consumeModeId");

                entity.Property(e => e.ConsumeTime).HasColumnName("consumeTime");

                entity.Property(e => e.Num).HasColumnName("num");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.Property(e => e.ChapterId).HasColumnName("chapterId");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.TbConsumes)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("tb_consume_ibfk_2");

                entity.HasOne(d => d.ConsumeMode)
                    .WithMany(p => p.TbConsumes)
                    .HasForeignKey(d => d.ConsumeModeId)
                    .HasConstraintName("tb_consume_ibfk_3");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TbConsumes)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("tb_consume_ibfk_1");
            });

            modelBuilder.Entity<TbConsumeMode>(entity =>
            {
                entity.ToTable("tb_consumeMode");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<TbFollow>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.FollowUid })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("tb_follow");

                entity.HasIndex(e => e.FollowUid, "followUId");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.Property(e => e.FollowUid).HasColumnName("followUId");

                entity.Property(e => e.FollowTime).HasColumnName("followTime");

                entity.HasOne(d => d.FollowU)
                    .WithMany(p => p.TbFollowFollowUs)
                    .HasForeignKey(d => d.FollowUid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tb_follow_ibfk_2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TbFollowUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tb_follow_ibfk_1");
            });

            modelBuilder.Entity<TbGrant>(entity =>
            {
                entity.ToTable("tb_grant");

                entity.HasIndex(e => e.AuthId, "tb_grant_FK");

                entity.HasIndex(e => e.RoleId, "tb_grant_FK_1");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AuthId).HasColumnName("authId");

                entity.Property(e => e.RoleId).HasColumnName("roleId");

                entity.HasOne(d => d.Auth)
                    .WithMany(p => p.TbGrants)
                    .HasForeignKey(d => d.AuthId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tb_grant_FK");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.TbGrants)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tb_grant_FK_1");
            });

            modelBuilder.Entity<TbInfo>(entity =>
            {
                entity.ToTable("tb_info");

                entity.HasIndex(e => e.UserId, "userId");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content)
                    .HasColumnType("text")
                    .HasColumnName("content");

                entity.Property(e => e.Readed)
                    .HasColumnType("bit(1)")
                    .HasColumnName("readed")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.SendTime).HasColumnName("sendTime");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TbInfos)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("tb_info_ibfk_1");
            });

            modelBuilder.Entity<TbItem>(entity =>
            {
                entity.ToTable("tb_item");

                entity.HasIndex(e => e.UserId, "userId");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BladeNum)
                    .HasColumnName("bladeNum")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.CoinNum)
                    .HasColumnName("coinNum")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Money)
                    .HasPrecision(10, 2)
                    .HasColumnName("money")
                    .HasDefaultValueSql("'0.00'");

                entity.Property(e => e.TiketNum)
                    .HasColumnName("tiketNum")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TbItems)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("tb_item_ibfk_1");
            });

            modelBuilder.Entity<TbListDetail>(entity =>
            {
                entity.ToTable("tb_listDetails");

                entity.HasIndex(e => e.BookId, "tb_listDetails_FK");

                entity.HasIndex(e => e.ListId, "tb_listDetails_FK_1");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BookId).HasColumnName("bookId");

                entity.Property(e => e.ListId).HasColumnName("listId");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.TbListDetails)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("tb_listDetails_FK");

                entity.HasOne(d => d.List)
                    .WithMany(p => p.TbListDetails)
                    .HasForeignKey(d => d.ListId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("tb_listDetails_FK_1");
            });

            modelBuilder.Entity<TbLogin>(entity =>
            {
                entity.ToTable("tb_login");

                entity.HasIndex(e => e.UserId, "userId");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .HasColumnName("address");

                entity.Property(e => e.Ip)
                    .HasMaxLength(255)
                    .HasColumnName("ip");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TbLogins)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("tb_login_ibfk_1");
            });

            modelBuilder.Entity<TbProfit>(entity =>
            {
                entity.ToTable("tb_profit");

                entity.HasIndex(e => e.ProfitModId, "profitMod");

                entity.HasIndex(e => e.BookId, "tb_profit_ibfk_3");

                entity.HasIndex(e => e.UserId, "userId");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BookId).HasColumnName("bookId");

                entity.Property(e => e.Num).HasColumnName("num");

                entity.Property(e => e.ProfitModId).HasColumnName("profitModId");

                entity.Property(e => e.Time).HasColumnName("time");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.TbProfits)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("tb_profit_ibfk_3");

                entity.HasOne(d => d.ProfitMod)
                    .WithMany(p => p.TbProfits)
                    .HasForeignKey(d => d.ProfitModId)
                    .HasConstraintName("tb_profit_ibfk_1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TbProfits)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("tb_profit_ibfk_2");
            });

            modelBuilder.Entity<TbProfitMod>(entity =>
            {
                entity.ToTable("tb_profitMod");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<TbRecharge>(entity =>
            {
                entity.ToTable("tb_recharge");

                entity.HasIndex(e => e.UserId, "userId");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CoinNum).HasColumnName("coinNum");

                entity.Property(e => e.Money)
                    .HasPrecision(10, 2)
                    .HasColumnName("money");

                entity.Property(e => e.RechargeTime).HasColumnName("rechargeTime");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TbRecharges)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("tb_recharge_ibfk_1");
            });

            modelBuilder.Entity<TbReportBook>(entity =>
            {
                entity.ToTable("tb_reportBook");

                entity.HasIndex(e => e.State, "state");

                entity.HasIndex(e => e.ChapterId, "tb_reportBook_ibfk_2");

                entity.HasIndex(e => e.UserId, "userId");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ChapterId).HasColumnName("chapterId");

                entity.Property(e => e.Details)
                    .HasColumnType("text")
                    .HasColumnName("details");

                entity.Property(e => e.State).HasColumnName("state");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.Chapter)
                    .WithMany(p => p.TbReportBooks)
                    .HasForeignKey(d => d.ChapterId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("tb_reportBook_ibfk_2");

                entity.HasOne(d => d.StateNavigation)
                    .WithMany(p => p.TbReportBooks)
                    .HasForeignKey(d => d.State)
                    .HasConstraintName("tb_reportBook_ibfk_1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TbReportBooks)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("tb_reportBook_ibfk_3");
            });

            modelBuilder.Entity<TbReportCommentBook>(entity =>
            {
                entity.ToTable("tb_reportCommentBook");

                entity.HasIndex(e => e.State, "state");

                entity.HasIndex(e => e.CId, "tb_reportCommentBook_ibfk_2");

                entity.HasIndex(e => e.UserId, "userId");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CId).HasColumnName("cId");

                entity.Property(e => e.Details)
                    .HasColumnType("text")
                    .HasColumnName("details");

                entity.Property(e => e.State).HasColumnName("state");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.CIdNavigation)
                    .WithMany(p => p.TbReportCommentBooks)
                    .HasForeignKey(d => d.CId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("tb_reportCommentBook_ibfk_2");

                entity.HasOne(d => d.StateNavigation)
                    .WithMany(p => p.TbReportCommentBooks)
                    .HasForeignKey(d => d.State)
                    .HasConstraintName("tb_reportCommentBook_ibfk_1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TbReportCommentBooks)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("tb_reportCommentBook_ibfk_3");
            });

            modelBuilder.Entity<TbReportCommentChapter>(entity =>
            {
                entity.ToTable("tb_reportCommentChapter");

                entity.HasIndex(e => e.State, "tb_reportCommentChapter_ibfk_1");

                entity.HasIndex(e => e.CId, "tb_reportCommentChapter_ibfk_2");

                entity.HasIndex(e => e.UserId, "userId");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CId).HasColumnName("cId");

                entity.Property(e => e.Details)
                    .HasColumnType("text")
                    .HasColumnName("details");

                entity.Property(e => e.State).HasColumnName("state");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.CIdNavigation)
                    .WithMany(p => p.TbReportCommentChapters)
                    .HasForeignKey(d => d.CId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("tb_reportCommentChapter_ibfk_2");

                entity.HasOne(d => d.StateNavigation)
                    .WithMany(p => p.TbReportCommentChapters)
                    .HasForeignKey(d => d.State)
                    .HasConstraintName("tb_reportCommentChapter_ibfk_1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TbReportCommentChapters)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("tb_reportCommentChapter_ibfk_3");
            });

            modelBuilder.Entity<TbReportState>(entity =>
            {
                entity.ToTable("tb_reportState");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<TbReportSubCommentBook>(entity =>
            {
                entity.ToTable("tb_reportSubCommentBook");

                entity.HasIndex(e => e.State, "state");

                entity.HasIndex(e => e.CId, "tb_reportSubCommentBook_ibfk_2");

                entity.HasIndex(e => e.UserId, "userId");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CId).HasColumnName("cId");

                entity.Property(e => e.Details)
                    .HasColumnType("text")
                    .HasColumnName("details");

                entity.Property(e => e.State).HasColumnName("state");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.CIdNavigation)
                    .WithMany(p => p.TbReportSubCommentBooks)
                    .HasForeignKey(d => d.CId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("tb_reportSubCommentBook_ibfk_2");

                entity.HasOne(d => d.StateNavigation)
                    .WithMany(p => p.TbReportSubCommentBooks)
                    .HasForeignKey(d => d.State)
                    .HasConstraintName("tb_reportSubCommentBook_ibfk_1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TbReportSubCommentBooks)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("tb_reportSubCommentBook_ibfk_3");
            });

            modelBuilder.Entity<TbReportSubCommentChapter>(entity =>
            {
                entity.ToTable("tb_reportSubCommentChapter");

                entity.HasIndex(e => e.State, "state");

                entity.HasIndex(e => e.CId, "tb_reportSubCommentChapter_ibfk_2");

                entity.HasIndex(e => e.UserId, "userId");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CId).HasColumnName("cId");

                entity.Property(e => e.Details)
                    .HasColumnType("text")
                    .HasColumnName("details");

                entity.Property(e => e.State).HasColumnName("state");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.CIdNavigation)
                    .WithMany(p => p.TbReportSubCommentChapters)
                    .HasForeignKey(d => d.CId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("tb_reportSubCommentChapter_ibfk_2");

                entity.HasOne(d => d.StateNavigation)
                    .WithMany(p => p.TbReportSubCommentChapters)
                    .HasForeignKey(d => d.State)
                    .HasConstraintName("tb_reportSubCommentChapter_ibfk_1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TbReportSubCommentChapters)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("tb_reportSubCommentChapter_ibfk_3");
            });

            modelBuilder.Entity<TbRole>(entity =>
            {
                entity.ToTable("tb_roles");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(100)
                    .HasColumnName("roleName");
            });

            modelBuilder.Entity<TbSalt>(entity =>
            {
                entity.ToTable("tb_salt");

                entity.HasIndex(e => e.UserId, "tb_salt_FK");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Salt)
                    .HasColumnType("text")
                    .HasColumnName("salt");

                entity.Property(e => e.UserId)
                    .HasMaxLength(100)
                    .HasColumnName("userId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TbSalts)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("tb_salt_FK");
            });

            modelBuilder.Entity<TbScroll>(entity =>
            {
                entity.ToTable("tb_scroll");

                entity.HasIndex(e => e.BookId, "tb_scroll_FK");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BookId)
                    .HasMaxLength(100)
                    .HasColumnName("bookId");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.TbScrolls)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("tb_scroll_FK");
            });

            modelBuilder.Entity<TbState>(entity =>
            {
                entity.ToTable("tb_state");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<TbSubCommentBook>(entity =>
            {
                entity.ToTable("tb_subCommentBook");

                entity.HasIndex(e => e.OtherId, "otherId");

                entity.HasIndex(e => e.ReplyId, "tb_subCommentBook_FK");

                entity.HasIndex(e => e.ParentId, "tb_subCommentBook_ibfk_3");

                entity.HasIndex(e => e.UserId, "userId");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content)
                    .HasColumnType("text")
                    .HasColumnName("content");

                entity.Property(e => e.OtherId).HasColumnName("otherId");

                entity.Property(e => e.ParentId).HasColumnName("parentId");

                entity.Property(e => e.ReplyId).HasColumnName("replyId");

                entity.Property(e => e.Time).HasColumnName("time");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.Other)
                    .WithMany(p => p.TbSubCommentBookOthers)
                    .HasForeignKey(d => d.OtherId)
                    .HasConstraintName("tb_subCommentBook_ibfk_2");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.TbSubCommentBooks)
                    .HasForeignKey(d => d.ParentId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("tb_subCommentBook_ibfk_3");

                entity.HasOne(d => d.Reply)
                    .WithMany(p => p.InverseReply)
                    .HasForeignKey(d => d.ReplyId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("tb_subCommentBook_FK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TbSubCommentBookUsers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("tb_subCommentBook_ibfk_1");
            });

            modelBuilder.Entity<TbSubCommentChapter>(entity =>
            {
                entity.ToTable("tb_subCommentChapter");

                entity.HasIndex(e => e.OtherId, "otherId");

                entity.HasIndex(e => e.ReplyId, "tb_subCommentChapter_FK");

                entity.HasIndex(e => e.ParentId, "tb_subCommentChapter_ibfk_3");

                entity.HasIndex(e => e.UserId, "userId");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content)
                    .HasColumnType("text")
                    .HasColumnName("content");

                entity.Property(e => e.OtherId).HasColumnName("otherId");

                entity.Property(e => e.ParentId).HasColumnName("parentId");

                entity.Property(e => e.ReplyId).HasColumnName("replyId");

                entity.Property(e => e.Time).HasColumnName("time");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.Other)
                    .WithMany(p => p.TbSubCommentChapterOthers)
                    .HasForeignKey(d => d.OtherId)
                    .HasConstraintName("tb_subCommentChapter_ibfk_2");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.TbSubCommentChapters)
                    .HasForeignKey(d => d.ParentId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("tb_subCommentChapter_ibfk_3");

                entity.HasOne(d => d.Reply)
                    .WithMany(p => p.InverseReply)
                    .HasForeignKey(d => d.ReplyId)
                    .HasConstraintName("tb_subCommentChapter_FK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TbSubCommentChapterUsers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("tb_subCommentChapter_ibfk_1");
            });

            modelBuilder.Entity<TbTag>(entity =>
            {
                entity.ToTable("tb_tag");

                entity.HasIndex(e => e.BookId, "tb_tag_FK");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BookId)
                    .HasMaxLength(100)
                    .HasColumnName("bookId");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.TbTags)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("tb_tag_FK");
            });

            modelBuilder.Entity<TbThumbsUpBook>(entity =>
            {
                entity.ToTable("tb_thumbsUpBook");

                entity.HasIndex(e => e.CommentId, "tb_thumbsUpBook_ibfk_2");

                entity.HasIndex(e => e.UserId, "userId");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CommentId).HasColumnName("commentId");

                entity.Property(e => e.Up)
                    .HasColumnName("up")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.Comment)
                    .WithMany(p => p.TbThumbsUpBooks)
                    .HasForeignKey(d => d.CommentId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("tb_thumbsUpBook_ibfk_2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TbThumbsUpBooks)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("tb_thumbsUpBook_ibfk_1");
            });

            modelBuilder.Entity<TbThumbsUpChapter>(entity =>
            {
                entity.ToTable("tb_thumbsUpChapter");

                entity.HasIndex(e => e.CommentId, "tb_thumbsUpChapter_ibfk_2");

                entity.HasIndex(e => e.UserId, "userId");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CommentId).HasColumnName("commentId");

                entity.Property(e => e.Up)
                    .HasColumnName("up")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.Comment)
                    .WithMany(p => p.TbThumbsUpChapters)
                    .HasForeignKey(d => d.CommentId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("tb_thumbsUpChapter_ibfk_2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TbThumbsUpChapters)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("tb_thumbsUpChapter_ibfk_1");
            });

            modelBuilder.Entity<TbThumbsUpSubBook>(entity =>
            {
                entity.ToTable("tb_thumbsUpSubBook");

                entity.HasIndex(e => e.CommentId, "tb_thumbsUpSubBook_ibfk_2");

                entity.HasIndex(e => e.UserId, "userId");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CommentId).HasColumnName("commentId");

                entity.Property(e => e.Up)
                    .HasColumnName("up")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.Comment)
                    .WithMany(p => p.TbThumbsUpSubBooks)
                    .HasForeignKey(d => d.CommentId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("tb_thumbsUpSubBook_ibfk_2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TbThumbsUpSubBooks)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("tb_thumbsUpSubBook_ibfk_1");
            });

            modelBuilder.Entity<TbThumbsUpSubChapter>(entity =>
            {
                entity.ToTable("tb_thumbsUpSubChapter");

                entity.HasIndex(e => e.CommentId, "tb_thumbsUpSubChapter_ibfk_2");

                entity.HasIndex(e => e.UserId, "userId");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CommentId).HasColumnName("commentId");

                entity.Property(e => e.Up)
                    .HasColumnName("up")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.Comment)
                    .WithMany(p => p.TbThumbsUpSubChapters)
                    .HasForeignKey(d => d.CommentId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("tb_thumbsUpSubChapter_ibfk_2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TbThumbsUpSubChapters)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("tb_thumbsUpSubChapter_ibfk_1");
            });

            modelBuilder.Entity<TbType>(entity =>
            {
                entity.ToTable("tb_type");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.Remark)
                    .HasMaxLength(100)
                    .HasColumnName("remark");
            });

            modelBuilder.Entity<TbUser>(entity =>
            {
                entity.ToTable("tb_users");

                entity.HasIndex(e => e.RoleId, "tb_users_FK_1");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BanState)
                    .HasColumnName("banState")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Password)
                    .HasColumnType("text")
                    .HasColumnName("password");

                entity.Property(e => e.RoleId).HasColumnName("roleId");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.TbUsers)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("tb_users_FK_1");
            });

            modelBuilder.Entity<TbUserData>(entity =>
            {
                entity.ToTable("tb_userData");

                entity.HasIndex(e => e.Account, "tb_userData_FK");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Account).HasColumnName("account");

                entity.Property(e => e.BankCard)
                    .HasMaxLength(100)
                    .HasColumnName("bankCard");

                entity.Property(e => e.Birthday).HasColumnName("birthday");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .HasColumnName("email");

                entity.Property(e => e.Gender)
                    .HasColumnName("gender")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.IdCard)
                    .HasMaxLength(100)
                    .HasColumnName("idCard");

                entity.Property(e => e.ImgUrl)
                    .HasMaxLength(100)
                    .HasColumnName("imgUrl")
                    .HasDefaultValueSql("'/img/BraveDragon.png'");

                entity.Property(e => e.NickName)
                    .HasMaxLength(100)
                    .HasColumnName("nickName");

                entity.Property(e => e.RegisterTime).HasColumnName("registerTime");

                entity.Property(e => e.Tel)
                    .HasMaxLength(100)
                    .HasColumnName("tel");

                entity.HasOne(d => d.AccountNavigation)
                    .WithMany(p => p.TbUserData)
                    .HasForeignKey(d => d.Account)
                    .HasConstraintName("tb_userData_FK");
            });

            modelBuilder.Entity<TbWord>(entity =>
            {
                entity.ToTable("tb_word");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Word)
                    .HasMaxLength(255)
                    .HasColumnName("word");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
