using WebAppAPIFrequncyPower.Model;
using WebAppAPIFrequncyPower.Services.InterfaceServic;

namespace WebAppAPIFrequncyPower.Services.Actions
{
    public class SendAlertAction : IAction
    {
        private readonly IAlertService _alertService;

        public SendAlertAction(IAlertService alertService)
        {
            _alertService = alertService;
        }

        public async Task ExecuteAsync(PowerFrequencyData frequencyData)
        {
            // اگر فرکانس کمتر از 59 هرتز باشد، ارسال هشدار
            if (frequencyData.Frequency < 59)
            {
                var alertMessage = $"Alert: Frequency dropped below threshold: {frequencyData.Frequency} Hz";
                await _alertService.SendAlertAsync(alertMessage);
            }
        }
    }
}
