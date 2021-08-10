using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WatchManager.Stores;

namespace WatchManager.ViewModels
{
    public class WatchViewModel : BaseViewModel
    {
        public string Title { get; set; } = "WATCHWINDOW";

        public WatchViewModel(NavigationStore navigationStore)
        {
            
        }
    }
}
