using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MVCApp_PowerFrequency.Services
{
    public class PowerFrequencyService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        // تزریق HttpClient و IConfiguration
        public PowerFrequencyService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiUrl = configuration["ApiUrl"]; // آدرس API را از پیکربندی می‌خوانیم
        }

        public async Task<bool> LogFrequencyAsync(double frequency, string timestamp)
        {
            var frequencyData = new
            {
                frequency = frequency,
                timestamp = timestamp
            };

            // تبدیل داده‌ها به فرمت JSON
            var content = new StringContent(JsonConvert.SerializeObject(frequencyData), Encoding.UTF8, "application/json");

            try
            {
                // ارسال درخواست به API
                var response = await _httpClient.PostAsync($"{_apiUrl}/api/frequency/log-frequency", content);

                // بررسی موفقیت‌آمیز بودن درخواست
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    // گزارش خطا در صورت عدم موفقیت درخواست
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    // در اینجا می‌توانید پیام خطا را لاگ کنید یا برای نمایش به کاربر ارسال کنید
                    return false;
                }
            }
            catch (Exception ex)
            {
                // مدیریت استثناها
                // در صورت بروز خطا می‌توانید استثنا را لاگ کرده یا پیغام مناسبی را به کاربر نمایش دهید
                return false;
            }
        }
    }
}
