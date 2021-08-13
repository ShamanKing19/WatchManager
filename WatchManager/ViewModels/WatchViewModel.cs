using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WatchManager.Commands;
using WatchManager.Stores;

namespace WatchManager.ViewModels
{
    public class WatchViewModel : BaseViewModel
    {
        private bool _isFilmsSelected;
        private bool _isSerialsSelected;
        private bool _isAnimeSelected;

        public bool IsFilmsSelected
        {
            get => _isFilmsSelected;
            set
            {
                _isFilmsSelected = value;
                OnPropertyChanged(nameof(IsFilmsSelected));
            }
        }
        public bool IsSerialsSelected
        {
            get => _isSerialsSelected;
            set
            {
                _isSerialsSelected = value;
                OnPropertyChanged(nameof(IsSerialsSelected));
            }
        }
        public bool IsAnimeSelected
        {
            get => _isAnimeSelected;
            set
            {
                _isAnimeSelected = value;
                OnPropertyChanged(nameof(IsAnimeSelected));
            }
        }

        public ObservableCollection<string> RowCollection { get; set; }

        public ICommand SwitchToSettingsViewModelCommand{ get; }
        public ICommand SwitchToAddViewModelCommand { get; }
        public ICommand SwitchToEditViewModelCommand { get; }
        public ICommand DeleteRowCommand { get; }


        public WatchViewModel(NavigationStore navigationStore)
        {
            RowCollection = new() { 
                "Naruto: Shippuden",
                "JOJO: Bizzare Adventure",
                "Hunter x Hunter",
                "Интерстеллар",
                "Карты, деньги, два ствола",
                "Остров проклятых",
                "Гримм",
                "Доктор Хаус",
                "Локи"
            };
            SwitchToAddViewModelCommand = new SwitchToAddPageCommand(navigationStore, () => new AddRowViewModel(navigationStore));

        }



    }
}
