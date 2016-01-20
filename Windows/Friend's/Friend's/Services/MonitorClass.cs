using Windows.ApplicationModel.Calls;
using Friend_s.Views;

namespace Friend_s.Services
{
    public sealed class MonitorClass
    {
        private bool _doesPhoneCallExist;

        public MonitorClass()
        {
        }

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