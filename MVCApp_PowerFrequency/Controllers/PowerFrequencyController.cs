using Microsoft.AspNetCore.Mvc;
using MVCApp_PowerFrequency.Services;
using MVCApp_PowerFrequency.Models; // مدل داده‌ای جدید

namespace MVCApp_PowerFrequency.Controllers
{
    public class PowerFrequencyController : Controller
    {
        private readonly PowerFrequencyService _powerFrequencyService;

        // تزریق وابستگی برای سرویس
        public PowerFrequencyController(PowerFrequencyService powerFrequencyService)
        {
            _powerFrequencyService = powerFrequencyService;
        }

        // صفحه نمایش فرم ارسال داده
        public IActionResult Index()
        {
            return View();
        }

        // ارسال داده‌ها به API
        [HttpPost]
        public async Task<IActionResult> LogFrequency(PowerFrequencyData frequencyData)
        {
            if (ModelState.IsValid) // بررسی اعتبار داده‌ها
            {
                var success = await _powerFrequencyService.LogFrequencyAsync(frequencyData.Frequency, frequencyData.Timestamp);

                if (success)
                {
                    TempData["Message"] = "Data submitted successfully!";
                }
                else
                {
                    TempData["Message"] = "Error submitting data.";
                }

                return RedirectToAction("Index");
            }

            TempData["Message"] = "Invalid data.";
            return View("Index");
        }
    }
}
