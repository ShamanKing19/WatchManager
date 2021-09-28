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
        private const bool _defaultCheckbockState = false;

        #region Private fields
        private string _userLogin;
        private bool _isFilmsSelected = _defaultCheckbockState;
        private bool _isSerialsSelected = _defaultCheckbockState;
        private bool _isAnimeSelected = _defaultCheckbockState;
        private DocumentModel _selectedDocument;
        #endregion

        #region Properties
        public bool IsFilmsSelected
        {
            get => _isFilmsSelected;
            set
            {
                _isFilmsSelected = value;
                OnPropertyChanged(nameof(IsFilmsSelected));
                SetRowCollectionAsync(_userLogin);
            }
        }
        public bool IsSerialsSelected
        {
            get => _isSerialsSelected;
            set
            {
                _isSerialsSelected = value;
                OnPropertyChanged(nameof(IsSerialsSelected));
                SetRowCollectionAsync(_userLogin);

            }
        }
        public bool IsAnimeSelected
        {
            get => _isAnimeSelected;
            set
            {
                _isAnimeSelected = value;
                OnPropertyChanged(nameof(IsAnimeSelected));
                SetRowCollectionAsync(_userLogin);

            }
        }
        public DocumentModel SelectedDocument
        {
            get => _selectedDocument;
            set
            {
                _selectedDocument = value;
                SwitchToEditViewModelCommand.Document = value;
                WatchEpisodeCommand.Document = _selectedDocument;
                WatchEpisodeCommand.Collection = RowCollection;
                OnPropertyChanged(nameof(SelectedDocument));
            }
        }

        public ObservableCollection<DocumentModel> RowCollection { get; set; } = new ObservableCollection<DocumentModel>();
        #endregion

        #region Commands
        public ICommand SwitchToSettingsViewModelCommand{ get; }
        public ICommand SwitchToAddViewModelCommand { get; }
        public SwitchToEditPageCommand SwitchToEditViewModelCommand { get; private set; }
        public WatchCommand WatchEpisodeCommand { get; set; }
        public BackToAuthenticationPageCommand BackToAuthenticationPageCommand { get; set; }
        #endregion

        public WatchViewModel(NavigationStore navigationStore, string userLogin)
        {
            _userLogin = userLogin;
            SetRowCollectionAsync(userLogin); // TODO: Придумать это делать асинхронно в отдельном потоке
            SwitchToAddViewModelCommand = new SwitchToAddPageCommand(navigationStore, () => new AddDocumentViewModel(navigationStore, userLogin));
            SwitchToEditViewModelCommand = new SwitchToEditPageCommand(navigationStore, () => new AddDocumentViewModel(navigationStore, userLogin, SelectedDocument, SetRowCollectionAsync), SelectedDocument);
            WatchEpisodeCommand = new WatchCommand(userLogin);
            BackToAuthenticationPageCommand = new BackToAuthenticationPageCommand(navigationStore, () => new AuthenticationViewModel(navigationStore));
        }



        // При нажатии на фильтр будет показываться только то, что выбрано
        // Новый функционал
        private async void SetRowCollectionAsync(string userLogin)
        {
            RowCollection.Clear();
            List<BsonDocument> titleList = await DatabaseModel.GetListOfDocumentsFromCollectionAsync(userLogin);
            foreach (BsonDocument bsonDoc in titleList)
            {
                DocumentModel document = BsonSerializer.Deserialize<DocumentModel>(bsonDoc);
                if (!(IsFilmsSelected || IsSerialsSelected || IsAnimeSelected))
                {
                    RowCollection.Add(document);
                }
                else if ((document.TitleType == "Film" && IsFilmsSelected) || (document.TitleType =="Serial" && IsSerialsSelected) || (document.TitleType == "Anime" && IsAnimeSelected))
                {
                    RowCollection.Add(document);
                }
            }
        }


        // Выбранная категория перестаёт отображаться
        // Старый функционал
        private async void SetRowCollectionAsyncOld(string userLogin)
        {
            RowCollection.Clear();
            List<BsonDocument> titleList = await DatabaseModel.GetListOfDocumentsFromCollectionAsync(userLogin);
            foreach (BsonDocument bsonDoc in titleList)
            {
                DocumentModel document = BsonSerializer.Deserialize<DocumentModel>(bsonDoc);
                if ((document.TitleType == "Film" && IsFilmsSelected) || (document.TitleType == "Serial" && IsSerialsSelected) || (document.TitleType == "Anime" && IsAnimeSelected))
                {
                    RowCollection.Add(document);
                }
            }
        }
    }
}
