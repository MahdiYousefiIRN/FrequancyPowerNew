using WebAppAPIFrequncyPower.Services.InterfaceServic;

namespace WebAppAPIFrequncyPower.Services.CLassServic
{
    public class LoggingService : ILoggingService
    {
        public async Task LogAsync(string logMessage)
        {
            // کد ثبت پیام لاگ (مثل ذخیره در فایل، دیتابیس یا نمایش در کنسول)
            Console.WriteLine($"Log: {logMessage}");
            await Task.CompletedTask; // فرض بر این است که عملیات ثبت لاگ به طور ناهمزمان انجام می‌شود
        }
    }
}
