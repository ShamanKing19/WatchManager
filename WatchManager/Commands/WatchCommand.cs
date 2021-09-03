using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WatchManager.Models;

namespace WatchManager.Commands
{
    public class WatchCommand : CommandBase
    {
        private string _userLogin;

        public DocumentModel Document { get; set; }
        public ObservableCollection<DocumentModel> Collection { get; set; }

        public WatchCommand(string userLogin)
        {
            _userLogin = userLogin;
        }

        public override async void Execute(object parameter)
        {
            if (Document != null)
            {
                int documentIndex = Collection.IndexOf(Document);
                
                Document.WatchEpisode();
                Task.Run(() => DatabaseModel.ChangeDocumentCurrentEpisodeAsync(_userLogin, Document.TitleName, Document.CurrentEpisode, Document.Watched));
                // Пожалуйста, не бейте за это (не хочу, чтобы DocumentModel реализовывала INotify)
                Collection.Insert(documentIndex+1, Document);
                Collection.Remove(Document);
                
            }

        }
    }
}
