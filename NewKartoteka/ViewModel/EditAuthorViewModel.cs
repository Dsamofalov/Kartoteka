using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using Kartoteka.Domain;
using MahApps.Metro.Controls.Dialogs;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace NewKartoteka
{
    public class EditAuthorViewModel : ViewModelBase, IDataErrorInfo
    {
        private IDialogCoordinator dialogCoordinator;
        private readonly IKartotekaService _service;
        private Logger _logger = LogManager.GetCurrentClassLogger();
        private int Id;
        private string _firstName;
        public string FirstName { get { return _firstName; } set { _firstName = value; RaisePropertyChanged("FirstName"); } }

        private string _secondName;
        public string SecondName { get { return _secondName; } set { _secondName = value; RaisePropertyChanged("SecondName"); } }

        private string _lastName;
        public string LastName { get { return _lastName; } set { _lastName = value; RaisePropertyChanged("LastName"); } }

        private ObservableCollection<Book> _books;
        public ObservableCollection<Book> Books { get { return _books; } set { _books = value; RaisePropertyChanged("Books"); } }


        private ObservableCollection<Book> _allBooks;
        public ObservableCollection<Book> AllBooks { get { return _allBooks; } set { _allBooks = value; RaisePropertyChanged("AllBooks"); } }


        private RelayCommand _editAuthorCommand;
        private RelayCommand _openEditBooksCommand;
        private RelayCommand<Window> _closeEditBooksCommand;
        private RelayCommand _removeAllBooksCommand;
        private RelayCommand<object> _removeBooksCommand;
        private RelayCommand<object> _addBooksCommand;
        public ICommand EditAuthorCommand
        {
            get
            {
                if (_editAuthorCommand == null) _editAuthorCommand = new RelayCommand(async () =>
                {
                    _logger.Info($"EditAuthorCommand with author id: {Id} ");
                    Author selectedAuthor = new Author
                    {
                        FirstName = FirstName,
                        SecondName = SecondName,
                        LastName = LastName,
                        Id = Id,
                        books = new ObservableCollection<Book>(Books)
                    };
                    _service.EditAuthor(selectedAuthor);
                    await dialogCoordinator.ShowMessageAsync(this, "Автор изменен", String.Concat("ID измененного автора: ", selectedAuthor.Id));
                });

                return _editAuthorCommand;
            }
        }
        public ICommand OpenEditBooksCommand
        {
            get
            {
                if (_openEditBooksCommand == null) _openEditBooksCommand = new RelayCommand(() =>
                {
                    EditListOfBooksWin newEditWin = new EditListOfBooksWin();
                    newEditWin.Show();
                });

                return _openEditBooksCommand;
            }
        }
        public ICommand CloseEditBooksCommand
        {
            get
            {
                if (_closeEditBooksCommand == null) _closeEditBooksCommand = new RelayCommand<Window>((Window window) =>
                {
                    if (window != null)
                    {
                        window.Close();
                    }
                });

                return _closeEditBooksCommand;
            }
        }
        public ICommand RemoveAllBooksCommand
        {
            get
            {
                if (_removeAllBooksCommand == null) _removeAllBooksCommand = new RelayCommand(() =>
                {
                    foreach (Book book in Books)
                    {
                        AllBooks.Add(book);
                    }
                    Books.Clear();
                });

                return _removeAllBooksCommand;
            }
        }
        public ICommand RemoveBooksCommand
        {
            get
            {
                if (_removeBooksCommand == null) _removeBooksCommand = new RelayCommand<object>((object parameter) =>
                {
                    IList selection = (IList)parameter;
                    List<Book> newBooks = selection.Cast<Book>().ToList();
                    foreach (Book book in newBooks)
                    {
                        Books.Remove(book);
                        AllBooks.Add(book);
                    }
                });

                return _removeBooksCommand;
            }
        }
        public ICommand AddBooksCommand
        {
            get
            {
                if (_addBooksCommand == null) _addBooksCommand = new RelayCommand<object>((object parameter) =>
                {
                    IList selection = (IList)parameter;
                    List<Book> newBooks = selection.Cast<Book>().ToList();
                    foreach (Book book in newBooks)
                    {
                        Books.Add(book);
                        AllBooks.Remove(book);
                    }
                });

                return _addBooksCommand;
            }
        }
        public string Error
        {
            get
            {
                return string.Empty;
            }
        }

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "FirstName":
                        {
                            if (String.IsNullOrEmpty(this.FirstName))
                                return "Заполните пустую строку";
                            break;
                        }
                    case "SecondName":
                        {
                            if (String.IsNullOrEmpty(this.SecondName))
                                return "Заполните пустую строку";
                            break;
                        }
                    case "LastName":
                        {
                            if (String.IsNullOrEmpty(this.LastName))
                                return "Заполните пустую строку";
                            break;
                        }
                }

                return string.Empty;
            }
        }
        public void FillInformationAboutAuthor(NotificationMessage notificationMessage)
        {
            string notification = notificationMessage.Notification;
            Author selectedAuthor = _service.GetAuthorByID(int.Parse(notification));
            FirstName = selectedAuthor.FirstName;
                Id = selectedAuthor.Id;
                SecondName = selectedAuthor.SecondName;
                LastName = selectedAuthor.LastName;
                Books = new ObservableCollection<Book>(selectedAuthor.books);
            AllBooks = new ObservableCollection<Book>(_service.GetAllBooks());
            foreach (Book book in Books)
            {
                AllBooks.Remove(book);
            }
        }
        public EditAuthorViewModel(IKartotekaService service)
        {
            dialogCoordinator = DialogCoordinator.Instance;
            try
            {
                if (service == null) throw new ArgumentNullException("service", "service is null");
                _service = service;
            }
            catch (ArgumentNullException ex)
            {
                _logger.Error($"EditAuthorViewModel ctor can't get a service {ex}");
                MessageBox.Show("An exception just occurred: " + ex.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
            var messenger = SimpleIoc.Default.GetInstance<Messenger>(KartotekaConstants.EditAuthorMessengerKey);
            messenger.Register<NotificationMessage>(this, FillInformationAboutAuthor);
        }
    }
}