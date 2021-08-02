using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchManager.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private BaseViewModel _startupViewModel;
        public BaseViewModel StartupViewModel
        {
            get => _startupViewModel;
            set
            {
                _startupViewModel = value;
                OnPropertyChanged(nameof(StartupViewModel));
            }
        }

        public MainWindowViewModel()
        {
            StartupViewModel = new AuthenticationViewModel();
        }
    }
}
