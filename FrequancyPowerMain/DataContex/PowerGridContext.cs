namespace FrequancyPowerMain.DataContex
{
    using FrequancyPowerMain.Model;
    using Microsoft.EntityFrameworkCore;

    public class PowerGridContext : DbContext
    {
        public PowerGridContext(DbContextOptions<PowerGridContext> options)
            : base(options) { }

        public DbSet<FrequencyRecord> FrequencyRecords { get; set; }
        public DbSet<Alert> Alerts { get; set; }
        public DbSet<PowerFrequencyData> PowerFrequencyData { get; set; }

    }
  


}
