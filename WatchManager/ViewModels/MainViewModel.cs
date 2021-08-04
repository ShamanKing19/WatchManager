using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WatchManager.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        //private BaseViewModel startupViewModel;
        private BaseViewModel _currentViewModel;

        public BaseViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        public MainViewModel(BaseViewModel vm)
        {
            CurrentViewModel = vm;
        }

        public MainViewModel() : base()
        {

        }

        protected void ChangeViewModel(BaseViewModel vm)
        {
            CurrentViewModel = vm;
            MessageBox.Show($"View should be changed!\nCurrent VM: {CurrentViewModel}");
        }
    }
}
