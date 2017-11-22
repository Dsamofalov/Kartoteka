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
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NewKartoteka
{
    public class EditAuthorViewModel : CustomViewModelBase
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
        [Required(ErrorMessage = "Введите имя")]
        public string FirstName { get { return _firstName; } set { _firstName = value; RaisePropertyChanged("FirstName"); } }
        [Required(ErrorMessage = "Введите отчество")]
        public string SecondName { get { return _secondName; } set { _secondName = value; RaisePropertyChanged("SecondName"); } }
        [Required(ErrorMessage = "Введите фамилию")]
        public string LastName { get { return _lastName; } set { _lastName = value; RaisePropertyChanged("LastName"); } }

        public ObservableCollection<Book> Books { get { return _books; } set { _books = value; RaisePropertyChanged("Books"); } }


        public ObservableCollection<Book> AllBooks { get { return _allBooks; } set { _allBooks = value; RaisePropertyChanged("AllBooks"); } }

        public ICommand EditAuthorCommand
        {
            get
            {
                return _editAuthorCommand;
            }
        }
        async private void EditAuthor()
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
        }
        public ICommand OpenEditBooksCommand
        {
            get
            {
                return _openEditBooksCommand;
            }
        }
        private void OpenEditBooks()
        {
            EditListOfBooksWin newEditWin = new EditListOfBooksWin();
            newEditWin.ShowDialog();
        }
        public ICommand CloseEditBooksCommand
        {
            get
            {
                return _closeEditBooksCommand;
            }
        }
        private void CloseEditBooks(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }
        public ICommand RemoveAllBooksCommand
        {
            get
            {
                return _removeAllBooksCommand;
            }
        }
        private void RemoveAllBooks()
        {
            foreach (Book book in Books)
            {
                AllBooks.Add(book);
            }
            Books.Clear();
        }
        public ICommand RemoveBooksCommand
        {
            get
            {
                return _removeBooksCommand;
            }
        }
        private void RemoveBooks(IList selection)
        {
            List<Book> newBooks = selection.Cast<Book>().ToList();
            foreach (Book book in newBooks)
            {
                Books.Remove(book);
                AllBooks.Add(book);
            }
        }
        public ICommand AddBooksCommand
        {
            get
            {
                return _addBooksCommand;
            }
        }
        private void AddBooks(IList selection)
        {
            List<Book> newBooks = selection.Cast<Book>().ToList();
            foreach (Book book in newBooks)
            {
                Books.Add(book);
                AllBooks.Remove(book);
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
            _editAuthorCommand = new RelayCommand(EditAuthor);
            _openEditBooksCommand = new RelayCommand(OpenEditBooks);
            _closeEditBooksCommand = new RelayCommand<Window>(CloseEditBooks);
            _removeAllBooksCommand = new RelayCommand(RemoveAllBooks);
            _removeBooksCommand = new RelayCommand<IList>(RemoveBooks);
            _addBooksCommand = new RelayCommand<IList>(AddBooks);
            var messenger = SimpleIoc.Default.GetInstance<Messenger>(ConfigurationManager.AppSettings["EditAuthorMessengerKey"]);
            messenger.Register<NotificationMessage>(this, FillInformationAboutAuthor);
        }
    }
}