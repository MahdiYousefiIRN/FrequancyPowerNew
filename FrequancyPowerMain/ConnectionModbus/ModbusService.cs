using FrequancyPowerMain.DataContex;
using FrequancyPowerMain.Model;
using Modbus.Data;
using Modbus.Device;
using System.Net.Sockets;
using Modbus.Net;
using System.Net.Sockets;

namespace FrequancyPowerMain.ConnectionModbus
{
   
  public class ModbusService
    {
        private string _ipAddress = "127.0.0.1"; // آدرس IP دستگاه Modbus
        private int _port = 502; // پورت Modbus

        public async Task<double> ReadFrequencyAsync()
        {
            // اتصال به دستگاه Modbus
            using (var client = new TcpClient(_ipAddress, _port))
            {
                var modbusMaster = ModbusIpMaster.CreateIp(client);

                // فرض می‌کنیم که داده‌های فرکانس در آدرس 40001 قرار دارد
                var holdingRegisters = await modbusMaster.ReadHoldingRegistersAsync(1, 40001, 2);

                // تبدیل ushort[] به byte[]
                byte[] byteArray = new byte[holdingRegisters.Length * 2]; // هر ushort معادل دو byte است
                Buffer.BlockCopy(holdingRegisters, 0, byteArray, 0, byteArray.Length);

                // تبدیل داده‌های خوانده‌شده به فرکانس
                double frequency = BitConverter.ToUInt16(byteArray, 0) / 10.0;
                return frequency;
            }
        }

        public class DataService
        {
            private readonly PowerGridContext _context;

            public DataService(PowerGridContext context)
            {
                _context = context;
            }

            public async Task SaveDataAsync(PowerFrequencyData data)
            {
                // ذخیره‌سازی داده‌های فرکانس و توان در پایگاه داده
                _context.PowerFrequencyData.Add(data);
                await _context.SaveChangesAsync();
            }
        }
    }
    public class ModbusService3 : IModbusService
    {
        public async Task<double> GetFrequencyAsync(string ipAddress, int port)
        {
            // اتصال به دستگاه Modbus و دریافت داده فرکانس
            // اینجا می‌توانید از کتابخانه‌های Modbus مانند NModbus استفاده کنید.
            return 50.0; // مقدار شبیه‌سازی‌شده
        }

        public async Task<double> GetPowerAsync(string ipAddress, int port)
        {
            // اتصال به دستگاه Modbus و دریافت داده توان
            return 100.0; // مقدار شبیه‌سازی‌شده
        }
    }
    public class ModbusService2
    {
        private readonly string _ipAddress;
        private readonly int _port;

        public ModbusService2(string ipAddress, int port)
        {
            _ipAddress = ipAddress;
            _port = port;
        }

        public async Task<PowerFrequencyData> ReadDataAsync()
        {
            using (var tcpClient = new TcpClient(_ipAddress, _port))  // استفاده از using برای مدیریت منابع
            {
                var modbusClient = ModbusIpMaster.CreateIp(tcpClient);

                // خواندن فرکانس و توان از رجیسترهای Modbus
                var frequencyRegisters = await modbusClient.ReadHoldingRegistersAsync(0, 1);
                var powerRegisters = await modbusClient.ReadHoldingRegistersAsync(1, 1);

                // بررسی اینکه آیا داده‌ها موجود هستند
                if (frequencyRegisters.Length > 0 && powerRegisters.Length > 0)
                {
                    double frequencyValue = frequencyRegisters[0] / 10.0;  // فرض بر این است که داده‌های فرکانس باید تقسیم بر 10 شوند
                    double powerValue = powerRegisters[0] / 1000.0;        // فرض بر این است که داده‌های توان باید تقسیم بر 1000 شوند

                    return new PowerFrequencyData
                    {
                        Timestamp = DateTime.Now,
                        Frequency = frequencyValue,
                        Power = powerValue
                    };
                }
                else
                {
                    throw new InvalidOperationException("Data read from Modbus is invalid.");
                }
            }
        }
    }
    public interface IModbusService
    {
        Task<double> GetFrequencyAsync(string ipAddress, int port);
        Task<double> GetPowerAsync(string ipAddress, int port);
    }
    public class ModbusTcpClient
    {
        public ModbusTcpClient(TcpClient tcpClient)
        {
            throw new NotImplementedException();
        }

        public async Task<object> ReadHoldingRegistersAsync(int p0, int p1)
        {
            throw new NotImplementedException();
        }
    }
}
