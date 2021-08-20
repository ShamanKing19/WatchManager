using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
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

        [BsonIgnoreIfNull]
        public Dictionary<BsonString, BsonInt32> Seasons { get; set; }

        [BsonIgnoreIfNull]
        public Dictionary<BsonString, BsonInt32> CurrentEpisode { get; set; }

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
        public DocumentModel(BsonString titleName, BsonString titleType, Dictionary<BsonString, BsonInt32> seasons, Dictionary<BsonString, BsonInt32> currentEpisode)
        {
            TitleName = titleName;
            TitleType = titleType;
            Seasons = seasons;
            CurrentEpisode = currentEpisode;
        }
    }
}
