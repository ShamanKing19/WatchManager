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
        #region Constants
        private const string DEFAULT_VALUE = "1";
        private const int SEASONS_LIMIT = 50;
        #endregion

        #region Document Model Fields
        private string _titleName;
        private string _titleType;
        private string _seasonsCount;
        private ObservableCollection<SeasonModel> _seasonsCollection;
        private string _currentSeason;
        private string _currentEpisode;
        #endregion

        #region View Parameters Fields
        private List<string> _titleTypeList = new List<string> { "Film", "Serial", "Anime" };
        private Visibility _filmTypeVisibility;

        #endregion

        #region Document Model Properties
        public string TitleName
        {
            get => _titleName;
            set
            {
                _titleName = value;
            }
        }
        public string TitleType
        {
            get => _titleType;
            set
            {
                _titleType = value;
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
                    OnPropertyChanged(nameof(CurrentEpisode));
                }
                else
                {
                    return;
                }
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

        public AddDocumentViewModel(NavigationStore navigationStore)
        {
            SeasonsCount = DEFAULT_VALUE;
            CurrentSeason = DEFAULT_VALUE;
            CurrentEpisode = DEFAULT_VALUE;
            TitleType = TitleTypeList[0]; // default type
            BackToWatchPageCommand = new BackToWatchPageCommand(navigationStore, () => new WatchViewModel(navigationStore));
            // Сюда надо передать объект DocumentModel со всеми данными тайтла

            AddDocumentCommand = new AddDocumentCommand
                (
                    navigationStore,
                    () => new WatchViewModel(navigationStore)
                );
        }


        private DocumentModel CreateNewDocument()
        {
            if (TitleType == "Film")
            {
                return new DocumentModel(TitleName, TitleType, false);
            }
            else
            {
                return new DocumentModel(TitleName, TitleType, SeasonsCollection, new SeasonModel(CurrentSeason, CurrentEpisode ));
            }
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


        // TODO: Implement
        private void CorrectCurrentSeason(string value)
        {
            
        }


        private bool IsNumber(string value)
        {
            return Int32.TryParse(value, out int input);
        }


        private void ChangeFieldsVisibitity(string value)
        {
            if (value == "Film")
            {
                FilmTypeVisibility = Visibility.Hidden; 
            }
            else
            {
                FilmTypeVisibility = Visibility.Visible;
            }
        }


        private bool IsEmpty(string value)
        {
            return value.Length == 0;
        }
    }
}
