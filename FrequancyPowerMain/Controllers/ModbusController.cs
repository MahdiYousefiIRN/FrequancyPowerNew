using FrequancyPowerMain.DataContex;
using FrequancyPowerMain.Model;
using Microsoft.AspNetCore.Mvc;
using NModbus;
using System.Net.Sockets;
using Microsoft.AspNetCore.Mvc;
using NModbus;
using System.Net.Sockets;
using FrequancyPowerMain.DataContex;
using FrequancyPowerMain.Model;

namespace FrequancyPowerMain.Controllers
{
    
        [ApiController]
        [Route("api/[controller]")]
        public class ModbusController : ControllerBase
        {
            private readonly PowerGridContext _context;

            // تنظیمات دستگاه Modbus (این مقادیر را بر اساس دستگاه واقعی خود تغییر دهید)
            private readonly string _ipAddress = "192.168.1.100"; // آدرس IP دستگاه Modbus
            private readonly int _port = 502; // پورت Modbus TCP

            public ModbusController(PowerGridContext context)
            {
                _context = context;
            }

            [HttpGet("read")]
            public IActionResult ReadModbusData()
            {
                try
                {
                    using TcpClient client = new TcpClient(_ipAddress, _port);

                    // استفاده از ModbusFactory برای ایجاد Master (سازگار با NModbus4)
                    var factory = new ModbusFactory();
                    var master = factory.CreateMaster(client);

                    // خواندن 10 مقدار از Holding Register از آدرس شروع 0
                    ushort startAddress = 0;
                    ushort numberOfPoints = 10;
                    ushort[] registers = master.ReadHoldingRegisters(1, startAddress, numberOfPoints);

                    // تفسیر اولین رجیستر به عنوان فرکانس (به عنوان مثال تقسیم بر 10 برای مقیاس‌دهی)
                    double frequency = registers[0] / 10.0;
                    var record = new FrequencyRecord
                    {
                        Timestamp = DateTime.Now,
                        Frequency = frequency,
                        StationName = "Station1"
                    };

                    // ذخیره داده در پایگاه داده
                    _context.FrequencyRecords.Add(record);
                    _context.SaveChanges();

                    return Ok(new { Registers = registers, Frequency = frequency });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"خطا در خواندن داده‌های Modbus: {ex.Message}");
                }
            }
        }
    

}
