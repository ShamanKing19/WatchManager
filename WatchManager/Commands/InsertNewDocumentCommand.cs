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
        private BsonDocument _oldDocument; // Преобразовал к этому BsonDocument, чтобы не менялось значение как у _newDocument, потому что они ссылаются на один участок памяти
        private DocumentModel _newDocument;
        private string _userLogin;


        public InsertNewDocumentCommand(NavigationStore navigtationStore, Func<BaseViewModel> createViewModel, string userLogin, DocumentModel document)
        {
            _navigtationStore = navigtationStore;
            _createViewModel = createViewModel;
            _newDocument = document;
            _userLogin = userLogin;
        }

        public InsertNewDocumentCommand(NavigationStore navigtationStore, Func<BaseViewModel> createViewModel, string userLogin, BsonDocument oldDocument, DocumentModel newDocument)
        {
            _navigtationStore = navigtationStore;
            _createViewModel = createViewModel;
            _oldDocument = oldDocument;
            _newDocument = newDocument;
            _userLogin = userLogin;
            _newDocument.Id = _oldDocument.GetValue("_id").AsObjectId;
        }


        public override async void Execute(object parameter)
        {
            // TODO: разбить на приватные методы (проверку на ноль можно убрать и сделать его онгоингом)
            bool isValidFilm = _newDocument.TitleType == "Film" && _newDocument.TitleName != null && _newDocument.TitleName != "";
            bool isValidSerial = _newDocument.TitleType != "Film" && _newDocument.TitleName != null && _newDocument.TitleName != "" && _newDocument.Seasons != null && _newDocument.CurrentEpisode.SeasonNumber != "0" && _newDocument.CurrentEpisode.SeasonEpisodesCount != "0" && _newDocument.CurrentEpisode.SeasonNumber != "" && _newDocument.CurrentEpisode.SeasonEpisodesCount != "" && !_newDocument.Seasons.Any(season => season.SeasonEpisodesCount == "" || season.SeasonEpisodesCount == "0");
            bool isValidSeasonsTable = _newDocument.Seasons != null && _newDocument.Seasons.All(x => int.TryParse(x.SeasonEpisodesCount, out int res));


            if (isValidFilm || (isValidSerial && isValidSeasonsTable))
            {
                if (_oldDocument == null)
                {
                    // TODO: Убрать async и await и сделать динамическое обновление записей в таблице
                    await DatabaseModel.InsertDocumentIntoCollectionAsync(_newDocument.ToBsonDocument(), _userLogin);
                }
                else
                {
                    await DatabaseModel.UpdateDocumentAsync(_userLogin, _oldDocument, _newDocument.ToBsonDocument());
                }
                
                _navigtationStore.CurrentViewModel = _createViewModel();
            }
        }
    }
}
