using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WWBLL;
using WWModel.Models;
using WWUI.Filter;

namespace WWUI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class WordController : ControllerBase
    {
        private readonly WordService _wordService;

        public WordController(WWDBContext db)
        {
            _wordService = new WordService(db);
        }

        /// <summary>
        /// 获取所有违禁词
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            return Ok(await _wordService.GetList(_ => true)); 
        }

        /// <summary>
        /// 添加违禁词
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        [HttpPost]
        [Auth("ModifyWord")]
        public async Task<IActionResult> Add(TbWord word)
        {
            return Ok(await _wordService.Append(word));
        }

        /// <summary>
        /// 删除违禁词
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Auth("ModifyWord")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _wordService.Remove(id));
        }

    }
}
