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
            IList selection = (IList)parameter;
            List<AuthorModel> newauthors = selection.Cast<AuthorModel>().ToList();
            if (CustomCommands.IsFilled(SelectedBook.Title, SelectedBook.Genre) == true)
            {
                SaveBook(newauthors);
            }
            else MessageBox.Show("Fill in the title and genre fields");
        }
        private void SaveBook(List<AuthorModel> newauthors)
        {
            using (DataBaseModel db1 = new DataBaseModel())
            {
                foreach (AuthorModel newauthor in newauthors)
                {
                    SelectedBook.authors.Add(db1.authors.Find(newauthor.Id));
                }
                db1.books.Add(SelectedBook);
                db1.SaveChanges();
                db1.Dispose();
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
                db.Dispose();
            }
        }
    }
}
