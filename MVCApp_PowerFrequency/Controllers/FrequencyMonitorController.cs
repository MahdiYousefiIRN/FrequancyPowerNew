using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;

namespace MVCApp_PowerFrequency.Controllers
{

    // متد برای شروع اتصال به SignalR
    public class FrequencyMonitorController : Controller
    {
        private readonly HubConnection _hubConnection;

        public FrequencyMonitorController()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7077/frequencyHub") // آدرس دقیق SignalR Hub
                .Build();
        }

        public async Task<IActionResult> Index()
        {
            return View(); // به‌طور پیش‌فرض به Views/FrequencyMonitor/Index.cshtml می‌رود
        }

        public async Task<IActionResult> StartSignalRConnection()
        {
            _hubConnection.On<string>("ReceiveMessage", (message) =>
            {
                // ارسال پیام دریافتی به View
                ViewData["Message"] = message;
            });

            await _hubConnection.StartAsync();
            return View("Index"); // نمایش View به نام Index
        }

        public async Task<IActionResult> StopSignalRConnection()
        {
            await _hubConnection.StopAsync();
            return View("Index"); // نمایش View به نام Index
        }
    }
}
