namespace MVCApp_PowerFrequency.Models
{
    public class ChartDataModel
    {
        public string[] Labels { get; set; }        // لیبل‌ها برای نمایش در محور افقی (تاریخ‌و‌زمان)
        public double[] Frequencies { get; set; }   // داده‌های فرکانس برای نمودار
        public double[] Powers { get; set; }        // داده‌های قدرت برای نمودار
    }
}
