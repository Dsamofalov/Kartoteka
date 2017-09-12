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
        private bool _isEditAuthorOpen = false;
        private ICollectionView _booksDataGridCollection;
        private ICollectionView _authorsDataGridCollection;
        private string _filterBooksString;
        private string _filterAuthorsString;
        private RelayCommand _openAddBookWinCommand;
        private RelayCommand<Book> _openEditBookWinCommand;
        private RelayCommand<Book> _removeBookWinCommand;
        private RelayCommand _openAddAuthorWinCommand;
        private RelayCommand _clearAddBookFlyoutCommand;
        private RelayCommand _clearAddAuthorFlyoutCommand;
        private RelayCommand<Author> _openEditAuthorWinCommand;
        private RelayCommand<Author> _removeAuthorWinCommand;
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
        public bool IsEditAuthorOpen
        {
            get
            {
                return _isEditAuthorOpen;
            }

            set
            {
                _isEditAuthorOpen = value;
                RaisePropertyChanged("IsEditAuthorOpen");
            }
        }
        public ICommand OpenAddBookWinCommand
        {
            get
            {
                if (_openAddBookWinCommand == null) _openAddBookWinCommand = new RelayCommand(() =>
                 {
                     IsNewBookOpen = true;
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
                    IsNewAuthorOpen = true;
                });

                return _openAddAuthorWinCommand;
            }
        }
        public ICommand ClearAddAuthorFlyoutCommand
        {
            get
            {
                if (_clearAddAuthorFlyoutCommand == null) _clearAddAuthorFlyoutCommand = new RelayCommand(() =>
                {
                    Messenger.Default.Send(new NotificationMessage("ClearAddAuthorFlyout"));
                    IsNewAuthorOpen = false;
                });

                return _clearAddAuthorFlyoutCommand;
            }
        }
        public ICommand ClearAddBookFlyoutCommand
        {
            get
            {
                if (_clearAddBookFlyoutCommand == null) _clearAddBookFlyoutCommand = new RelayCommand(() =>
                {
                    Messenger.Default.Send(new NotificationMessage("ClearAddBookFlyout"));
                    IsNewBookOpen = false;
                });

                return _clearAddBookFlyoutCommand;
            }
        }
        public ICommand OpenEditBookWinCommand
        {
            get
            {
                if (_openEditBookWinCommand == null) _openEditBookWinCommand = new RelayCommand<Book>((Book bookToEdit) =>
                {
                    IsEditBookOpen = !IsEditBookOpen;
                    ViewModelLocator._editBookMessenger.Send<NotificationMessage>(new NotificationMessage(bookToEdit.Id.ToString()));
                });
                return _openEditBookWinCommand;
            }
        }
        public ICommand OpenEditAuthorWinCommand
        {
            get
            {
                if (_openEditAuthorWinCommand == null) _openEditAuthorWinCommand = new RelayCommand<Author>((Author authorToEdit) =>
                {
                    IsEditAuthorOpen = !IsEditAuthorOpen;
                    ViewModelLocator._editAuthorMessenger.Send<NotificationMessage>(new NotificationMessage(authorToEdit.Id.ToString()));
                });
                return _openEditAuthorWinCommand;
            }
        }
        public ICommand RemoveBookWinCommand
        {
            get
            {
                if (_removeBookWinCommand == null) _removeBookWinCommand = new RelayCommand<Book>(async (Book bookToRemove) =>
                {
                    _service.DeleteBook(bookToRemove.Id);
                    Books.Remove(bookToRemove);
                    FilterBooksCollection();
                    await dialogCoordinator.ShowMessageAsync(this, "Книга удалена", String.Concat("ID удаленной книги: ", bookToRemove.Id.ToString()));
                });
                return _removeBookWinCommand;
            }
        }
        public ICommand RemoveAuthorWinCommand
        {
            get
            {
                if (_removeAuthorWinCommand == null) _removeAuthorWinCommand = new RelayCommand<Author>(async (Author authorToRemove) =>
                {
                    _service.DeleteAuthor(authorToRemove.Id);
                    Authors.Remove(authorToRemove);
                    FilterAuthorsCollection();
                    await dialogCoordinator.ShowMessageAsync(this, "Автор удален", String.Concat("ID удаленного автора: ", authorToRemove.Id.ToString()));
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
            SimpleIoc.Default.Register(() => ViewModelLocator._editBookMessenger,
              KartotekaConstants.EditBookMessengerKey);
            SimpleIoc.Default.Register(() => ViewModelLocator._editAuthorMessenger,
              KartotekaConstants.EditAuthorMessengerKey);
            MessengerInstance.Register<NotificationMessage>( this, AddAuthorViewModel.Token, message =>
            {
                Authors.Add(_service.GetAuthorByID(int.Parse(message.Notification)));
                FilterAuthorsCollection();
            });
            MessengerInstance.Register<NotificationMessage>(this, AddBookViewModel.Token, message =>
            {
                Books.Add(_service.GetBookByID(int.Parse(message.Notification)));
                FilterBooksCollection();
            });
        }

    }
}