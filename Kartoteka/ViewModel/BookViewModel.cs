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
    public class BookViewModel:ViewModelBase
    {
        public RelayCommand<Window> CloseWindowCommand { get; private set; }
        private ObservableCollection<BookModel> books;
        private BookModel selectedBook;
        public BookModel SelectedBook
        {
            get
            {
                return selectedBook;
            }
            set
            {
                selectedBook = value;
                RaisePropertyChanged("SelectedBook");
            }
        }
        public BookViewModel()
        {
            this.CloseWindowCommand = new RelayCommand<Window>(ClosingWindow.CloseWindow);
        }
    }
}
