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
    public class AddDocumentCommand : CommandBase
    {
        private NavigationStore _navigtationStore;
        private Func<BaseViewModel> _createViewModel;
        private DocumentModel _document;


        public AddDocumentCommand(NavigationStore navigtationStore, Func<BaseViewModel> createViewModel, DocumentModel document)
        {
            _navigtationStore = navigtationStore;
            _createViewModel = createViewModel;
            _document = document;
        }
        public override void Execute(object parameter)
        {
            // Вот здесь запись будет сохраняться в базу данных
            MessageBox.Show(_document.ToString());
            _navigtationStore.CurrentViewModel = _createViewModel();
        }
    }
}
