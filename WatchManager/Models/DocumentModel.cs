using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchManager.Models
{
    public class DocumentModel
    {       
        // По идее этим записям ID не нужен, вместо них используется логин пользователя
        // Для разрешения одинаковых логинов можно будет попробовать сделать
        // id названием коллекции, хотя не факт что сработает, потому что набор символов одинаковый
        [BsonId]
        public ObjectId Id { get; set; }
        public string TitleName { get; set; }
        public string TitleType { get; set; }


        [BsonIgnoreIfNull]
        [BsonElement("Seasons")]
        public ObservableCollection<SeasonModel> Seasons { get; set; }
        

        [BsonIgnoreIfNull]
        public SeasonModel CurrentEpisode { get; set; }


        [BsonDefaultValue(false)]
        public bool Watched { get; set; }

        #region Contstructors
        // Фильм
        public DocumentModel(string titleName, string titleType, bool watched)
        {
            TitleName = titleName;
            TitleType = titleType;
            Watched = watched;
        }


        // Сериал/аниме
        public DocumentModel(string titleName, string titleType, ObservableCollection<SeasonModel> seasons, SeasonModel currentEpisode)
        {
            TitleName = titleName;
            TitleType = titleType;
            Seasons = seasons;
            CurrentEpisode = currentEpisode;
        }


        // Нужен для создания пустого объекта, через которой
        // передаются данные для записи в команду
        public DocumentModel()
        {

        }
        #endregion


        public void WatchEpisode()
        {
            if (TitleType == "Film")
            {
                Watched = true;
            }
            else
            {
                if (IsLastEpisode())
                {
                    SetNextSeason();
                }
                else
                {
                    SetNextEpisode();
                }
            }
        }
        private void SetNextEpisode()
        {
            CurrentEpisode.SeasonEpisodesCount = (int.Parse(CurrentEpisode.SeasonEpisodesCount) + 1).ToString();
            Watched = false;
        }

        private bool IsLastEpisode()
        {
            int currentEpisode = int.Parse(CurrentEpisode.SeasonEpisodesCount);
            int currentSeason = int.Parse(CurrentEpisode.SeasonNumber);

            return currentEpisode + 1 > int.Parse(Seasons[currentSeason - 1].SeasonEpisodesCount);
        }

        private void SetNextSeason()
        {
            if (!IsLastSeasonsEpisode() && !IsLastSeason())
            {
                CurrentEpisode.SeasonNumber = (int.Parse(CurrentEpisode.SeasonNumber) + 1).ToString();
                CurrentEpisode.SeasonEpisodesCount = "1";
                Watched = false;
            }
            else
            {
                Watched = true;
            }
        }

        private bool IsLastSeason()
        {
            bool lastSeason = int.Parse(CurrentEpisode.SeasonNumber) == Seasons.Count;
            return lastSeason;
        }

        private bool IsLastSeasonsEpisode()
        {
            int currentEpisode = int.Parse(CurrentEpisode.SeasonEpisodesCount);
            int lastSeasonsEpisode = int.Parse(Seasons[Seasons.Count - 1].SeasonEpisodesCount);
            return currentEpisode == lastSeasonsEpisode && IsLastSeason();
        }


    }
}
