using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Kartoteka
{
    public class AuthorViewModel:ViewModelBase
    {
        public RelayCommand<Window> CloseWindowCommand { get; private set; }
        public RelayCommand<object> SaveListCommand { get; private set; }

        private ObservableCollection<BookModel> books;
        private static AuthorModel selectedAuthor;
        public static AuthorModel SelectedAuthor
        {
            get
            {
                return selectedAuthor;
            }
            set
            {
                selectedAuthor = value;
            }
        }

        public ObservableCollection<BookModel> Books
        {
            get
            {
                return books;
            }

            set
            {
                books = value;
                RaisePropertyChanged("Books");
            }
        }
        private void SaveAuthor(object parameter)
        {
            IList selection = (IList)parameter;
            List<BookModel> newbooks = selection.Cast<BookModel>().ToList();
            foreach (BookModel item in newbooks)
            {
                SelectedAuthor.Books.Add(item);
            }
        }

        public AuthorViewModel()
        {
            
            this.CloseWindowCommand = new RelayCommand<Window>(CustomCommands.CloseWindow);
            this.SaveListCommand = new RelayCommand<object>(SaveAuthor);
            books = new ObservableCollection<BookModel>
            {
                new BookModel {ID=1, Title="dsg", Genre="dgshhhh" },
                new BookModel {ID=1, Title="dsg", Genre="dgshhhh" },
                new BookModel {ID=1, Title="dsg", Genre="dgshhhh" },
                new BookModel {ID=1, Title="dsg", Genre="dgshhhh" },
                new BookModel {ID=1, Title="dsg", Genre="dgshhhh" },
                new BookModel {ID=1, Title="dsg", Genre="dgshhhh" },
                new BookModel {ID=1, Title="dsg", Genre="dgshhhh" },
                new BookModel {ID=1, Title="dsg", Genre="dgshhhh" },
                new BookModel {ID=1, Title="dsg", Genre="dgshhhh" },
                new BookModel {ID=1, Title="dsg", Genre="dgshhhh" },
            };
        }
    }
}
