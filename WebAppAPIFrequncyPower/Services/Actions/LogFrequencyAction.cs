using WebAppAPIFrequncyPower.Model;
using WebAppAPIFrequncyPower.Services.InterfaceServic;

namespace WebAppAPIFrequncyPower.Services.Actions
{
    public class LogFrequencyAction : IAction
    {
        private readonly ILoggingService _loggingService;

        public LogFrequencyAction(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        public async Task ExecuteAsync(PowerFrequencyData frequencyData)
        {
            // ثبت اطلاعات فرکانس در گزارش
            string logMessage = $"Frequency at {DateTime.Now}: {frequencyData.Frequency} Hz";
            await _loggingService.LogAsync(logMessage);
        }
    }

}
