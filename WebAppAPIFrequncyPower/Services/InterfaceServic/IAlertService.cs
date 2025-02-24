namespace WebAppAPIFrequncyPower.Services.InterfaceServic
{
    public interface IAlertService
    {
        Task SendAlertAsync(string message);

    }
}
