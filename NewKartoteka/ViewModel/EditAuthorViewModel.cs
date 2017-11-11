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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NewKartoteka
{
    public class EditAuthorViewModel : ViewModelBase, IDataErrorInfo
    {
        private readonly IKartotekaService _service;
        private readonly ILoggerService _loggingService;

        private IDialogCoordinator dialogCoordinator;
        private int Id;
        private string _firstName;
        private string _secondName;
        private string _lastName;
        private ObservableCollection<Book> _allBooks;
        private ObservableCollection<Book> _books;
        private RelayCommand _editAuthorCommand;
        private RelayCommand _openEditBooksCommand;
        private RelayCommand<Window> _closeEditBooksCommand;
        private RelayCommand _removeAllBooksCommand;
        private RelayCommand<IList> _removeBooksCommand;
        private RelayCommand<IList> _addBooksCommand;
        public string FirstName { get { return _firstName; } set { _firstName = value; RaisePropertyChanged("FirstName"); } }

        public string SecondName { get { return _secondName; } set { _secondName = value; RaisePropertyChanged("SecondName"); } }

        public string LastName { get { return _lastName; } set { _lastName = value; RaisePropertyChanged("LastName"); } }

        public ObservableCollection<Book> Books { get { return _books; } set { _books = value; RaisePropertyChanged("Books"); } }


        public ObservableCollection<Book> AllBooks { get { return _allBooks; } set { _allBooks = value; RaisePropertyChanged("AllBooks"); } }

        //REVIEW: Инкапсулировать команды.
        public ICommand EditAuthorCommand
        {
            get
            {
                if (_editAuthorCommand == null) _editAuthorCommand = new RelayCommand(async () =>
                {
                    _loggingService.LogInfo($"EditAuthorCommand with author id: {Id} ");
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
                    newEditWin.ShowDialog();
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
                if (_removeBooksCommand == null) _removeBooksCommand = new RelayCommand<IList>((IList selection) =>
                {
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
                if (_addBooksCommand == null) _addBooksCommand = new RelayCommand<IList>((IList selection) =>
                {
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
                //REVIEW: А если 25 столбцов и 20 вьюмоделей - будем свичи городить в каждой?
                //Надо подумать над инкапсуляцией
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
            Task task1 = Task.Run(() =>
            {
                Author selectedAuthor = _service.GetAuthorByID(int.Parse(notificationMessage.Notification));
                FirstName = selectedAuthor.FirstName;
                Id = selectedAuthor.Id;
                SecondName = selectedAuthor.SecondName;
                LastName = selectedAuthor.LastName;
                Books = new ObservableCollection<Book>(selectedAuthor.books);
                ObservableCollection<Book> TempBooks = new ObservableCollection<Book>(_service.GetAllBooks());
                AllBooks = new ObservableCollection<Book>(TempBooks.Where(n => !Books.Any(t => t.Id == n.Id)));
            });
        }
        public EditAuthorViewModel(IKartotekaService service, ILoggerService loggerService)
        {
            dialogCoordinator = DialogCoordinator.Instance;
            try
            {
                if (service == null) throw new ArgumentNullException("service", "service is null");
                _service = service;
                if (loggerService == null) throw new ArgumentNullException("loggerService", "loggerService is null");
                _loggingService = loggerService;
            }
            catch (ArgumentNullException ex)
            {
                _loggingService.LogError($"EditAuthorViewModel ctor can't get a service {ex}");
                MessageBox.Show("An exception just occurred: " + ex.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
            var messenger = SimpleIoc.Default.GetInstance<Messenger>(KartotekaConstants.EditAuthorMessengerKey);
            messenger.Register<NotificationMessage>(this, FillInformationAboutAuthor);
        }
    }
}