using Windows.ApplicationModel.Calls;
using BeFriend.Views;

namespace BeFriend.Services
{
    public sealed class MonitorClass
    {
        private bool _doesPhoneCallExist;

        public event HomePage.CallingInfoDelegate ActivePhoneCallStateChanged;

        private void MonitorCallState()
        {
            PhoneCallManager.CallStateChanged += (o, args) =>
            {
                _doesPhoneCallExist = PhoneCallManager.IsCallActive || PhoneCallManager.IsCallIncoming;
                if (ActivePhoneCallStateChanged != null)
                {
                    ActivePhoneCallStateChanged();
                }
            };
        }
    }
}