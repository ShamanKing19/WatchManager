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
        #region Document Model Fields
        private string _titleName;
        private string _titleType;
        private int _seasonsCount;
        private ObservableCollection<SeasonModel> _seasonsCollection;
        private int _currentSeason;
        private int _currentEpisode;
        #endregion

        #region View Parameters Fields
        private List<string> _titleTypeList = new List<string> { "Film", "Serial", "Anime" };
        private Visibility _seasonsTableVisibility;
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
                ChangeSeasonsTableVisibitity(value);
            }
        }
        public int SeasonsCount
        {
            get => _seasonsCount;
            set
            {
                if (value <= 100) 
                { 
                    _seasonsCount = value;
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
        public int CurrentSeason
        {
            get => _currentSeason;
            set
            {
                _currentSeason = value;
                OnPropertyChanged(nameof(CurrentSeason));
            }
        }
        public int CurrentEpisode
        {
            get => _currentEpisode;
            set
            {
                _currentEpisode = value;
                OnPropertyChanged(nameof(CurrentEpisode));
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
        public Visibility SeasonsTableVisibility
        {
            get => _seasonsTableVisibility;
            set
            {
                _seasonsTableVisibility = value;
                OnPropertyChanged(nameof(SeasonsTableVisibility));
            }
        }
        #endregion

        #region Commands
        public ICommand BackToWatchPageCommand { get; }
        public ICommand AddDocumentCommand { get; }
        #endregion 

        public AddDocumentViewModel(NavigationStore navigationStore)
        {
            SeasonsCount = 1;
            CurrentSeason = 1;
            CurrentEpisode = 1;
            TitleType = TitleTypeList[0]; // default type
            BackToWatchPageCommand = new BackToWatchPageCommand(navigationStore, () => new WatchViewModel(navigationStore));
            // Сюда надо передать объект DocumentModel со всеми данными тайтла
            AddDocumentCommand = new AddDocumentCommand
                (
                    navigationStore,
                    () => new WatchViewModel(navigationStore)
                );
        }


        private void GenerateSeasonsAndEpisodesContainer()
        {
            SeasonsCollection = new ObservableCollection<SeasonModel>();
            for (int i = 1; i <= SeasonsCount; i++)
            {
                SeasonsCollection.Add(new SeasonModel(i.ToString(), "0"));
            }
        }

        private void ChangeSeasonsTableVisibitity(string value)
        {
            if (value == "Film")
            {
                SeasonsTableVisibility = Visibility.Hidden;
            }
            else
            {
                SeasonsTableVisibility = Visibility.Visible;
            }
        }


        private bool IsNumber(string value)
        {
            return true;
        }
    }
}
