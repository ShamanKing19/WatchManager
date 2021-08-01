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
    public class AuthenticationViewModel : INotifyPropertyChanged
    {
        #region Реализация интерфейса INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        #endregion


        private string _login;
        private string _password;
        
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
            MessageBox.Show($"Login: {Login}, Password: {Password}");
        }


        public bool CanExecuteAuthenticateUser(object parameter)
        {
            // if (Login != null) return Login.Length
            // else return 0
            int inputLength = Login?.Length ?? 0;
            
            return inputLength > 0;
        }

    }
}
