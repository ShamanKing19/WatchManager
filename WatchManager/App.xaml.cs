using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
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
                navigationStore.CurrentViewModel = userModel.IsLogged ? new WatchViewModel(navigationStore, userModel.Login) : new AuthenticationViewModel(navigationStore);
                MainWindow = new MainWindow()
                {
                    DataContext = new MainViewModel(navigationStore)
                };
                
                MainWindow.Show();


                base.OnStartup(e);
            }
        }
    }
}
