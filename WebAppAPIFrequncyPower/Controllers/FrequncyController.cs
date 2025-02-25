using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using WebAppAPIFrequncyPower.Model;
using WebAppAPIFrequncyPower.Services.Actions;
using WebAppAPIFrequncyPower.Services.SignalR;
using WebAppAPIFrequncyPower.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppAPIFrequncyPower.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FrequencyController : ControllerBase
    {
        private readonly ActionManager _actionManager;
        private readonly IHubContext<FrequencyHub> _hubContext;
        private readonly PowerGridContext _context;

        public FrequencyController(ActionManager actionManager, IHubContext<FrequencyHub> hubContext, PowerGridContext context)
        {
            _actionManager = actionManager;
            _hubContext = hubContext;
            _context = context;
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

        // دریافت داده‌های فرکانس در بازه زمانی خاص
        [HttpGet("get-frequency-data")]
        public async Task<IActionResult> GetFrequencyData([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                // دریافت داده‌ها از دیتابیس با فیلتر تاریخ
                var frequencyData = await _context.DbSetFrequency
                    .Where(f => f.Timestamp >= startDate && f.Timestamp <= endDate)
                    .ToListAsync();

                if (frequencyData == null || frequencyData.Count == 0)
                {
                    return NotFound("No frequency data found for the given time range.");
                }

                return Ok(frequencyData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        // اکشن جدید برای دریافت داده‌های فرکانس با فیلتر تاریخ
        [HttpGet("get-frequency-data-by-date")]
        public async Task<IActionResult> GetFrequencyDataByDate([FromQuery] string startDate, [FromQuery] string endDate)
        {
            try
            {
                // تبدیل تاریخ‌ها به نوع DateTime
                if (DateTime.TryParse(startDate, out var parsedStartDate) && DateTime.TryParse(endDate, out var parsedEndDate))
                {
                    // دریافت داده‌ها با فیلتر تاریخ‌ها
                    var frequencyData = await _context.DbSetFrequency
                        .Where(f => f.Timestamp >= parsedStartDate && f.Timestamp <= parsedEndDate)
                        .ToListAsync();

                    if (frequencyData == null || frequencyData.Count == 0)
                    {
                        return NotFound("No frequency data found for the given date range.");
                    }

                    return Ok(frequencyData);
                }
                else
                {
                    return BadRequest("Invalid date format.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
