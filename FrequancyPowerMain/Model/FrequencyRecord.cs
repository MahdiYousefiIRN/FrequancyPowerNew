namespace FrequancyPowerMain.Model
{
    public class FrequencyRecord
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public double Frequency { get; set; }
        public string? StationName { get; set; }
    }
}
