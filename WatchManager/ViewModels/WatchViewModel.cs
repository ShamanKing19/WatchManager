using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WatchManager.ViewModels
{
    public class WatchViewModel : BaseViewModel
    {
        public string Title { get; set; } = "WATCHWINDOW";

        public WatchViewModel()
        {
            //MessageBox.Show("This is the constructor of WatchViewModel!");
        }
    }
}
