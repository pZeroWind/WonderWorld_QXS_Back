using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Aop.Api.Domain;
using Aop.Api;
using WWModel.Models;
using Aop.Api.Request;
using Tools;

namespace WWUI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PayController : ControllerBase
    {
        private readonly WWDBContext _db;
        private readonly AlipayConfig _payConfig;

        public PayController(WWDBContext db)
        {
            _db = db;
            _payConfig = new AlipayConfig()
            {
                AppId = PayConfig.AppId,
                PrivateKey = PayConfig.PrivateKey,
                AlipayPublicKey = PayConfig.AlipayPublicKey,
                Charset = PayConfig.CharSet,
                Format = PayConfig.Format,
                SignType = PayConfig.SignType,
                ServerUrl = PayConfig.GatewayUrl
            };
        }

        /// <summary>
        /// 购买硬币
        /// </summary>
        /// <returns></returns>
        [HttpPost("charge")]
        public async Task<IActionResult> PayCoin(int size, int type)
        {
            string account = HttpContext.Request.Headers["Authorization"].ToString().GetAccount();
            //配置支付设置
            DefaultAopClient aopClient = new DefaultAopClient(_payConfig);
            //配置订单
            AlipayTradePagePayModel model = new AlipayTradePagePayModel();
            //订单详情
            model.Subject = "充值";
            //订单总价
            switch (type)
            {
                case 1:
                    model.TotalAmount = size.ToString();
                    break;
                case 2:
                    model.TotalAmount = size.ToString();
                    break;
                case 3:
                    model.TotalAmount = (size * 0.01).ToString();
                    break;
            }
            //订单编号
            model.OutTradeNo = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            model.ProductCode = "FAST_INSTANT_TRADE_PAY";
            //发送请求
            AlipayTradePagePayRequest request = new AlipayTradePagePayRequest();
            //返回地址
            request.SetReturnUrl($"http://175.178.20.112:5107/pay/result/{type}/{account}/{size}");
            //订单模型
            request.SetBizModel(model);
            //获取请求的URL
            //var response = aopClient.pageExecute(request);
            //return Content(response.Body);
            var response = aopClient.SdkExecute(request);
            return await Task.FromResult(Ok(new WWModel.Result.Result<string>() { data = PayConfig.GatewayUrl + "?" + response.Body }));
        }

        [HttpGet("result/{type}/{account}/{size}")]
        public async Task<IActionResult> Result(string account, int size,int? type = 1)
        {
            var item = _db.TbItems.Where(p=>p.UserId==account).First();
            switch (type)
            {
                case 1:
                    item!.TiketNum += size;
                    break;
                case 2:
                    item!.BladeNum += size;
                    break;
                case 3:
                    item!.CoinNum += size;
                    break;
            }
            _db.TbItems.Update(item!);
            await _db.SaveChangesAsync();
            return await Task.FromResult(new RedirectResult("http://175.178.20.112:8080"));
            //return Ok();
        }

        [HttpPost("result/{type}")]
        public async Task<IActionResult> Result2(int? type = 1)
        {
            Console.WriteLine(type);
            return await Task.FromResult(Ok("success"));
        }
    }
}
