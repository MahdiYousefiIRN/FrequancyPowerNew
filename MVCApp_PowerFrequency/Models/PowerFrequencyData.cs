namespace MVCApp_PowerFrequency.Models
{
    public class PowerFrequencyData
    {
        public int Id { get; set; }
        public string PostName { get; set; }  // نام پست نیروگاهی
        public DateTime Timestamp { get; set; } // زمان دریافت داده
        public double Frequency { get; set; }   // فرکانس (Hz)
        public double Power { get; set; }

        // پروپرتی‌های جداگانه برای تاریخ و زمان
        public string Date { get; set; }  // تاریخ (مثلاً: "2025-02-25")
        public string Time { get; set; }  // زمان (مثلاً: "14:30")
    }
}
