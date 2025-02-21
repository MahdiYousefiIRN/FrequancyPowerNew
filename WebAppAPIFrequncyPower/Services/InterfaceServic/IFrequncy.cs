using WebAppAPIFrequncyPower.Model;

namespace WebAppAPIFrequncyPower.Services.InterfaceServic;

public interface IFrequncy
{
    Task<IEnumerable<PowerFrequencyData>> GetFrequncyDataTask();
    Task<PowerFrequencyData> GetFrequncytById(int id);

}