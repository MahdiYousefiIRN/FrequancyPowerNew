namespace WebAppAPIFrequncyPower.Model
{
    public class Alert
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Message { get; set; }
        public string Severity { get; set; }
    }
}
