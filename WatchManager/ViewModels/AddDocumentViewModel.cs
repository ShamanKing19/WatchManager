using MongoDB.Bson;
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
    public class AddDocumentViewModel : BaseViewModel
    {
        #region View Parameters Fields
        private List<string> _titleTypeList = new List<string> { "Film", "Serial", "Anime" };
        private Visibility _filmTypeVisibility;
        #endregion

        #region Constants
        private const int TITLE_LENGTH_LIMIT = 200;
        private const int SEASONS_LIMIT = 50;
        private const string DEFAULT_VALUE = "1";
        #endregion

        #region Document Model Private Fields
        private string _titleName;
        private string _titleType;
        private string _seasonsCount;
        private ObservableCollection<SeasonModel> _seasonsCollection;
        private string _currentSeason;
        private string _currentEpisode;
        private bool _watched;
        private DocumentModel _document = new DocumentModel();
        #endregion

        #region Document Model Properties
        public string TitleName
        {
            get => _titleName;
            set
            {
                if (value.Length <= TITLE_LENGTH_LIMIT)
                {
                    _titleName = value;
                    _document.TitleName = value;
                }
            }
        }
        public string TitleType
        {
            get => _titleType;
            set
            {
                _titleType = value;
                _document.TitleType = value;
                ChangeFieldsVisibitity(value);
                SetCurrentSeason(value);
            }
        }
        public string SeasonsCount
        {
            get => _seasonsCount;
            set
            {
                int.TryParse(value, out int intValue);
                if (IsValid(value) && intValue <= SEASONS_LIMIT)
                {
                    _seasonsCount = value;
                    if (TitleType != "Film")
                    {
                        CorrectCurrentSeason(value);
                        SetEmptySeasonsCollection();
                    }
                    OnPropertyChanged(nameof(SeasonsCount));
                }
            }
        }
        public ObservableCollection<SeasonModel> SeasonsCollection
        {
            get => _seasonsCollection;
            set
            {
                _seasonsCollection = value;
                _document.Seasons = value;
                OnPropertyChanged(nameof(SeasonsCollection));
            }
        }
        public string CurrentSeason
        {
            get => _currentSeason;
            set
            {
                if (IsValid(value))
                {
                    _currentSeason = value;
                    // TODO: Сделать это красивее
                    if (TitleType != "Film")
                    {
                        _document.CurrentEpisode.SeasonNumber = value;
                    }
                    OnPropertyChanged(nameof(CurrentSeason));
                }
            }
        }
        public string CurrentEpisode
        {
            get => _currentEpisode;
            set
            {
                if (IsValid(value))
                {
                    _currentEpisode = value;
                    // TODO: Сделать это красивее
                    if (TitleType != "Film")
                    {
                        _document.CurrentEpisode.SeasonEpisodesCount = value;
                    }
                    OnPropertyChanged(nameof(CurrentEpisode));
                }
            }
        }
        public bool Watched
        {
            get => _watched;
            set
            {
                _watched = value;
                _document.Watched = value;
            }
        }
        #endregion

        #region View Parameters Properties
        public List<string> TitleTypeList
        {
            get => _titleTypeList;
            set
            {
                _titleTypeList = value;
                OnPropertyChanged(nameof(TitleType));
            }
        }
        public Visibility FilmTypeVisibility
        {
            get => _filmTypeVisibility;
            set
            {
                _filmTypeVisibility = value;
                OnPropertyChanged(nameof(FilmTypeVisibility));
            }
        }

        #endregion

        #region Commands
        public ICommand BackToWatchPageCommand { get; }
        public ICommand InsertDocumentCommand { get; }
        #endregion 


        // Add case
        public AddDocumentViewModel(NavigationStore navigationStore, string userLogin)
        {
            SetFieldsToAddDocument();
            BackToWatchPageCommand = new BackToWatchPageCommand(navigationStore, () => new WatchViewModel(navigationStore, userLogin));
            InsertDocumentCommand = new InsertNewDocumentCommand
                (
                    navigationStore,
                    () => new WatchViewModel(navigationStore, userLogin),
                    userLogin,
                    _document                    
                );
        }

        // Edit case
        public AddDocumentViewModel(NavigationStore navigationStore, string userLogin, DocumentModel oldDocument)
        {
            SetFieldsToEditDocument(oldDocument);
            BackToWatchPageCommand = new BackToWatchPageCommand(navigationStore, () => new WatchViewModel(navigationStore, userLogin));
            InsertDocumentCommand = new InsertNewDocumentCommand
                (
                    navigationStore,
                    () => new WatchViewModel(navigationStore, userLogin),
                    userLogin,
                    oldDocument.ToBsonDocument(),
                    _document
                );
        }


        private void SetFieldsToEditDocument(DocumentModel document)
        {
            TitleName = document.TitleName;
            TitleType = document.TitleType;

            if (document.TitleType != TitleTypeList[0])
            {
                CurrentSeason = document.CurrentEpisode.SeasonNumber;
                CurrentEpisode = document.CurrentEpisode.SeasonEpisodesCount;
                SeasonsCount = document.Seasons.Count.ToString();
                SeasonsCollection = document.Seasons;
            }
            Watched = document.Watched;
        }


        private void SetFieldsToAddDocument()
        {
            TitleType = TitleTypeList[0];
            CurrentSeason = DEFAULT_VALUE;
            CurrentEpisode = DEFAULT_VALUE;
            SetEmptySeasonsCollection();
        }


        private void SetEmptySeasonsCollection()
        {
            bool isInt= int.TryParse(SeasonsCount, out int value);
            if (isInt)
            {
                SeasonsCollection = new ObservableCollection<SeasonModel>();
                for (int i = 1; i <= value; i++)
                {
                    SeasonsCollection.Add(new SeasonModel(i.ToString(), ""));
                }
            }
        }


        private void SetCurrentSeason(string titleType)
        {
            if (TitleType == "Film")
            {
                _document.CurrentEpisode = null;
                _document.Seasons = null;
            }
            else
            {
                _document.CurrentEpisode = new SeasonModel("1", "1");
            }
        }


        // Изменяет текущий сезон, если было изменено количество сезонов
        private void CorrectCurrentSeason(string value)
        {
            
        }


        private bool IsValid(string val)
        {
            if (val.Length == 0)
            {
                return true;
            }
            else
            {
                int value;
                bool isInt = int.TryParse(val, out value);

                return isInt;
            }
            
        }

        private void ChangeFieldsVisibitity(string value)
        {
            if (value == "Film")
            {
                FilmTypeVisibility = Visibility.Collapsed; 
            }
            else
            {
                FilmTypeVisibility = Visibility.Visible;
            }
        }
    }
}
