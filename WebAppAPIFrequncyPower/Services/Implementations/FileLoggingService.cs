using WebAppAPIFrequncyPower.Services.InterfaceServic;

namespace WebAppAPIFrequncyPower.Services.Implementations
{
    public class FileLoggingService : ILoggingService
    {
        private readonly string _logFilePath = "logs.txt"; // مسیر فایل گزارش

        public async Task LogAsync(string message)
        {
            // ثبت پیام گزارش در فایل
            await File.AppendAllTextAsync(_logFilePath, $"{DateTime.Now}: {message}\n");
        }
    }
}
