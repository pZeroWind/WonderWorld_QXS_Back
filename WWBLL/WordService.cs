using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WWModel.Models;

namespace WWBLL
{
    public class WordService : BaseService<TbWord>
    {

        public WordService(WWDBContext db) : base(db)
        {
           
        }
    }
}
