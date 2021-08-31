using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WatchManager.Models;
using WatchManager.Stores;
using WatchManager.ViewModels;

namespace WatchManager.Commands
{
    public class InsertNewDocumentCommand : CommandBase
    {
        private NavigationStore _navigtationStore;
        private Func<BaseViewModel> _createViewModel;
        private DocumentModel _document;
        private string _userLogin;


        public InsertNewDocumentCommand(NavigationStore navigtationStore, Func<BaseViewModel> createViewModel, DocumentModel document, string userLogin)
        {
            _navigtationStore = navigtationStore;
            _createViewModel = createViewModel;
            _document = document;
            _userLogin = userLogin;
        }
        public override async void Execute(object parameter)
        {
            // TODO: новая вьюмодель создаётся раньше чем вносится запись в БД в другом потоке
            // надо что-то с эти придумать
            await DatabaseModel.InsertDocumentIntoCollectionAsync(_document.ToBsonDocument(), _userLogin);
            _navigtationStore.CurrentViewModel = _createViewModel();
        }
    }
}
