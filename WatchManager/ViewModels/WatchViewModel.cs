using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WatchManager.Stores;

namespace WatchManager.ViewModels
{
    public class WatchViewModel : BaseViewModel
    {
        public string TestColumn1 { get; } = "Column 1";
        public string TestColumn2 { get; } = "Column 2";
        public ObservableCollection<string> TitleCollection { get; set; }

        public WatchViewModel(NavigationStore navigationStore)
        {
            TitleCollection = new() { "Naruto: Shippuden", "JOJO: Bizzare Adventure", "Hunter x Hunter" };
        }

    }
}
