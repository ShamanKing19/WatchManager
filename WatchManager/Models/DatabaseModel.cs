using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchManager.Models
{
    class DatabaseModel
    {
        readonly string connectionString = "mongodb://localhost";
        readonly string databaseName = "EpisodeManager";
        readonly string accountsCollectionName = "Accounts";
        readonly string userdataCollectionName = "testuserData";



        // Test
        private void CreateFilmDocument()
        {
            DocumentModel user = new("Pulp Fiction", "Film", false);
            BsonDocument doc = user.ToBsonDocument();
            InsertDocumentIntoCollectionAsync(doc, userdataCollectionName).Wait();
        }


        // Test
        private void CreateAnimeDocument()
        {
            Dictionary<BsonString, BsonInt32> seasons = new Dictionary<BsonString, BsonInt32>()
            {
                {"1", 220},
                {"2", 500}
            };

            Dictionary<BsonString, BsonInt32> currentEpisode = new Dictionary<BsonString, BsonInt32>()
            {
                {"Season", 2},
                {"Episode", 358 }

            };

            DocumentModel user = new("Naruto", "Anime", seasons, currentEpisode);
            BsonDocument doc = user.ToBsonDocument();
            InsertDocumentIntoCollectionAsync(doc, userdataCollectionName).Wait();

        }

        // Test
        private void CreateSerialDocument()
        {
            Dictionary<BsonString, BsonInt32> seasons = new Dictionary<BsonString, BsonInt32>()
            {
                {"1", 24},
                {"2",24},
                {"3", 24},
                {"4", 24},
                {"5", 24},
                {"6", 24},
            };

            Dictionary<BsonString, BsonInt32> currentEpisode = new Dictionary<BsonString, BsonInt32>()
            {
                {"Season", 5},
                {"Episode", 1 }

            };

            DocumentModel user = new("Flash", "Serial", seasons, currentEpisode);
            BsonDocument doc = user.ToBsonDocument();
            InsertDocumentIntoCollectionAsync(doc, userdataCollectionName).Wait();

        }


        public async void CreateAccountAsync(string username, string password)
        {
            BsonDocument doc = new AccountModel(username, password).ToBsonDocument();
            await InsertDocumentIntoCollectionAsync(doc, accountsCollectionName);
        }


        public async Task DeleteDocumentAsync(string collectionName, string documentName)
        {
            var collection = GetCollection(collectionName);
            var filter = Builders<BsonDocument>.Filter.Eq("TitleName", documentName);
            await collection.DeleteOneAsync(filter);
        }


        // TODO: сделать универсальным для изменения любого значения, не добавляя параметр
        private async Task ChangeDocumentCurrentEpisodeAsync(string collectionName, string documentName, Dictionary<BsonString, BsonInt32> newValue)
        {
            BsonDocument oldDocument = await GetDocumentByNameAsync(collectionName, documentName);
            DocumentModel user = BsonSerializer.Deserialize<DocumentModel>(oldDocument);
            user.CurrentEpisode = newValue;
            BsonDocument newDocument = user.ToBsonDocument();
            await UpdateDocumentAsync(collectionName, oldDocument, newDocument);
        }


        public async Task UpdateDocumentAsync(string collectionName, BsonDocument oldDocument, BsonDocument newDocument)
        {
            IMongoCollection<BsonDocument> collection = GetCollection(collectionName);
            var result = await collection.UpdateOneAsync(oldDocument, new BsonDocument("$set", newDocument));
        }


        public async Task<BsonDocument> GetDocumentByNameAsync(string collectionName, string documentName)
        {
            IMongoCollection<BsonDocument> collection = GetCollection(collectionName);
            BsonDocument filter = new BsonDocument("TitleName", documentName);
            List<BsonDocument> filteredDocument = new List<BsonDocument>();

            using (IAsyncCursor<BsonDocument> cursor = await collection.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    IEnumerable<BsonDocument> userData = cursor.Current;
                    foreach (var doc in userData)
                    {
                        filteredDocument.Add(doc);
                    }
                }
            }

            return filteredDocument.First();
        }


        // filterParameter = "Film" or "Serial" or "Anime"
        public async Task<List<BsonDocument>> GetTypeFilteredListFromCollectionAsync(string collectionName, string filterParameter)
        {
            IMongoCollection<BsonDocument> collection = GetCollection(collectionName);
            BsonDocument filter = new BsonDocument("TitleType", filterParameter);
            List<BsonDocument> filteredDocuments = new List<BsonDocument>();

            using (IAsyncCursor<BsonDocument> cursor = await collection.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    IEnumerable<BsonDocument> userData = cursor.Current;
                    foreach (var doc in userData)
                    {
                        filteredDocuments.Add(doc);
                    }
                }
            }

            return filteredDocuments;
        }


        public async Task<List<BsonDocument>> GetListOfDocumentsFromCollectionAsync(string collectionName)
        {
            IMongoCollection<BsonDocument> collection = GetCollection(collectionName);
            BsonDocument filter = new BsonDocument();
            List<BsonDocument> documents = new List<BsonDocument>();

            using (IAsyncCursor<BsonDocument> cursor = await collection.FindAsync(filter))
            {
                while (await cursor.MoveNextAsync())
                {
                    IEnumerable<BsonDocument> userData = cursor.Current;
                    foreach (var doc in userData)
                    {
                        documents.Add(doc);
                    }
                }
            }

            return documents;

        }


        // Если коллекции нет, то она создастся
        public async Task InsertDocumentIntoCollectionAsync(BsonDocument document, string collectionName)
        {
            IMongoCollection<BsonDocument> accountsCollection = GetCollection(collectionName);
            await accountsCollection.InsertOneAsync(document);
        }


        private IMongoCollection<BsonDocument> GetCollection(string collectionName)
        {
            MongoClient client = ConnectToDatabaseAndGetClient(connectionString);
            IMongoDatabase database = GetDatabase(client, databaseName);
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>(collectionName);
            return collection;
        }


        private IMongoDatabase GetDatabase(MongoClient client, string databaseName)
        {
            IMongoDatabase database = client.GetDatabase(databaseName);
            return database;
        }


        private MongoClient ConnectToDatabaseAndGetClient(string connectionString)
        {
            MongoClient client = new MongoClient(connectionString);
            return client;
        }
    }
}
