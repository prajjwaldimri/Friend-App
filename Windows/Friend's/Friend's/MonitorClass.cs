using Windows.ApplicationModel.Calls;

namespace Friend_s
{
    public sealed class MonitorClass
    {
        private bool doesPhoneCallExist;

        public MonitorClass()
        {
        }

        public event MainPage.CallingInfoDelegate ActivePhoneCallStateChanged;

        private void MonitorCallState()
        {
            PhoneCallManager.CallStateChanged += (o, args) =>
            {
                doesPhoneCallExist = PhoneCallManager.IsCallActive || PhoneCallManager.IsCallIncoming;
                if (ActivePhoneCallStateChanged != null)
                {
                    ActivePhoneCallStateChanged();
                }
            };
        }
    }
}