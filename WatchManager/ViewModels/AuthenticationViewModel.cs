using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WatchManager.Commands;
using WatchManager.Stores;

namespace WatchManager.ViewModels
{
    public class AuthenticationViewModel : BaseViewModel
    {
        private string _login;
        private string _password;

        public string Title { get; } = "WatchManager";
        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                AuthenticationCommand.Login = value;
                OnPropertyChanged(nameof(Login));
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                AuthenticationCommand.Password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public AuthenticationCommand AuthenticationCommand { get; }

        
        public AuthenticationViewModel(NavigationStore navigationStore)
        {
            AuthenticationCommand = new AuthenticationCommand(navigationStore, () => new WatchViewModel(navigationStore, Login));
        }
    }
}
