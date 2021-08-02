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

namespace WatchManager.ViewModels
{
    public class AuthenticationViewModel : BaseViewModel
    {
        private BaseViewModel _selectedViewModel;
        private string _login;
        private string _password;
        

        public BaseViewModel SelectedViewModel
        {
            get => _selectedViewModel;
            set
            {
                _selectedViewModel = value;
                OnPropertyChanged(nameof(SelectedViewModel));
            }
        }
        public string Login
        {
            get
            {
                return _login;
            }
            set
            {
                _login = value;
                OnPropertyChanged(nameof(Login));
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public string Title { get; } = "WatchManager";


        public RelayCommand AuthenticationCommand { get; set; }

        public AuthenticationViewModel()
        {
            AuthenticationCommand = new RelayCommand
            (
                ExecuteUserAuthentication,
                CanExecuteAuthenticateUser
            );
        }


        public void ExecuteUserAuthentication(object parameter)
        {
            // Затычка для авторизации
            if (Login == Password)
            {
                ChangeViewModel(new WatchViewModel());
                MessageBox.Show($"Hello {Login}!");
            }
            else
            {
                Password = "";
                MessageBox.Show($"Wrong password");
            }
        }


        public bool CanExecuteAuthenticateUser(object parameter)
        {
            // if (Login != null) return Login.Length
            // else return 0
            int inputLength = Login?.Length ?? 0;
            int passwordLength = Password?.Length ?? 0;

            return (inputLength > 0) && (passwordLength > 0);
        }

        private void ChangeViewModel(BaseViewModel viewmodel)
        {
            SelectedViewModel = viewmodel;
        }
    }
}
