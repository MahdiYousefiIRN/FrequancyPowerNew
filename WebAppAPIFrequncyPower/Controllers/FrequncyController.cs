using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebAppAPIFrequncyPower.Model;
using WebAppAPIFrequncyPower.Services.Actions;
using WebAppAPIFrequncyPower.Services.SignalR;
using System.Collections.Generic;

namespace WebAppAPIFrequncyPower.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FrequencyController : ControllerBase
    {
        private readonly ActionManager _actionManager;
        private readonly IHubContext<FrequencyHub> _hubContext;

        public FrequencyController(ActionManager actionManager, IHubContext<FrequencyHub> hubContext)
        {
            _actionManager = actionManager;
            _hubContext = hubContext;
        }

        // ثبت داده‌های فرکانس و ارسال به کلاینت‌ها
        [HttpPost("log-frequency")]
        public async Task<IActionResult> LogFrequency([FromBody] PowerFrequencyData frequencyData)
        {
            if (frequencyData == null)
            {
                return BadRequest("Data is required");
            }

            // اجرای اکشن‌ها از طریق ActionManager
            await _actionManager.ExecuteActionsAsync(frequencyData);

            // ارسال داده به کلاینت‌های متصل به SignalR
            await _hubContext.Clients.All.SendAsync("ReceiveFrequencyData", frequencyData);

            return Ok("Frequency data processed and sent successfully");
        }

        // ارسال هشدار در صورت کاهش فرکانس
        [HttpPost("send-alert")]
        public async Task<IActionResult> SendAlert([FromBody] PowerFrequencyData frequencyData)
        {
            if (frequencyData == null)
            {
                return BadRequest("Data is required");
            }

            // اجرای اکشن ارسال هشدار
            await _actionManager.ExecuteActionsAsync(frequencyData);

            return Ok("Alert sent successfully if necessary");
        }

        // دریافت داده‌های فرکانس
        [HttpGet("get-frequency-data")]
        public IActionResult GetFrequencyData()
        {
            try
            {
                // فرض می‌کنیم که داده‌های فرکانس از جایی خوانده می‌شوند (مثلاً از دیتابیس یا حافظه)
                var frequencyData = GetFrequencyDataFromSource(); // باید این متد را به پیاده‌سازی کنید

                if (frequencyData == null || frequencyData.Count == 0)
                {
                    return NotFound("No frequency data found.");
                }

                return Ok(frequencyData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        // متد فرضی برای خواندن داده‌های فرکانس
        private List<PowerFrequencyData> GetFrequencyDataFromSource()
        {
            // در اینجا باید داده‌های فرکانس را از منبع مورد نظر (مثل دیتابیس یا حافظه) برگردانید.
            // این فقط یک مثال است.
            return new List<PowerFrequencyData>
            {
                new PowerFrequencyData { Timestamp = DateTime.Now, Frequency = 59.5, Power = 100 },
                new PowerFrequencyData { Timestamp = DateTime.Now.AddMinutes(-1), Frequency = 59.2, Power = 95 }
            };
        }
    }
}
