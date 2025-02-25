using Microsoft.AspNetCore.SignalR;
using WebAppAPIFrequncyPower.Model;

namespace WebAppAPIFrequncyPower.Services.SignalR
{
    public class FrequencyHub : Hub
    {
        public async Task SendFrequencyUpdate(PacketFrequencyMessage packet)
        {
            try
            {
                if (packet == null)
                {
                    throw new ArgumentNullException(nameof(packet), "PacketFrequencyMessage cannot be null.");
                }

                await Clients.All.SendAsync("ReceiveMessage", packet);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SendFrequencyUpdate: {ex.Message}");
            }
        }
    }
}
