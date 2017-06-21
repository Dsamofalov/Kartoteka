using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kartoteka
{
    public class AuthorViewModel:ViewModelBase
    {
        public RelayCommand<Window> CloseWindowCommand { get; private set; }
        private ObservableCollection<AuthorModel> authors;
        private AuthorModel selectedAuthor;
        public AuthorModel SelectedAuthor
        {
            get
            {
                return selectedAuthor;
            }
            set
            {
                selectedAuthor = value;
                RaisePropertyChanged("SelectedAuthor");
            }
        }
        public AuthorViewModel()
        {
            this.CloseWindowCommand = new RelayCommand<Window>(ClosingWindow.CloseWindow);
        }
    }
}
