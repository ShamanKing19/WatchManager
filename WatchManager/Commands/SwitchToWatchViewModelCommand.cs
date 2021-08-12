using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WatchManager.Stores;
using WatchManager.ViewModels;

namespace WatchManager.Commands
{
    public class SwitchToWatchViewModelCommand : CommandBase
    {
        private readonly NavigationStore _navigtationStore;
        private readonly Func<BaseViewModel> _createViewModel;

        public SwitchToWatchViewModelCommand(NavigationStore navigtationStore, Func<BaseViewModel> createViewModel)
        {
            _navigtationStore = navigtationStore;
            _createViewModel = createViewModel;
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            AuthenticationViewModel authVM = (AuthenticationViewModel)_navigtationStore.CurrentViewModel;
            if (authVM.Login == authVM.Password) 
            {
                _navigtationStore.CurrentViewModel = _createViewModel();
            }
            else
            {
                MessageBox.Show("Wrong password");
            }
        }

    }
}