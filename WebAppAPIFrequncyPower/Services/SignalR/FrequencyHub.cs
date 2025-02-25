using Microsoft.AspNetCore.SignalR;

namespace WebAppAPIFrequncyPower.Services.SignalR
{
    public class FrequencyHub : Hub
    {
        public async Task SendFrequencyUpdate(string message)
        {
            await Clients.All.SendAsync("ReceiveFrequencyUpdate", message);
        }
    }
}
