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
        private const string DEFAULT_VALUE = "1";
        private const int SEASONS_LIMIT = 50;
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
            get => _titleName == null ? "" : _titleName;
            set
            {
                _titleName = value;
                _document.TitleName = value;
            }
        }
        public string TitleType
        {
            get => _titleType == null ? DEFAULT_TYPE_VALUE : _titleType;
            set
            {
                _titleType = value;
                _document.TitleType = value;
                ChangeFieldsVisibitity(value);
            }
        }
        public string SeasonsCount
        {
            get => _seasonsCount;
            set
            {
                if (IsEmpty(value))
                {
                    _seasonsCount = "";
                }
                else if (IsNumber(value) && Int32.Parse(value) <= SEASONS_LIMIT)
                {
                    _seasonsCount = value;
                    CorrectCurrentSeason(value);
                    GenerateSeasonsAndEpisodesContainer();
                    OnPropertyChanged(nameof(SeasonsCount));
                }
                else
                {
                    return;
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
                if (IsEmpty(value))
                {
                    _currentSeason = "";
                }
                else if (IsNumber(value) && Int32.Parse(value) <= Int32.Parse(SeasonsCount))
                {
                    _currentSeason = value;
                    _document.CurrentEpisode.SeasonNumber = value;
                    OnPropertyChanged(nameof(CurrentSeason));
                }
                else
                {
                    return;
                }
            }
        }
        public string CurrentEpisode
        {
            get => _currentEpisode;
            set
            {
                if (IsEmpty(value))
                {
                    _currentEpisode = "";
                }
                else if (IsNumber(value))
                {
                    _currentEpisode = value;
                    _document.CurrentEpisode.SeasonEpisodes =  value;
                    OnPropertyChanged(nameof(CurrentEpisode));
                }
                else
                {
                    return;
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
            SeasonsCount = DEFAULT_VALUE;
            CurrentSeason = DEFAULT_VALUE;
            CurrentEpisode = DEFAULT_VALUE;
            BackToWatchPageCommand = new BackToWatchPageCommand(navigationStore, () => new WatchViewModel(navigationStore, userLogin));
            AddDocumentCommand = new InsertNewDocumentCommand
                (
                    navigationStore,
                    () => new WatchViewModel(navigationStore, userLogin),
                    _document,
                    userLogin
                );
        }


        private void GenerateSeasonsAndEpisodesContainer()
        {
            SeasonsCollection = new ObservableCollection<SeasonModel>();
            int value = Int32.Parse(SeasonsCount);
            for (int i = 1; i <= value; i++)
            {
                SeasonsCollection.Add(new SeasonModel(i.ToString(), "0"));
            }
        }


        // Изменяет текущий сезон, если было изменено количество сезонов
        private void CorrectCurrentSeason(string value)
        {
            
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
