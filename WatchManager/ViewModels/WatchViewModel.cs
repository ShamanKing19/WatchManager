using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WatchManager.Commands;
using WatchManager.Models;
using WatchManager.Stores;

namespace WatchManager.ViewModels
{
    public class WatchViewModel : BaseViewModel
    {
        private bool _isFilmsSelected;
        private bool _isSerialsSelected;
        private bool _isAnimeSelected;
        private string _userLogin;

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

        public ObservableCollection<DocumentModel> RowCollection { get; set; } = new ObservableCollection<DocumentModel>();

        public ICommand SwitchToSettingsViewModelCommand{ get; }
        public ICommand SwitchToAddViewModelCommand { get; }
        public ICommand SwitchToEditViewModelCommand { get; }
        public ICommand DeleteRowCommand { get; }


        public WatchViewModel(NavigationStore navigationStore, string userLogin)
        {
            _userLogin = userLogin;
            SetRowCollectionAsync(userLogin); // TODO: Придумать это делать в отдельном потоке
            SwitchToAddViewModelCommand = new SwitchToAddPageCommand(navigationStore, () => new AddDocumentViewModel(navigationStore, userLogin));
        }


        private async void SetRowCollectionAsync(string userLogin)
        {
            
            List<BsonDocument> titleList = await DatabaseModel.GetListOfDocumentsFromCollectionAsync(userLogin);
            foreach (BsonDocument doc in titleList)
            {
                RowCollection.Add(BsonSerializer.Deserialize<DocumentModel>(doc));
            }
        }
    }
}
