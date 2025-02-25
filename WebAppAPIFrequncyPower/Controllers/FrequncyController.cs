using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebAppAPIFrequncyPower.Model;
using WebAppAPIFrequncyPower.Services.Actions;
using WebAppAPIFrequncyPower.Services.SignalR;

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
    }
}
