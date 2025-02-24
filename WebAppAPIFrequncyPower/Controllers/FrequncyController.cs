//// FrequencyController.cs
//using Microsoft.AspNetCore.Mvc;
//using WebAppAPIFrequncyPower.Model;
//using WebAppAPIFrequncyPower.Services.InterfaceServic;

//namespace WebAppAPIFrequncyPower.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class FrequencyController : ControllerBase
//    {
//        private readonly IFrequncy _frequncyService;

//        // دریافت سرویس از طریق DI
//        public FrequencyController(IFrequncy frequncyService)
//        {
//            _frequncyService = frequncyService;
//        }

//        // گرفتن 10 داده آخر فرکانس
//        [HttpGet]
//        public async Task<IActionResult> GetFrequencies()
//        {
//            var frequencies = await _frequncyService.GetFrequncyDataTask();
//            return Ok(frequencies.OrderByDescending(f => f.Timestamp).Take(10).ToList());
//        }

//        // دریافت داده جدید فرکانس
//        [HttpPost]
//        public async Task<IActionResult> PostFrequency([FromBody] PowerFrequencyData frequencyRecord)
//        {
//            // اگر فرکانس کمتر از 59 هرتز باشد، هشدار ثبت می‌شود.
//            if (frequencyRecord.Frequency < 59)
//            {
//                var alert = new Alert()
//                {
//                    Timestamp = DateTime.Now,
//                    Message = $"Frequency dropped below threshold: {frequencyRecord.Frequency} Hz",
//                    Severity = "High"
//                };
//                // اضافه کردن هشدار به پایگاه داده از طریق سرویس
//                await _frequncyService.AddAlertAsync(alert);
//            }

//            // اضافه کردن داده جدید فرکانس به پایگاه داده از طریق سرویس
//            await _frequncyService.AddFrequencyDataAsync(frequencyRecord);

//            return CreatedAtAction(nameof(GetFrequencies), new { id = frequencyRecord.Id }, frequencyRecord);
//        }
//    }
//}
using Microsoft.AspNetCore.Mvc;
using WebAppAPIFrequncyPower.Model;
using WebAppAPIFrequncyPower.Services.Actions;
namespace WebAppAPIFrequncyPower.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FrequencyController : ControllerBase
    {
        private readonly ActionManager _actionManager;

        public FrequencyController(ActionManager actionManager)
        {
            _actionManager = actionManager;
        }

        // این متد برای ثبت داده‌های فرکانس و انجام اکشن‌ها
        [HttpPost("log-frequency")]
        public async Task<IActionResult> LogFrequency([FromBody] PowerFrequencyData frequencyData)
        {
            if (frequencyData == null)
            {
                return BadRequest("Data is required");
            }

            // اجرای اکشن‌ها از طریق ActionManager
            await _actionManager.ExecuteActionsAsync(frequencyData);

            return Ok("Frequency data processed successfully");
        }

        // این متد برای ارسال هشدار در صورت پایین آمدن فرکانس
        [HttpPost("send-alert")]
        public async Task<IActionResult> SendAlert([FromBody] PowerFrequencyData frequencyData)
        {
            if (frequencyData == null)
            {
                return BadRequest("Data is required");
            }

            // فرض می‌کنیم که اکشن ارسال هشدار در صورت پایین آمدن فرکانس اجرا می‌شود
            await _actionManager.ExecuteActionsAsync(frequencyData);

            return Ok("Alert sent successfully if necessary");
        }
    }
}

