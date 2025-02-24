// FrequencyController.cs
using Microsoft.AspNetCore.Mvc;
using WebAppAPIFrequncyPower.Model;
using WebAppAPIFrequncyPower.Services.InterfaceServic;

namespace WebAppAPIFrequncyPower.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FrequencyController : ControllerBase
    {
        private readonly IFrequncy _frequncyService;

        // دریافت سرویس از طریق DI
        public FrequencyController(IFrequncy frequncyService)
        {
            _frequncyService = frequncyService;
        }

        // گرفتن 10 داده آخر فرکانس
        [HttpGet]
        public async Task<IActionResult> GetFrequencies()
        {
            var frequencies = await _frequncyService.GetFrequncyDataTask();
            return Ok(frequencies.OrderByDescending(f => f.Timestamp).Take(10).ToList());
        }

        // دریافت داده جدید فرکانس
        [HttpPost]
        public async Task<IActionResult> PostFrequency([FromBody] PowerFrequencyData frequencyRecord)
        {
            // اگر فرکانس کمتر از 59 هرتز باشد، هشدار ثبت می‌شود.
            if (frequencyRecord.Frequency < 59)
            {
                var alert = new Alert()
                {
                    Timestamp = DateTime.Now,
                    Message = $"Frequency dropped below threshold: {frequencyRecord.Frequency} Hz",
                    Severity = "High"
                };
                // اضافه کردن هشدار به پایگاه داده از طریق سرویس
                await _frequncyService.AddAlertAsync(alert);
            }

            // اضافه کردن داده جدید فرکانس به پایگاه داده از طریق سرویس
            await _frequncyService.AddFrequencyDataAsync(frequencyRecord);

            return CreatedAtAction(nameof(GetFrequencies), new { id = frequencyRecord.Id }, frequencyRecord);
        }
    }
}
