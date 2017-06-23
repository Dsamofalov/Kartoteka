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

namespace Kartoteka
{
    public class BookViewModel:ViewModelBase
    {
        public RelayCommand<Window> CloseWindowCommand { get; private set; }
        private ObservableCollection<AuthorModel> authors;
        public RelayCommand<object> SaveListCommand { get; private set; }
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
        public ObservableCollection<AuthorModel> Authors
        {
            get
            {
                return authors;
            }

            set
            {
                authors = value;
                RaisePropertyChanged("Authors");
            }
        }
        private void SaveBook(object parameter)
        {
            IList selection = (IList)parameter;
            List<AuthorModel> newauthors = selection.Cast<AuthorModel>().ToList();
            MessageBox.Show(newauthors.Count.ToString());
        }
        public BookViewModel()
        {
            this.CloseWindowCommand = new RelayCommand<Window>(CustomCommands.CloseWindow);
            this.SaveListCommand = new RelayCommand<object>(SaveBook);
            authors = new ObservableCollection<AuthorModel>
            {
                new AuthorModel {Id=1, Name="dsg", Surname="dgshhhh" },
                new AuthorModel {Id=1, Name="dsg", Surname="dgshhhh" },
                new AuthorModel {Id=1, Name="dsg", Surname="dgshhhh" },
                new AuthorModel {Id=1, Name="dsg", Surname="dgshhhh" },
                new AuthorModel {Id=1, Name="dsg", Surname="dgshhhh" },
                new AuthorModel {Id=1, Name="dsg", Surname="dgshhhh" },
                new AuthorModel {Id=1, Name="dsg", Surname="dgshhhh" },
                new AuthorModel {Id=1, Name="dsg", Surname="dgshhhh" },
                new AuthorModel {Id=1, Name="dsg", Surname="dgshhhh" },
                new AuthorModel {Id=1, Name="dsg", Surname="dgshhhh" },
                new AuthorModel {Id=1, Name="dsg", Surname="dgshhhh" },
                new AuthorModel {Id=1, Name="dsg", Surname="dgshhhh" },
                new AuthorModel {Id=1, Name="dsg", Surname="dgshhhh" },
                new AuthorModel {Id=1, Name="dsg", Surname="dgshhhh" }
            };
            SelectedBook = new BookModel();
        }
    }
}
