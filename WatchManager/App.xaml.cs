using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using WatchManager.Models;
using WatchManager.Stores;
using WatchManager.ViewModels;

namespace WatchManager
{
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            using (FileStream fs = new FileStream("AuthorizationInfo.json", FileMode.OpenOrCreate))
            {
                AuthorizedUserModel userModel = new AuthorizedUserModel();
                if (fs.Length > 0)
                {
                    userModel = await JsonSerializer.DeserializeAsync<AuthorizedUserModel>(fs);
                }


                NavigationStore navigationStore = new();
                
                if (userModel.IsLogged)
                {
                    navigationStore.CurrentViewModel = new WatchViewModel(navigationStore, userModel.Login);
                }
                else
                {
                    navigationStore.CurrentViewModel = new AuthenticationViewModel(navigationStore);
                }

                MainWindow = new MainWindow() { DataContext = new MainViewModel(navigationStore) };
                MainWindow.Show();
                base.OnStartup(e);
            }
        }
    }
}
