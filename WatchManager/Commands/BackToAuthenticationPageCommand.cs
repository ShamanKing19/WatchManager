using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using WatchManager.Models;
using WatchManager.Stores;
using WatchManager.ViewModels;

namespace WatchManager.Commands
{
    public class BackToAuthenticationPageCommand : CommandBase
    {
        private NavigationStore _navigtationStore;
        private Func<BaseViewModel> _createViewModel;

        public BackToAuthenticationPageCommand(NavigationStore navigtationStore, Func<BaseViewModel> createViewModel)
        {
            _navigtationStore = navigtationStore;
            _createViewModel = createViewModel;
        }
        public override void Execute(object parameter)
        {
            Task.Run(() => ClearAuthorizedInfoFileAsync());
            _navigtationStore.CurrentViewModel = _createViewModel();
        }


        private async void ClearAuthorizedInfoFileAsync()
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            using (FileStream fs = new FileStream("AuthorizationInfo.json", FileMode.Create))
            {
                AuthorizedUserModel userModel = new AuthorizedUserModel();
                await JsonSerializer.SerializeAsync<AuthorizedUserModel>(fs, userModel, options);
            }
        }
    }
}
