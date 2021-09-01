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

        public DocumentModel Document
        {
            get => _document;
            set => _document = value;
        }

        public DeleteDocumentCommand(string userLogin, DocumentModel document, Action<string> SetRowCollection)
        {
            _userLogin = userLogin;
            _document = document;
            _SetRowCollection = SetRowCollection;
        }

        public override async void Execute(object parameter)
        {
            if (_document != null)
            {
                await DatabaseModel.DeleteDocumentAsync(_userLogin, _document.TitleName);
                _SetRowCollection(_userLogin);

            }
        }
    }
}
