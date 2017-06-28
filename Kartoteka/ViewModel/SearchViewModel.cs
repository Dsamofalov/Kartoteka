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
    public class SearchViewModel:ViewModelBase 
    {
        public RelayCommand<Window> CloseWindowCommand { get; private set; }
        public RelayCommand FindCommand { get; private set; }
        public RelayCommand ViewAuthorCommand { get; private set; }
        public RelayCommand ViewBookCommand { get; private set; }
        public RelayCommand EditAuthorCommand { get; private set; }
        private ObservableCollection<AuthorModel> authors;
        private ObservableCollection<BookModel> books;
        private ObservableCollection<BookModel> allbooks;
        private  bool isAuthor = true;
        private bool isBook = true;
        private string wordToFind;
        private BookModel selectedBook;
        private AuthorModel selectedAuthor;
        private DataBaseModel db = new DataBaseModel();

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
            MessageBox.Show(SelectedBook.Id.ToString());
        }
        public  void CloseWinAndDb(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
            db.Dispose();
        }
        public SearchViewModel()
        {
            this.CloseWindowCommand = new RelayCommand<Window>(CloseWinAndDb);
            this.FindCommand = new RelayCommand(ToFind);
            this.ViewAuthorCommand = new RelayCommand(ViewAuthor);
            this.ViewBookCommand = new RelayCommand(ViewBook);
            this.EditAuthorCommand = new RelayCommand(EditAuthor);
            authors = new ObservableCollection<AuthorModel>();
            books = new ObservableCollection<BookModel>();
            allbooks = new ObservableCollection<BookModel>();
        }
        
    }
}
