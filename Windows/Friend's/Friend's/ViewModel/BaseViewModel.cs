using GalaSoft.MvvmLight;

namespace Friend_s.Portable.ViewModel
{
    public class BaseViewModel:ViewModelBase
    {
        public BaseViewModel()
        {
            if (IsInDesignMode)
                LoadDesignTimeData();
        }

        protected virtual void LoadDesignTimeData() {}
        private bool _isLoading;

        public virtual bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                RaisePropertyChanged();
            }
        }
    }
}
