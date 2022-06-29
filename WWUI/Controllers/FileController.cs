using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WWModel.Models;
using WWModel.Result;
using WWUI.Filter;
using Tools;
using Microsoft.AspNetCore.Authorization;

namespace WWUI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHost;

        public FileController(IWebHostEnvironment webHost, WWDBContext db)
        {
            _webHost = webHost;
        }

        [HttpPost("Upload")]
        [Authorize]
        public async Task<IActionResult> UploadFile()
        {
            return await Task.Run(() =>
            {
                var files = Request.Form.Files.ToList();
                Result<List<string>> result = new Result<List<string>>();
                result.data = new List<string>();
                files.ForEach(f =>
                {
                    string ext = Path.GetExtension(f.FileName);
                    string name = Guid.NewGuid().ToString();
                    string date = DateTime.Now.ToString("yyyy_M_d");
                    string path = $"/File/{HttpContext.Request.Headers["Authorization"].ToString().GetAccount()}/{date}/";
                    string root = _webHost.WebRootPath;
                    if (!Directory.Exists(root + path))
                    {
                        Directory.CreateDirectory(root + path);
                    }
                    using (FileStream fs = new FileStream(root + path + name + ext, FileMode.Create, FileAccess.Write))
                    {
                        f.CopyTo(fs);
                        result.data.Add(path + name + ext);
                        fs.Close();
                    }
                });
                return Ok(result);
            });
        }
    }
}
