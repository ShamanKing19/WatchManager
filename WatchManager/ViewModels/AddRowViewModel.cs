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
    public class AddRowViewModel : BaseViewModel
    {
        private string _titleName;
        private string _titleType;
        private int _seasonsCount;
        private ObservableCollection<SeasonModel> _seasonsCollection;
        private int _currentSeason;
        private int _currentEpisode;

        public string TitleName
        {
            get => _titleName;
            set => _titleName = value;
        }
        public string TitleType
        {
            get => _titleType;
            set => _titleType = value;
        }
        public int SeasonsCount
        {
            get => _seasonsCount;
            set
            {
                if (value < 40) 
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
            set => _currentSeason = value;
        }
        public int CurrentEpisode
        {
            get => _currentEpisode;
            set => _currentEpisode = value;
        }



        public ICommand BackToWatchPageCommand { get; }
        public ICommand AddDocumentCommand { get; }

        public AddRowViewModel(NavigationStore navigationStore)
        {
            SeasonsCount = 1;
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
    }
}
