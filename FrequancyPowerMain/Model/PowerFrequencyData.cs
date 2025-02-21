namespace FrequancyPowerMain.Model
{
    public class PowerFrequencyData
    {
        public int Id { get; set; }
        public string PostName { get; set; }  // نام پست نیروگاهی
        public DateTime Timestamp { get; set; } // زمان دریافت داده
        public double Frequency { get; set; }   // فرکانس (Hz)
        public double Power { get; set; }       // توان (MW یا kW)

      
    }

}
