using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchManager.Models;
using WatchManager.Stores;
using WatchManager.ViewModels;

namespace WatchManager.Commands
{
    public class EditDocumentCommand : CommandBase
    {
        private NavigationStore _navigtationStore;
        private Func<BaseViewModel> _createViewModel;
        private DocumentModel _oldDocument;
        private DocumentModel _newDocument;
        private string _userLogin;


        public EditDocumentCommand(NavigationStore navigtationStore, Func<BaseViewModel> createViewModel, DocumentModel oldDocument, DocumentModel newDocument, string userLogin)
        {
            _navigtationStore = navigtationStore;
            _createViewModel = createViewModel;
            _oldDocument = oldDocument;
            _newDocument = newDocument;
            _userLogin = userLogin;
        }


        public override async void Execute(object parameter)
        {
            bool isValidFilm = _newDocument.TitleType == "Film" && _newDocument.TitleName != null && _newDocument.TitleName != "";
            bool isValidSerial = _newDocument.TitleType != "Film" && _newDocument.TitleName != null && _newDocument.TitleName != "" && _newDocument.Seasons != null && _newDocument.CurrentEpisode.SeasonNumber != "0" & _newDocument.CurrentEpisode.SeasonEpisodesCount != "0" && _newDocument.CurrentEpisode.SeasonNumber != "" & _newDocument.CurrentEpisode.SeasonEpisodesCount != "";

            if (isValidFilm || isValidSerial)
            {
                // TODO: Убрать async и await и сделать динамическое обновление записей в таблице
                await DatabaseModel.UpdateDocumentAsync(_userLogin, _oldDocument.ToBsonDocument(), _newDocument.ToBsonDocument());
                _navigtationStore.CurrentViewModel = _createViewModel();
            }
        }
    }
}