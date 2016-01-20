using Friend_s.Model;

namespace Friend_s.ViewModel
{
    public class CallandSettingsPageViewModel : BaseViewModel
    {
        private TestItem _selectedItem;

        public TestItem SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                RaisePropertyChanged();
            }
        }

        protected override void LoadDesignTimeData()
        {
            base.LoadDesignTimeData();

            SelectedItem = new TestItem()
            {
                Title = "Design Time Selected Item",
                Subtitle = "Design subtitle",
                HexColor = "#333333"
            };
        }
    }
}