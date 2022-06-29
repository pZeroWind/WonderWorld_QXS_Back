using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WWModel.input;
using WWModel.Models;
using Tools;

namespace WWDAL
{
    public class BookManager : BaseManager<TbBook>
    {
        public BookManager(WWDBContext db) : base(db)
        {
        }

        public async Task<int> UploadBook(BookModel model,string token)
        {
            try
            {
                await _db.Database.BeginTransactionAsync();
                long date = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                string dateStr = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();
                string account = token.GetAccount();
                var book = new TbBook()
                {
                    Id = $"WW{dateStr}{account}{_db.TbBooks.Where(p => p.UserId == account).Count()}",
                    Title = model.Title,
                    Intro = model.Intro,
                    TypeId = model.TypeId,
                    CreateTime = date,
                    UpdateTime = date,
                    StateId = 1,
                    ClickNum = 0,
                    UserId = token.GetAccount()
                };
                if (model.Cover != null)
                {
                    book.Cover = model.Cover;
                }
                else
                {
                    book.Cover = "/img/BraveDragon.png";
                }
                await _db.TbBooks.AddAsync(book);
                if (!await IsTransactionSuccess())
                {
                    return 500;
                }
                foreach (string item in model.Tags!)
                {
                    await _db.TbTags.AddAsync(new TbTag()
                    {
                        BookId = book.Id,
                        Name = item
                    });
                    if (!await IsTransactionSuccess())
                    {
                        return 500;
                    }
                }
                await _db.Database.CommitTransactionAsync();
                return 200;
            }
            catch (Exception ex)
            {
                Log.Print(ex.Message, PrintMode.Error);
                return 500;
            }    
        }


        public async Task<int> UpdateBook(BookModel model)
        {
            try
            {
                await _db.Database.BeginTransactionAsync();
                var book = await _db.TbBooks.FindAsync(model.Id);
                if (model.Cover != null)
                {
                    book!.Cover = model.Cover;
                }
                else
                {
                    book!.Cover = "/img/BraveDragon.png";
                }
                book.Intro = model.Intro;
                book.Title = model.Title;
                book.TypeId = model.TypeId;
                _db.TbBooks.Update(book);
                if (!await IsTransactionSuccess())
                {
                    return 500;
                }
                await _db.TbTags.Where(p=>p.BookId == model.Id).DeleteFromQueryAsync();
                await _db.BulkSaveChangesAsync();
                foreach (string item in model.Tags!)
                {
                    await _db.TbTags.AddAsync(new TbTag()
                    {
                        BookId = book.Id,
                        Name = item
                    });
                    if (!await IsTransactionSuccess())
                    {
                        return 500;
                    }
                }
                await _db.Database.CommitTransactionAsync();
                return 200;
            }
            catch (Exception ex)
            {
                Log.Print(ex.Message, PrintMode.Error);
                return 500;
            }
        }
    }
}
