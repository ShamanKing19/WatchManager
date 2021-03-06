using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchManager.Commands;
using WatchManager.Models;
using WatchManager.StaticClasses;
using WatchManager.Stores;

namespace WatchManager.ViewModels
{
    public class RegistrationViewModel : BaseViewModel
    {
        private string _login;
        private string _password;
        private string _email;

        public string RegistrationTitle { get; } = "Register your new account!";
        public string Login
        {
            get => _login;
            set
            {
                _login = HashFunction.GetHash(value);
                RegisterAccountCommand.NewAccount.Login = HashFunction.GetHash(value);
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = HashFunction.GetHash(value);
                RegisterAccountCommand.NewAccount.Password = HashFunction.GetHash(value);
            }
        }
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                RegisterAccountCommand.NewAccount.Email = value;
            }
        }
        public BackToAuthenticationPageCommand BackToAuthenticationPageCommand { get; }
        public RegisterNewAccountCommand RegisterAccountCommand { get; set; }

        public RegistrationViewModel(NavigationStore navigationStore)
        {
            BackToAuthenticationPageCommand = new BackToAuthenticationPageCommand(navigationStore, () => new AuthenticationViewModel(navigationStore));
            RegisterAccountCommand = new RegisterNewAccountCommand(navigationStore, () => new AuthenticationViewModel(navigationStore), new AccountModel(Login, Password, Email));
        }


    }
}
