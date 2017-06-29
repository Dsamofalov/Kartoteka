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
    public class SearchViewModel:ViewModelBase,IDisposable
    {
        public RelayCommand<Window> CloseWindowCommand { get; private set; }
        public RelayCommand<Window> CloseEditWinCommand { get; private set; }
        public RelayCommand FindCommand { get; private set; }
        public RelayCommand ViewAuthorCommand { get; private set; }
        public RelayCommand ViewBookCommand { get; private set; }
        public RelayCommand EditAuthorCommand { get; private set; }
        public RelayCommand EditBookCommand { get; private set; }
        public RelayCommand<object> AddNewBooksCommand { get; private set; }
        public RelayCommand<object> RemoveBooksCommand { get; private set; }
        public RelayCommand<object> AddNewAuthorsCommand { get; private set; }
        public RelayCommand<object> RemoveAuthorsCommand { get; private set; }
        private ObservableCollection<AuthorModel> authors;
        private ObservableCollection<BookModel> books;
        private ObservableCollection<AuthorModel> allauthors;
        private ObservableCollection<BookModel> allbooks;
        private  bool isAuthor = true;
        private bool isBook = true;
        private string wordToFind;
        private BookModel selectedBook;
        private AuthorModel selectedAuthor;
        private DataBaseModel db;

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
        public ObservableCollection<BookModel> Allbooks
        {
            get
            {
                return allbooks;
            }

            set
            {
                allbooks = value;
                RaisePropertyChanged("Allbooks");
            }
        }
        public ObservableCollection<AuthorModel> Allauthors
        {
            get
            {
                return allauthors;
            }

            set
            {
                allauthors = value;
                RaisePropertyChanged("Allauthors");
            }
        }
        public bool IsAuthor
        {
            get
            {
                return isAuthor;
            }

            set
            {
                isAuthor = value;
                RaisePropertyChanged("IsAuthor");
                ;
            }
        }
        public bool IsBook
        {
            get
            {
                return isBook;
            }

            set
            {
                isBook = value;
                RaisePropertyChanged("IsBook");
                ;
            }
        }

        public string WordToFind
        {
            get
            {
                return wordToFind;
            }

            set
            {
                wordToFind = value;
                RaisePropertyChanged("WordToFind");
            }
        }
        private void ClearCollections()
        {
            books.Clear();
            authors.Clear();
        }
        private void ToFind()
        {
            db = new DataBaseModel();
            ClearCollections();
            if (CustomCommands.IsFilled(WordToFind) == true)
            {
                GetFromDb();
            }
            else MessageBox.Show("Write a word to find");
        }
        private void GetFromDb()
        {
                foreach (AuthorModel newauthor in db.authors)
                {
                    if ((newauthor.Name.Contains(WordToFind)) || (newauthor.Surname.Contains(WordToFind)))
                    {
                        authors.Add(newauthor);
                    }
                }
                foreach (BookModel newbook in db.books)
                {

                    if (newbook.Title.Contains(WordToFind))
                    {
                        books.Add(newbook);
                    }
                }
                MessageBox.Show("Search was succsessfully performed");
        }
        private void ViewAuthor()
        {
                AuthorProfile AuthorP = new AuthorProfile();
                AuthorP.Show();
        }
        private void EditAuthor()
        {
            EditAuthor AuthorEd = new EditAuthor();
            AuthorEd.Show();
            foreach (BookModel newbook in db.books)
            {
                if(SelectedAuthor.books.Contains(newbook))
                {
                    
                }
                else
                {
                    allbooks.Add(newbook);
                }

            }
        }
        private void ViewBook()
        {
            BookProfile BookP = new BookProfile();
            BookP.Show();
        }
        private void EditBook()
        {
            EditBook BookEd = new EditBook();
            BookEd.Show();
            foreach (AuthorModel newauthor in db.authors)
            {
                if (SelectedBook.authors.Contains(newauthor))
                {

                }
                else
                {
                    allauthors.Add(newauthor);
                }

            }
        }
        private void CloseWinAndDb(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
            ClearCollections();
            WordToFind = null;
        }
        private void AddNewBooks(object parameter)
        {
            List<BookModel> newbooks = CustomCommands.GetBooksFromList(parameter);
            CustomCommands.AddBooks(SelectedAuthor, newbooks, db);
            db.SaveChanges();
            MessageBox.Show("Added succsessfully");
        }
        private void RemoveBooks(object parameter)
        {
            List<BookModel> newbooks = CustomCommands.GetBooksFromList(parameter);
            CustomCommands.RemoveBooks(SelectedAuthor, newbooks, db);
            db.SaveChanges();
            MessageBox.Show("Removed succsessfully");
        }
        private void AddNewAuthors(object parameter)
        {
            List<AuthorModel> newauthors = CustomCommands.GetAuthorsFromList(parameter);
            CustomCommands.AddAuthors(SelectedBook, newauthors, db);
            db.SaveChanges();
            MessageBox.Show("Added succsessfully");
        }
        private void RemoveAuthors(object parameter)
        {
            List<AuthorModel> newauthors = CustomCommands.GetAuthorsFromList(parameter);
            CustomCommands.RemoveAuthors(SelectedBook, newauthors, db);
            db.SaveChanges();
            MessageBox.Show("Removed succsessfully");
        }

        public void Dispose()
        {
            try
            {
                ((IDisposable)db).Dispose();
            }
            catch
            {

            }
        }

        public SearchViewModel()
        {
            this.CloseWindowCommand = new RelayCommand<Window>(CloseWinAndDb);
            this.CloseEditWinCommand = new RelayCommand<Window>(CustomCommands.CloseWindow);
            this.FindCommand = new RelayCommand(ToFind);
            this.ViewAuthorCommand = new RelayCommand(ViewAuthor);
            this.EditAuthorCommand = new RelayCommand(EditAuthor);
            this.AddNewBooksCommand = new RelayCommand<object>(AddNewBooks);
            this.RemoveBooksCommand = new RelayCommand<object>(RemoveBooks);
            this.ViewBookCommand = new RelayCommand(ViewBook);
            this.EditBookCommand = new RelayCommand(EditBook);
            this.AddNewAuthorsCommand = new RelayCommand<object>(AddNewAuthors);
            this.RemoveAuthorsCommand = new RelayCommand<object>(RemoveAuthors);
            authors = new ObservableCollection<AuthorModel>();
            allauthors = new ObservableCollection<AuthorModel>();
            books = new ObservableCollection<BookModel>();
            allbooks = new ObservableCollection<BookModel>();

        }
        
    }
}
