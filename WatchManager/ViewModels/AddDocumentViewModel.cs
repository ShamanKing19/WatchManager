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
        private List<string> _titleTypeList;
        private Visibility _filmTypeVisibility;
        #endregion

        #region Constants
        private const int TITLE_LENGTH_LIMIT = 200;
        private const int SEASONS_LIMIT = 50;
        private const string DEFAULT_VALUE = "1";
        private readonly string DEFAULT_TYPE_VALUE;
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
                        SetSeasonsCollection();
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
        public ICommand AddDocumentCommand { get; }
        #endregion 

        public AddDocumentViewModel(NavigationStore navigationStore, string userLogin)
        {
            TitleTypeList = new List<string> { "Film", "Serial", "Anime" };
            DEFAULT_TYPE_VALUE = TitleTypeList[0];
            TitleType = DEFAULT_TYPE_VALUE;
            CurrentSeason = DEFAULT_VALUE;
            CurrentEpisode = DEFAULT_VALUE;
            SetSeasonsCollection();
            BackToWatchPageCommand = new BackToWatchPageCommand(navigationStore, () => new WatchViewModel(navigationStore, userLogin));
            AddDocumentCommand = new InsertNewDocumentCommand
                (
                    navigationStore,
                    () => new WatchViewModel(navigationStore, userLogin),
                    _document,
                    userLogin
                );
        }


        private Dictionary<string, string> GetDictFromCollection(ObservableCollection<SeasonModel> collection)
        {
            Dictionary<string, string> newDict = new();
            foreach (var season in collection)
            {
                newDict[season.SeasonNumber] = season.SeasonEpisodesCount;
            }
            return newDict;
        } 


        private void SetSeasonsCollection()
        {
            int value;
            bool isInt= int.TryParse(SeasonsCount, out value);
            
            if (isInt)
            {
                SeasonsCollection = new ObservableCollection<SeasonModel>();
                for (int i = 1; i <= value; i++)
                {
                    SeasonsCollection.Add(new SeasonModel(i.ToString(), "0"));
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


        private bool IsNumber(string value)
        {
            return Int32.TryParse(value, out int input);
        }


        private bool IsEmpty(string value)
        {
            return value.Length == 0;
        }
    }
}
