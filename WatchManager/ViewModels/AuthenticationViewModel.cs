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
using WatchManager.StaticClasses;
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
                _login = HashFunction.GetHash(value);
                AuthenticationCommand.Login = HashFunction.GetHash(value);
                OnPropertyChanged(nameof(Login));
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = HashFunction.GetHash(value);
                AuthenticationCommand.Password = HashFunction.GetHash(value);
                OnPropertyChanged(nameof(Password));
            }
        }

        public AuthenticationCommand AuthenticationCommand { get; }
        public SwitchToRegistrationPageCommand RegistrationCommand { get; }
        
        public AuthenticationViewModel(NavigationStore navigationStore)
        {
            AuthenticationCommand = new AuthenticationCommand(navigationStore, () => new WatchViewModel(navigationStore, Login));
            RegistrationCommand = new SwitchToRegistrationPageCommand(navigationStore, () => new RegistrationViewModel(navigationStore));
        }
    }
}
