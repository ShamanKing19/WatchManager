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
    static class DatabaseModel
    {
        static readonly string connectionString = "mongodb://localhost";
        static readonly string databaseName = "EpisodeManager";
        static readonly string accountsCollectionName = "Accounts";


        public static BsonDocument GetAccount(string userName)
        {
            IMongoCollection<BsonDocument> collection = GetCollection(accountsCollectionName);
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("Login", userName);
            IFindFluent<BsonDocument, BsonDocument> account = collection.Find(filter);
            return account.FirstOrDefault();
        }


        public static async void CreateAccountAsync(string username, string password, string email)
        {
            BsonDocument doc = new AccountModel(username, password, email).ToBsonDocument();
            await InsertDocumentIntoCollectionAsync(doc, accountsCollectionName);
        }


        public static async Task DeleteDocumentAsync(string collectionName, string documentName)
        {
            var collection = GetCollection(collectionName);
            var filter = Builders<BsonDocument>.Filter.Eq("TitleName", documentName);
            await collection.DeleteOneAsync(filter);
        }


        // TODO: сделать универсальным для изменения любого значения, не добавляя параметр
        public static async Task ChangeDocumentCurrentEpisodeAsync(string collectionName, string documentName, SeasonModel newValue, bool watched)
        {
            BsonDocument oldBsonDocument = await GetDocumentByNameAsync(collectionName, documentName);
            DocumentModel oldDocument = BsonSerializer.Deserialize<DocumentModel>(oldBsonDocument);
            oldDocument.CurrentEpisode = newValue;
            oldDocument.Watched = watched;
            BsonDocument newBsonDocument = oldDocument.ToBsonDocument();
            await UpdateDocumentAsync(collectionName, oldBsonDocument, newBsonDocument);
        }


        public static async Task UpdateDocumentAsync(string collectionName, BsonDocument oldDocument, BsonDocument newDocument)
        {
            IMongoCollection<BsonDocument> collection = GetCollection(collectionName);
            var result = await collection.UpdateOneAsync(oldDocument, new BsonDocument("$set", newDocument));
        }


        public static async Task<BsonDocument> GetDocumentByNameAsync(string collectionName, string documentName)
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
        public static async Task<List<BsonDocument>> GetTypeFilteredListFromCollectionAsync(string collectionName, string filterParameter)
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


        public static async Task<List<BsonDocument>> GetListOfDocumentsFromCollectionAsync(string collectionName)
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
        public static async Task InsertDocumentIntoCollectionAsync(BsonDocument document, string collectionName)
        {
            IMongoCollection<BsonDocument> accountsCollection = GetCollection(collectionName);
            await accountsCollection.InsertOneAsync(document);
        }


        private static IMongoCollection<BsonDocument> GetCollection(string collectionName)
        {
            MongoClient client = ConnectToDatabaseAndGetClient(connectionString);
            IMongoDatabase database = GetDatabase(client, databaseName);
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>(collectionName);
            return collection;
        }


        private static IMongoDatabase GetDatabase(MongoClient client, string databaseName)
        {
            IMongoDatabase database = client.GetDatabase(databaseName);
            return database;
        }


        private static MongoClient ConnectToDatabaseAndGetClient(string connectionString)
        {
            MongoClient client = new MongoClient(connectionString);
            return client;
        }
    }
}
