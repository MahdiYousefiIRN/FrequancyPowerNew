// Services/ClassServic/DbFrequncy.cs
using Microsoft.EntityFrameworkCore;
using WebAppAPIFrequncyPower.DataContext;
using WebAppAPIFrequncyPower.Model;
using WebAppAPIFrequncyPower.Services.InterfaceServic;

namespace WebAppAPIFrequncyPower.Services.ClassServic
{
    public class DbFrequncy : IFrequncy
    {
        private readonly PowerGridContext _context;

        public DbFrequncy(PowerGridContext context)
        {
            _context = context;
        }

        // دریافت داده‌های فرکانس
        public async Task<IEnumerable<PowerFrequencyData>> GetFrequncyDataTask()
        {
            return await _context.DbSetFrequency.ToListAsync();
        }

        // دریافت داده فرکانس بر اساس ID
        public async Task<PowerFrequencyData> GetFrequncytById(int id)
        {
            return await _context.DbSetFrequency.SingleOrDefaultAsync(f => f.Id == id);
        }

        // افزودن داده جدید فرکانس به پایگاه داده
        public async Task AddFrequencyDataAsync(PowerFrequencyData frequencyData)
        {
            _context.DbSetFrequency.Add(frequencyData);
            await _context.SaveChangesAsync();
        }

        // افزودن هشدار به پایگاه داده
        public async Task AddAlertAsync(Alert alert)
        {
            _context.DbSetAlerts.Add(alert);
            await _context.SaveChangesAsync();
        }
    }
}
