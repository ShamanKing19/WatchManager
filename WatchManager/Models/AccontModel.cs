using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchManager.Models
{
    public class AccountModel
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public AccountModel(string login, string password, string email)
        {
            Login = login;
            Password = password;
            Email = email;
        }
    }
}
