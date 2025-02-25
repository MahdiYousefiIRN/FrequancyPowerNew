namespace WebAppAPIFrequncyPower.Model
{
    public record PacketFrequencyMessage(
        PowerFrequencyData PowerFrequencyData,
        double Number,
        string SenderName,
        bool IsValid,
        DateTime Timestamp
    )
    {
        public PacketFrequencyMessage(PowerFrequencyData powerFrequency, double number, string senderName, bool isValid)
            : this(
                  new PowerFrequencyData
                  {
                      Id = powerFrequency.Id,
                      Frequency = powerFrequency.Frequency,
                      PostName = powerFrequency.PostName,
                      Power = powerFrequency.Power,
                      Timestamp = powerFrequency.Timestamp
                  },
                  number,
                  senderName,
                  isValid,
                  DateTime.UtcNow // به‌جای DateTimeOffset.Now
              )
        { }
    }
}
