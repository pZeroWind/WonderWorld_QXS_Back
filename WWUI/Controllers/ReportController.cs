using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tools;
using WWBLL;
using WWModel.Models;
using WWModel.Result;

namespace WWUI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ReportService _service;

        public ReportController(WWDBContext db)
        {
            _service = new ReportService(db);
        }

        [HttpGet]
        public async Task<IActionResult> Get(int? page = 1,int? size = 5)
        {
            return Ok(await _service.GetData((int)page!, (int)size!));
        }

        [HttpPost]
        public async Task<IActionResult> Add(ReportData model)
        {
            var account = HttpContext.Request.Headers["Authorization"].ToString().GetAccount();
            model.UserId = account;
            return Ok(await _service.Add(model));
        }

        [HttpPut]
        public async Task<IActionResult> Update(int id)
        {
            return Ok(await _service.Complete(id));
        }
    }
}
