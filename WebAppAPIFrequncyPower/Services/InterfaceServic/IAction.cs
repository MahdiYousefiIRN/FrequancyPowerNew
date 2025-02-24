using WebAppAPIFrequncyPower.Model;

namespace WebAppAPIFrequncyPower.Services.InterfaceServic
{
    public interface IAction
    {
        // متدی که اکشن باید اجرا کند
        Task ExecuteAsync(PowerFrequencyData frequencyData);
    }
}
