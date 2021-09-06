using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchManager.ViewModels
{
    public class RegistrationViewModel : BaseViewModel
    {
        private string _login;
        private string _password;
        private string _email;

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
        public string Email
        {
            get => _email;
            set => _email = value;
        }
    }
}
