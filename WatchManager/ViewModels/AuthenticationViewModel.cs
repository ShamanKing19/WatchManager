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
using WatchManager.Stores;

namespace WatchManager.ViewModels
{
    public class AuthenticationViewModel : BaseViewModel
    {
        public string Title { get; } = "WatchManager";
        public string Login { get; set; }
        public string Password { get; set; }

        public ICommand SwitchViewModelCommand { get; }
        public ICommand AuthenticationCommand{ get; }


        public AuthenticationViewModel(NavigationStore navigationStore)
        {
            SwitchViewModelCommand = new SwitchToWatchViewModelCommand(navigationStore, () => new WatchViewModel(navigationStore));
        }
    }
}
