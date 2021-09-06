using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchManager.Models;
using WatchManager.Stores;
using WatchManager.ViewModels;

namespace WatchManager.Commands
{
    public class RegisterNewAccountCommand : CommandBase
    {
        private NavigationStore _navigtationStore;
        private Func<BaseViewModel> _createViewModel;
        
        public AccountModel NewAccount;

        public RegisterNewAccountCommand(NavigationStore navigtationStore, Func<BaseViewModel> createViewModel, AccountModel account)
        {
            _navigtationStore = navigtationStore;
            _createViewModel = createViewModel;
            NewAccount = account;
        }
        public override void Execute(object parameter)
        {
            Task.Run(() => DatabaseModel.CreateAccountAsync(NewAccount.Login, NewAccount.Password, NewAccount.Email));
            _navigtationStore.CurrentViewModel = _createViewModel();
        }
    }
}
