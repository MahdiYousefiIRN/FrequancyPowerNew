using Microsoft.EntityFrameworkCore;
using WebAppAPIFrequncyPower.DataContext;
using WebAppAPIFrequncyPower.Model;
using WebAppAPIFrequncyPower.Services.InterfaceServic;

namespace WebAppAPIFrequncyPower.Services.CLassServic;

public class DbFrequncy : IFrequncy
{
    private readonly PowerGridContext _powerGridContext;

    public DbFrequncy(PowerGridContext powerGridContext)
    {
        _powerGridContext = powerGridContext;
    }
   
    public async Task<IEnumerable<PowerFrequencyData>> GetFrequncyDataTask()
    {
        return await _powerGridContext.DbSetFrequency .ToListAsync();

    }

    public async Task<PowerFrequencyData> GetFrequncytById(int id)
    {
        var product = await _powerGridContext.DbSetFrequency. SingleOrDefaultAsync(x => x.Id == id); 
        return product;
    }
}