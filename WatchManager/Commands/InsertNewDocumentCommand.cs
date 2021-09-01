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
        private DocumentModel _oldDocument;
        private DocumentModel _newDocument;
        private string _userLogin;


        public InsertNewDocumentCommand(NavigationStore navigtationStore, Func<BaseViewModel> createViewModel, string userLogin, DocumentModel document)
        {
            _navigtationStore = navigtationStore;
            _createViewModel = createViewModel;
            _newDocument = document;
            _userLogin = userLogin;
        }

        public InsertNewDocumentCommand(NavigationStore navigtationStore, Func<BaseViewModel> createViewModel, string userLogin, DocumentModel oldDocument, DocumentModel newDocument)
        {
            _navigtationStore = navigtationStore;
            _createViewModel = createViewModel;
            _oldDocument = oldDocument;
            _newDocument = newDocument;
            _userLogin = userLogin;
            _newDocument.Id = _oldDocument.Id;
        }


        public override async void Execute(object parameter)
        {
            bool isValidFilm = _newDocument.TitleType == "Film" && _newDocument.TitleName != null && _newDocument.TitleName != "";
            bool isValidSerial = _newDocument.TitleType != "Film" && _newDocument.TitleName != null && _newDocument.TitleName != "" && _newDocument.Seasons != null && _newDocument.CurrentEpisode.SeasonNumber != "0" && _newDocument.CurrentEpisode.SeasonEpisodesCount != "0" && _newDocument.CurrentEpisode.SeasonNumber != "" && _newDocument.CurrentEpisode.SeasonEpisodesCount != "";
            
            if (isValidFilm || isValidSerial)
            {
                if (_oldDocument == null)
                {
                    // TODO: Убрать async и await и сделать динамическое обновление записей в таблице
                    await DatabaseModel.InsertDocumentIntoCollectionAsync(_newDocument.ToBsonDocument(), _userLogin);
                }
                else
                {
                    await DatabaseModel.UpdateDocumentAsync(_userLogin, _oldDocument.ToBsonDocument(), _newDocument.ToBsonDocument());
                }
                
                _navigtationStore.CurrentViewModel = _createViewModel();
            }
        }
    }
}
