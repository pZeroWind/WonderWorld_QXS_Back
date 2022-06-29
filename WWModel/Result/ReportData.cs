using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WWModel.Models;

namespace WWModel.Result
{
    public class ReportData
    {
        public ReportData() { }

        public ReportData(TbReportBook book)
        {
            Id = book.Id;
            ChapterId = book.ChapterId;
            Details = book.Details;
            State = book.State;
            UserId = book.UserId;
        }

        public int Id { get; set; }
        public int? ChapterId { get; set; }
        public string? Details { get; set; }
        public int? State { get; set; }
        public string? UserId { get; set; }
    }
}
