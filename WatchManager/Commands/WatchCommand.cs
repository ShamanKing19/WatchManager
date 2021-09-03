using MongoDB.Bson;
using System;
using System.Collections.Generic;
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
        Action<string> UpdateTable { get; set; }



        public WatchCommand(string userLogin, Action<string> updateTable)
        {
            _userLogin = userLogin;
            UpdateTable = updateTable;
        }

        public override async void Execute(object parameter)
        {
            if (Document != null)
            {
                Document.WatchEpisode();
                await DatabaseModel.ChangeDocumentCurrentEpisodeAsync(_userLogin, Document.TitleName, Document.CurrentEpisode, Document.Watched);
                UpdateTable(_userLogin);
            }

        }
    }
}
