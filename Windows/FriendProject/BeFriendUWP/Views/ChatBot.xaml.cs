using System;
using System.Diagnostics;
using System.IO;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Syn.Bot;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BeFriend.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChatBot : Page
    {
        private readonly SynBot _chatbot;
        public ChatBot()
        {
            this.InitializeComponent();
            _chatbot = new SynBot();
            _chatbot.PackageManager.LoadFromString(File.ReadAllText("BeFriendBot.simlpk"));
        }




        private void SendButton_OnClick(object sender, RoutedEventArgs e)
        {
            ChatBotConnector();
        }

        private void InputText_OnKeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key != VirtualKey.Enter) return;
            try
            {
                ChatBotConnector();
                e.Handled = true; LoseFocus(sender);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }

        private void ChatBotConnector()
        {
            var result = _chatbot.Chat(InputText.Text);
            //OutputText = string.Format("User: {0}\nBot: {1}\n{2}", InputText.Text, result.BotMessage);
                
            InputText.Text = string.Empty;
        }

        /// <summary>
        /// Makes virtual keyboard disappear
        /// </summary>
        /// <param name="sender"></param>
        private void LoseFocus(object sender)
        {
            var control = sender as Control;
            var isTabStop = control.IsTabStop;
            control.IsTabStop = false;
            control.IsEnabled = false;
            control.IsEnabled = true;
            control.IsTabStop = isTabStop;
        }
    }
}
