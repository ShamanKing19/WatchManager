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
        [BsonId]
        public ObjectId Id { get; set; }
        public BsonString TitleName { get; set; }
        public BsonString TitleType { get; set; }

        // Это должен быть словарь <string, string>
        [BsonIgnoreIfNull]
        public ObservableCollection<SeasonModel> Seasons { get; set; }

        [BsonIgnoreIfNull]
        public SeasonModel CurrentEpisode { get; set; } = new SeasonModel("0", "0");

        [BsonDefaultValue(false)]
        public bool Watched { get; set; }


        // Фильм
        public DocumentModel(BsonString titleName, BsonString titleType, bool watched)
        {
            TitleName = titleName;
            TitleType = titleType;
            Watched = watched;
        }


        // Сериал/аниме
        public DocumentModel(BsonString titleName, BsonString titleType, ObservableCollection<SeasonModel> seasons, SeasonModel currentEpisode)
        {
            TitleName = titleName;
            TitleType = titleType;
            Seasons = seasons;
            CurrentEpisode = currentEpisode;
        }


        public DocumentModel()
        {

        }


        public override string ToString()
        {
            if (TitleType == "Film")
            {
                return $"{TitleName}, {TitleType}, {Watched}";
            }
            else
            {
                return $"{TitleName}, {TitleType}, {Seasons}, {CurrentEpisode}, {Watched}";
            }
        }
    }
}
