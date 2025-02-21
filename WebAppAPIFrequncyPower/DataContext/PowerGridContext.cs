using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using WebAppAPIFrequncyPower.Model;

namespace WebAppAPIFrequncyPower.DataContext
{
    public class PowerGridContext:DbContext 
    {
        public PowerGridContext(DbContextOptions<PowerGridContext>options)
        :base(options)
        {
                
        }
        public DbSet<Alert> DbSetAlerts { get; set; }

        public DbSet<PowerFrequencyData> DbSetFrequency { get; set; }      
    }
}
