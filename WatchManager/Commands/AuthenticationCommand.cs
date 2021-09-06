using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WatchManager.Models;
using WatchManager.Stores;
using WatchManager.ViewModels;

namespace WatchManager.Commands
{
    public class AuthenticationCommand : CommandBase
    {
        private readonly NavigationStore _navigtationStore;
        private readonly Func<BaseViewModel> _createViewModel;
        private string _login;
        private string _password;
        private string collectionName = "Accounts";

        public string Login
        {
            get => _login;
            set => _login = value;
        }
        public string Password
        {
            get => _password;
            set => _password = value;
        }
        public AuthenticationCommand(NavigationStore navigtationStore, Func<BaseViewModel> createViewModel)
        {
            _navigtationStore = navigtationStore;
            _createViewModel = createViewModel;
        }

        public override bool CanExecute(object parameter)
        { 
            return true;
        }

        public override  void Execute(object parameter)
        {
            BsonDocument account = DatabaseModel.GetAccount(Login);
            AuthenticationViewModel authVM = (AuthenticationViewModel)_navigtationStore.CurrentViewModel;
            
            if (account == null)
            {
                MessageBox.Show("No such account");
            }
            else if (authVM.Password == account.GetValue("Password")) 
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