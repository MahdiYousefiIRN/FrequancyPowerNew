using WebAppAPIFrequncyPower.Model;
using WebAppAPIFrequncyPower.Services.InterfaceServic;

namespace WebAppAPIFrequncyPower.Services.Actions
{
    public class SaveFrequencyDataAction : IAction
    {
        private readonly IFrequncy _frequncyService;

        // برای تزریق وابستگی به سرویس داده‌ها
        public SaveFrequencyDataAction(IFrequncy frequncyService)
        {
            _frequncyService = frequncyService;
        }

        // پیاده‌سازی متد ExecuteAsync
        public async Task ExecuteAsync(PowerFrequencyData frequencyData)
        {
            // فراخوانی سرویس برای ذخیره داده‌ها در دیتابیس
            await _frequncyService.AddFrequencyDataAsync(frequencyData);
        }
    }
}
