namespace MVCApp_PowerFrequency.Models
{
    public class PowerFrequencyData
    {
        public int Id { get; set; }
        public string PostName { get; set; }
        public DateTime Timestamp { get; set; }
        public double Frequency { get; set; }
        public double Power { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
    }
}
