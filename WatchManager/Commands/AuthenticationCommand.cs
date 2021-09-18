using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using WatchManager.Models;
using WatchManager.StaticClasses;
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

        public override void Execute(object parameter)
        {
            BsonDocument account = DatabaseModel.GetAccount(Login);
            //AuthenticationViewModel authVM = (AuthenticationViewModel)_navigtationStore.CurrentViewModel;
            
            if (account == null)
            {
                MessageBox.Show("No such account");
            }
            else if (Password == account.GetValue("Password")) 
            {
                Task.Run(() => SaveUserInfoAsync(Login, Password));
                _navigtationStore.CurrentViewModel = _createViewModel();
            }
            else
            {
                MessageBox.Show("Wrong password");
            }
        }

        private async void SaveUserInfoAsync(string login, string password)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            using (FileStream fs = new FileStream("AuthorizationInfo.json", FileMode.OpenOrCreate))
            {
                // TODO: хэшировать значения
                AuthorizedUserModel userModel = new AuthorizedUserModel(login, password);
                await JsonSerializer.SerializeAsync<AuthorizedUserModel>(fs, userModel, options);
            }
        }
    }
}