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
        private DocumentModel _selectedDocument;


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

        public DocumentModel SelectedDocument
        {
            get => _selectedDocument;
            set
            {
                _selectedDocument = value;
                SwitchToEditViewModelCommand.Document = value;
                DeleteRowCommand.Document = value;
            }
        }

        public ObservableCollection<DocumentModel> RowCollection { get; set; } = new ObservableCollection<DocumentModel>();

        public ICommand SwitchToSettingsViewModelCommand{ get; }
        public ICommand SwitchToAddViewModelCommand { get; }
        public SwitchToEditPageCommand SwitchToEditViewModelCommand { get; private set; }
        public DeleteDocumentCommand DeleteRowCommand { get; private set; }


        public WatchViewModel(NavigationStore navigationStore, string userLogin)
        {
            SetRowCollectionAsync(userLogin); // TODO: Придумать это делать асинхронно в отдельном потоке
            SwitchToAddViewModelCommand = new SwitchToAddPageCommand(navigationStore, () => new AddDocumentViewModel(navigationStore, userLogin));
            SwitchToEditViewModelCommand = new SwitchToEditPageCommand(navigationStore, () => new AddDocumentViewModel(navigationStore, userLogin, SelectedDocument), SelectedDocument);
            DeleteRowCommand = new DeleteDocumentCommand(userLogin, SelectedDocument, SetRowCollectionAsync);
        }


        private async void SetRowCollectionAsync(string userLogin)
        {
            RowCollection.Clear();
            List<BsonDocument> titleList = await DatabaseModel.GetListOfDocumentsFromCollectionAsync(userLogin);
            foreach (BsonDocument doc in titleList)
            {
                RowCollection.Add(BsonSerializer.Deserialize<DocumentModel>(doc));
            }
        }
    }
}
