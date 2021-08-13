using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchManager.Stores;
using WatchManager.ViewModels;

namespace WatchManager.Commands
{
    public class BackToWatchPageCommand : CommandBase
    {

        private NavigationStore _navigtationStore;
        private Func<BaseViewModel> _createViewModel;

        public BackToWatchPageCommand(NavigationStore navigtationStore, Func<BaseViewModel> createViewModel)
        {
            _navigtationStore = navigtationStore;
            _createViewModel = createViewModel;
        }
        public override void Execute(object parameter)
        {
            _navigtationStore.CurrentViewModel = _createViewModel();
        }
    }
}
