using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using MVCApp_PowerFrequency.Models;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace MVCApp_PowerFrequency.Controllers
{
    public class PacketController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<PacketController> _logger;
        private readonly HubConnection _hubConnection;
        private readonly string _apiUrl; // تغییر آدرس API به تنظیمات
        private bool _isSignalRConnected = false;

        public PacketController(IHttpClientFactory httpClientFactory, ILogger<PacketController> logger, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;

            _apiUrl = configuration["ApiUrl"] ?? throw new InvalidOperationException("API URL is not configured.");

            _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7077/frequencyHub") // SignalR Hub URL
                .Build();
        }

        // صفحه اصلی با داده‌های فرکانس و قدرت
        public async Task<IActionResult> Index()
        {
            var chartData = await GetChartDataAsync(); // دریافت داده‌ها از Web API
            ViewData["SignalRStatus"] = _isSignalRConnected ? "Connected" : "Disconnected";
            return View("~/Views/Home/PacketView.cshtml", chartData);  // ارسال داده‌ها به ویو
        }

        // متد برای دریافت داده‌های فرکانس از Web API
        private async Task<ChartDataModel> GetChartDataAsync()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetStringAsync($"{_apiUrl}/Frequency/get-frequency-data");  // ارسال درخواست به Web API

            var data = JsonSerializer.Deserialize<PowerFrequencyData[]>(response);  // تبدیل پاسخ به داده‌ها
            return new ChartDataModel
            {
                Labels = data.Select(m => m.Timestamp.ToString("HH:mm:ss")).ToArray(),
                Frequencies = data.Select(m => m.Frequency).ToArray(),
                Powers = data.Select(m => m.Power).ToArray()
            };
        }

        // متد برای شروع اتصال به SignalR
        public async Task<IActionResult> StartSignalRConnection()
        {
            try
            {
                _hubConnection.On<PowerFrequencyData>("ReceiveMessage", (data) =>
                {
                    _logger.LogInformation("Received SignalR data: " + data.Frequency);
                    UpdateChartData(data); // به روز رسانی داده‌ها در نمودار
                });

                await _hubConnection.StartAsync();
                _isSignalRConnected = true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error connecting to SignalR: " + ex.Message);
                ViewData["SignalRStatus"] = "Error connecting to SignalR";
            }

            return RedirectToAction("Index");
        }

        // متد برای قطع اتصال از SignalR
        public async Task<IActionResult> StopSignalRConnection()
        {
            await _hubConnection.StopAsync();
            _isSignalRConnected = false;
            ViewData["SignalRStatus"] = "Disconnected";
            return RedirectToAction("Index");
        }

        // متد برای ارسال داده به Web API
        [HttpPost]
        public async Task<IActionResult> SendPacket(PowerFrequencyData packet)
        {
            if (packet == null)
            {
                ViewData["Message"] = "Invalid packet data.";
                return RedirectToAction("Index");
            }

            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                var jsonContent = JsonContent.Create(packet);
                var response = await httpClient.PostAsync($"{_apiUrl}/log-frequency", jsonContent); // اصلاح URL

                if (response.IsSuccessStatusCode)
                {
                    ViewData["Message"] = "Packet sent successfully!";
                }
                else
                {
                    ViewData["Message"] = "Error sending packet!";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error sending packet: " + ex.Message);
                ViewData["Message"] = "Error sending packet!";
            }

            return RedirectToAction("Index");
        }

        // متد برای به روز رسانی داده‌ها در نمودار بعد از دریافت از SignalR
        private void UpdateChartData(PowerFrequencyData data)
        {
            var chartData = new ChartDataModel
            {
                Labels = new string[] { data.Timestamp.ToString("HH:mm:ss") },
                Frequencies = new double[] { data.Frequency },
                Powers = new double[] { data.Power }
            };
            ViewData["ChartData"] = chartData;
        }
    }
}
