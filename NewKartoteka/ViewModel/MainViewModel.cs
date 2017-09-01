using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Kartoteka.Domain;
using NewKartoteka.Model;
using System;
using System.Collections.ObjectModel;
using MahApps.Metro.Controls;
using System.Windows;
using Microsoft.Practices.ServiceLocation;
using System.Windows.Input;
using Kartoteka.DAL;
using System.ComponentModel;
using System.Windows.Data;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Ioc;
using MahApps.Metro.Controls.Dialogs;

namespace NewKartoteka.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IKartotekaService _service;
        private ObservableCollection<Book> _books;
        private ObservableCollection<Author> _authors;
        private bool _isNewBookOpen =false;
        private bool _isNewAuthorOpen = false;
        private bool _isEditBookOpen = false;
        private ICollectionView _booksDataGridCollection;
        private ICollectionView _authorsDataGridCollection;
        private string _filterBooksString;
        private string _filterAuthorsString;
        private RelayCommand _openAddBookWinCommand;
        private RelayCommand<Book> _openEditBookWinCommand;
        private RelayCommand<Book> _removeBookWinCommand;
        private RelayCommand _openAddAuthorWinCommand;
        private RelayCommand<Author> _removeAuthorWinCommand;
        private Messenger _editBookMessenger;
        private IDialogCoordinator dialogCoordinator;
        public ObservableCollection<Book> Books
        {
            get
            {
                return _books;
            }

            set
            {
                _books = value;
                RaisePropertyChanged("Books");
            }
        }
        public ObservableCollection<Author> Authors
        {
            get
            {
                return _authors;
            }

            set
            {
                _authors = value;
                RaisePropertyChanged("Authors");
            }
        }

        public bool IsNewBookOpen
        {
            get
            {
                return _isNewBookOpen;
            }

            set
            {
                _isNewBookOpen = value;
                RaisePropertyChanged("IsNewBookOpen");
            }
        }

        public bool IsEditBookOpen
        {
            get
            {
                return _isEditBookOpen;
            }

            set
            {
                _isEditBookOpen = value;
                RaisePropertyChanged("IsEditBookOpen");
            }
        }
        public bool IsNewAuthorOpen
        {
            get
            {
                return _isNewAuthorOpen;
            }

            set
            {
                _isNewAuthorOpen = value;
                RaisePropertyChanged("IsNewAuthorOpen");
            }
        }
        public ICommand OpenAddBookWinCommand
        {
            get
            {
                if (_openAddBookWinCommand == null) _openAddBookWinCommand = new RelayCommand(() =>
                 {
                     IsNewBookOpen = !IsNewBookOpen;
                 });

                return _openAddBookWinCommand;
            }
        }
        public ICommand OpenAddAuthorWinCommand
        {
            get
            {
                if (_openAddAuthorWinCommand == null) _openAddAuthorWinCommand = new RelayCommand(() =>
                {
                    IsNewAuthorOpen = !IsNewAuthorOpen;
                });

                return _openAddAuthorWinCommand;
            }
        }
        public ICommand OpenEditBookWinCommand
        {
            get
            {
                if (_openEditBookWinCommand == null) _openEditBookWinCommand = new RelayCommand<Book>((Book BookToEdit) =>
                {
                    IsEditBookOpen = !IsEditBookOpen;
                    _editBookMessenger.Send<NotificationMessage>(new NotificationMessage(BookToEdit.Id.ToString()));
                });
                return _openEditBookWinCommand;
            }
        }
        public ICommand RemoveBookWinCommand
        {
            get
            {
                if (_removeBookWinCommand == null) _removeBookWinCommand = new RelayCommand<Book>(async (Book BookToRemove) =>
                {
                    _service.DeleteBook(BookToRemove.Id);
                    Books.Remove(BookToRemove);
                    FilterBooksCollection();
                    await dialogCoordinator.ShowMessageAsync(this, "Книга удалена", String.Concat("ID удаленной книги: ", BookToRemove.Id.ToString()));
                });
                return _removeBookWinCommand;
            }
        }
        public ICommand RemoveAuthorWinCommand
        {
            get
            {
                if (_removeAuthorWinCommand == null) _removeAuthorWinCommand = new RelayCommand<Author>(async (Author AuthorToRemove) =>
                {
                    _service.DeleteAuthor(AuthorToRemove.Id);
                    Authors.Remove(AuthorToRemove);
                    FilterAuthorsCollection();
                    await dialogCoordinator.ShowMessageAsync(this, "Автор удален", String.Concat("ID удаленного автора: ", AuthorToRemove.Id.ToString()));
                });
                return _removeAuthorWinCommand;
            }
        }

        public ICollectionView BooksDataGridCollection
        {
            get { return _booksDataGridCollection; }
            set { _booksDataGridCollection = value; RaisePropertyChanged("BooksDataGridCollection"); }
        }
        public ICollectionView AuthorsDataGridCollection
        {
            get { return _authorsDataGridCollection; }
            set { _authorsDataGridCollection = value; RaisePropertyChanged("AuthorsDataGridCollection"); }
        }
        public string FilterBooksString
        {
            get { return _filterBooksString; }
            set
            {
                _filterBooksString = value;
                RaisePropertyChanged("FilterBooksString");
                FilterBooksCollection();
            }
        }
        public string FilterAuthorsString
        {
            get { return _filterAuthorsString; }
            set
            {
                _filterAuthorsString = value;
                RaisePropertyChanged("FilterAuthorsString");
                FilterAuthorsCollection();
            }
        }

        private void FilterBooksCollection()
        {
            if (_booksDataGridCollection != null)
            {
                _booksDataGridCollection.Refresh();
            }
        }
        private void FilterAuthorsCollection()
        {
            if (_authorsDataGridCollection != null)
            {
                _authorsDataGridCollection.Refresh();
            }
        }
        public bool FilterBooks(object obj)
        {
            var data = obj as Book;
            if (data != null)
            {
                if (!string.IsNullOrEmpty(_filterBooksString))
                {
                    return data.Name.Contains(_filterBooksString) || data.Description.Contains(_filterBooksString) || data.Year.ToString().Contains(_filterBooksString) || data.Id.ToString().Contains(_filterBooksString);
                }
                return true;
            }
            return false;
        }
        public bool FilterAuthors(object obj)
        {
            var data = obj as Author;
            if (data != null)
            {
                if (!string.IsNullOrEmpty(_filterAuthorsString))
                {
                    return data.FirstName.Contains(_filterAuthorsString) || data.SecondName.Contains(_filterAuthorsString) || data.LastName.Contains(_filterAuthorsString) || data.Id.ToString().Contains(_filterAuthorsString);
                }
                return true;
            }
            return false;
        }
        public MainViewModel(IKartotekaService service)
        {
            if (service == null) throw new ArgumentNullException("service", "service is null");
            _service = service;
            dialogCoordinator = DialogCoordinator.Instance;
            Books = new ObservableCollection<Book>(_service.GetAllBooks());
            Authors = new ObservableCollection<Author>(_service.GetAllAuthors());
            BooksDataGridCollection = CollectionViewSource.GetDefaultView(Books);
            BooksDataGridCollection.Filter = new Predicate<object>(FilterBooks);
            AuthorsDataGridCollection = CollectionViewSource.GetDefaultView(Authors);
            AuthorsDataGridCollection.Filter = new Predicate<object>(FilterAuthors);
            MessengerInstance.Register<NotificationMessage>(this,message =>
            {
                if(message.Notification == "RefreshListOfBooks")
                FilterBooksCollection();
            });
            MessengerInstance.Register<NotificationMessage>(this, message =>
            {
                if (message.Notification == "RefreshListOfAuthors")
                    FilterBooksCollection();
            });
            _editBookMessenger = new Messenger();
            SimpleIoc.Default.Register(() => _editBookMessenger,
              "EditBookMessenger");
        }

    }
}