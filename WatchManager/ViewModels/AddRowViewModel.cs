using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WatchManager.Commands;
using WatchManager.Stores;

namespace WatchManager.ViewModels
{
    public class AddRowViewModel : BaseViewModel
    {
        public ICommand BackToWatchPage { get; }
        public AddRowViewModel(NavigationStore navigationStore)
        {
            BackToWatchPage = new BackToWatchPageCommand(navigationStore, () => new WatchViewModel(navigationStore));
        }
    }
}
