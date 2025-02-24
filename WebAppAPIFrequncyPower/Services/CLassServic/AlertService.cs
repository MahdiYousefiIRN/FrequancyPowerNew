using WebAppAPIFrequncyPower.Services.InterfaceServic;

namespace WebAppAPIFrequncyPower.Services.CLassServic
{
    public class AlertService : IAlertService
    {
        public async Task SendAlertAsync(string message)
        {
            // کد ارسال هشدار (مثل ارسال ایمیل، پیامک یا هر سیستم دیگری)
            Console.WriteLine($"Alert: {message}");
            await Task.CompletedTask; // فرض بر این است که عملیات ارسال هشدار به طور ناهمزمان انجام می‌شود
        }
    }
}
