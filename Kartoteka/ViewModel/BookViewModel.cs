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
        private void GetFromList(object parameter)
        {
            if (CustomCommands.IsFilled(SelectedBook.Title, SelectedBook.Genre) == true)
            {
                SaveBook(CustomCommands.GetAuthorsFromList(parameter));
            }
            else MessageBox.Show("Fill in the title and genre fields");
        }
        private void SaveBook(List<AuthorModel> newauthors)
        {
            using (DataBaseModel db1 = new DataBaseModel())
            {
                CustomCommands.AddAuthors(SelectedBook, newauthors, db1);
                db1.books.Add(SelectedBook);
                db1.SaveChanges();
            }
            MessageBox.Show("Saved succsessfully");
            SelectedBook.Genre = null;
            SelectedBook.Title = null;
            SelectedBook.authors.Clear();
        }
        public BookViewModel()
        {
            this.CloseWindowCommand = new RelayCommand<Window>(CustomCommands.CloseWindow);
            this.SaveListCommand = new RelayCommand<object>(GetFromList);
            authors = new ObservableCollection<AuthorModel>();
            this.SelectedBook = new BookModel();
            using (DataBaseModel db = new DataBaseModel())
            {
                foreach (AuthorModel newauthor in db.authors)
                {
                    authors.Add(newauthor);
                }
            }
        }
    }
}
