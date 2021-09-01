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
    public class SwitchToEditPageCommand : CommandBase
    {
        private NavigationStore _navigtationStore;
        private Func<BaseViewModel> _createViewModel;
        private DocumentModel _document;

        public DocumentModel Document
        {
            get => _document;
            set => _document = value;
        }

        public SwitchToEditPageCommand(NavigationStore navigtationStore, Func<BaseViewModel> createViewModel, DocumentModel document)
        {
            _navigtationStore = navigtationStore;
            _createViewModel = createViewModel;
            _document = document;
        }
        public override void Execute(object parameter)
        {
            if (Document != null)
            {
                _navigtationStore.CurrentViewModel = _createViewModel();
            }
        }
    }
}
