using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchManager.Models
{
    public class AuthorizedUserModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public bool IsLogged { get; set; }

        public AuthorizedUserModel()
        {
            IsLogged = false;
        }

        public AuthorizedUserModel(string login, string password)
        {
            Login = login;
            Password = password;
            IsLogged = true;
        }
    }
}
