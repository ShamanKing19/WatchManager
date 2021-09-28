using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchManager.Models;

namespace WatchManager.Commands
{
    public class DeleteDocumentCommand : CommandBase
    {
        private string _userLogin;
        private DocumentModel _document;
        private Action<string> _SetRowCollection;
        private BackToWatchPageCommand _backToWatchPageCommand;

        public DocumentModel Document
        {
            get => _document;
            set => _document = value;
        }

        public DeleteDocumentCommand(string userLogin, DocumentModel document, Action<string> SetRowCollection, BackToWatchPageCommand backToWatchPageCommand)
        {
            _userLogin = userLogin;
            _document = document;
            _SetRowCollection = SetRowCollection;
            _backToWatchPageCommand = backToWatchPageCommand;
        }

        public override async void Execute(object parameter)
        {
            if (_document != null)
            {
                await DatabaseModel.DeleteDocumentAsync(_userLogin, _document.TitleName);
                _SetRowCollection(_userLogin);
                _backToWatchPageCommand.Execute(parameter);
            }
        }
    }
}
