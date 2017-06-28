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
        private ObservableCollection<AuthorModel> authors;
        private ObservableCollection<BookModel> books;
        private  bool isAuthor = true;
        private bool isBook = true;
        private string wordToFind;
        private BookModel selectedBook;
        private AuthorModel selectedAuthor;
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
            using (DataBaseModel db = new DataBaseModel())
            {
                foreach (AuthorModel newauthor in db.authors)
                {
                    if ((newauthor.Name.Contains(WordToFind))||(newauthor.Surname.Contains(WordToFind)))
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

                db.Dispose();
                MessageBox.Show("Search was succsessfully performed");
            }
        }
        private void ViewAuthor()
        {
            AuthorProfile AuthorP = new AuthorProfile();
            AuthorP.Show();
        }
        private void ViewBook()
        {
            MessageBox.Show(SelectedBook.Id.ToString());
        }
        public SearchViewModel()
        {
            this.CloseWindowCommand = new RelayCommand<Window>(CustomCommands.CloseWindow);
            this.FindCommand = new RelayCommand(ToFind);
            this.ViewAuthorCommand = new RelayCommand(ViewAuthor);
            this.ViewBookCommand = new RelayCommand(ViewBook);
            authors = new ObservableCollection<AuthorModel>();
            books = new ObservableCollection<BookModel>();
            
        }
        
    }
}
