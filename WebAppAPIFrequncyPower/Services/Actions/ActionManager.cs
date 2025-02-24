using WebAppAPIFrequncyPower.Model;
using WebAppAPIFrequncyPower.Services.InterfaceServic;

namespace WebAppAPIFrequncyPower.Services.Actions
{
    public class ActionManager
    {
        private readonly IEnumerable<IAction> _actions;

        // تزریق اکشن‌ها از طریق DI
        public ActionManager(IEnumerable<IAction> actions)
        {
            _actions = actions;
        }

        // متد اجرایی برای اجرای تمامی اکشن‌ها
        public async Task ExecuteActionsAsync(PowerFrequencyData frequencyData)
        {
            foreach (var action in _actions)
            {
                await action.ExecuteAsync(frequencyData);
            }
        }
    }
}
