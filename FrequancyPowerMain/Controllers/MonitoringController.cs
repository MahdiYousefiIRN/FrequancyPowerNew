using FrequancyPowerMain.ConnectionModbus;
using FrequancyPowerMain.Model;
using Microsoft.AspNetCore.Mvc;

namespace FrequancyPowerMain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonitoringController : ControllerBase
    {
        private readonly ModbusService2 _modbusService;
        private readonly ModbusService.DataService _dataService;

        // تزریق وابستگی‌ها
        public MonitoringController(ModbusService2 modbusService, ModbusService.DataService dataService)
        {
            _modbusService = modbusService;
            _dataService = dataService;
        }

        [HttpGet("fetch-data")]
        public async Task<IActionResult> FetchData()
        {
            try
            {
                // خواندن داده‌ها از Modbus
                PowerFrequencyData data = await _modbusService.ReadDataAsync();

                // ذخیره‌سازی داده‌ها در پایگاه داده
                await _dataService.SaveDataAsync(data);

                // ارسال داده‌ها به‌صورت موفقیت‌آمیز
                return Ok(data);
            }
            catch (Exception ex)
            {
                // در صورت وقوع خطا، پیامی مناسب ارسال می‌کنیم
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}