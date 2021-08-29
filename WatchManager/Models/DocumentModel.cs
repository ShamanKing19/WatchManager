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
        private Dictionary<string, string> _seasonsDict;
        
        // По идее этим записям ID не нужен, вместо них используется логин пользователя
        // Для разрешения одинаковых логинов можно будет попробовать сделать
        // id названием коллекции, хотя не факт что сработает, потому что набор символов одинаковый
        [BsonId]
        public ObjectId Id { get; set; }
        public string TitleName { get; set; }
        public string TitleType { get; set; }


        [BsonIgnore]
        public ObservableCollection<SeasonModel> Seasons { get; set; }
        

        [BsonIgnoreIfNull][BsonElement("Seasons")]
        public Dictionary<string, string> SeasonsDict
        {
            get
            {
                if (Seasons != null)
                {
                    _seasonsDict = new Dictionary<string, string>();
                    foreach (var season in Seasons)
                    {
                        _seasonsDict[season.SeasonNumber] = season.SeasonEpisodesCount;
                    }
                }
                return _seasonsDict;

            }
            set
            {
                _seasonsDict = value;
            }
        }


        [BsonIgnore]
        public SeasonModel CurrentEpisode { get; set; }
        

        [BsonIgnoreIfNull][BsonElement("CurrentEpisode")]
        public Dictionary<string, string> CurrentEpisodeDict
        {
            get
            {
                if (CurrentEpisode != null)
                {
                    return new Dictionary<string, string>
                    {
                        {CurrentEpisode.SeasonNumber, CurrentEpisode.SeasonEpisodesCount }
                    };
                }
                return null;
            }
        }


        [BsonDefaultValue(false)]
        public bool Watched { get; set; }


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
    }
}
