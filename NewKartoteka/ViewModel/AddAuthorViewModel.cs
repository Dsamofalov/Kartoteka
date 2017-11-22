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
    public class AddAuthorViewModel : CustomViewModelBase
    {
        public  const string Token = "f7a99adf-591d-4809-9477-0fe49fc51511";

        private readonly IKartotekaService _service;
        private readonly ILoggerService _loggingService;

        private IDialogCoordinator dialogCoordinator;
        private string _firstName;
        private string _secondName;
        private string _lastName;
        private ObservableCollection<Book> _books;
        private ObservableCollection<Book> _allBooks;
        private RelayCommand<IList> _saveAuthorCommand;
        [Required(ErrorMessage = "Введите имя")]
        public string FirstName { get { return _firstName; } set { _firstName = value; RaisePropertyChanged("FirstName"); } }
        [Required(ErrorMessage = "Введите отчество")]
        public string SecondName { get { return _secondName; } set { _secondName = value; RaisePropertyChanged("SecondName"); } }
        [Required(ErrorMessage = "Введите фамилию")]
        public string LastName { get { return _lastName; } set { _lastName = value; RaisePropertyChanged("LastName"); } }

        public ObservableCollection<Book> Books { get { return _books; } set { _books = value; RaisePropertyChanged("Books"); } }

        public ObservableCollection<Book> AllBooks { get { return _allBooks; } set { _allBooks = value; RaisePropertyChanged("AllBooks"); } }

        public ICommand SaveAuthorCommand
        {
            get
            {
                return _saveAuthorCommand;
            }
        }
        async private void SaveAuthor(IList selection)
        {
            _loggingService.LogInfo($"SaveAuthorCommand with {FirstName} {SecondName} {LastName}");
            Author author = new Author()
            {
                FirstName = FirstName,
                SecondName = SecondName,
                LastName = LastName,
                books = new ObservableCollection<Book>()
            };
            foreach (Book book in selection)
            {
                author.books.Add(book);
            }
            string id = _service.RegisterNewAuthor(author).ToString();
            ClearAddAuthorFlyout();
            await dialogCoordinator.ShowMessageAsync(this, "Автор добавлен", String.Concat("ID добавленного автора: ", id));
            MessengerInstance.Send(new NotificationMessage(id), Token);
        }
        private void  ClearAddAuthorFlyout()
        {
            FirstName = null;
            SecondName = null;
            LastName = null;
        }

        public AddAuthorViewModel(IKartotekaService service, ILoggerService loggerService)
        {
            dialogCoordinator = DialogCoordinator.Instance;
            try
            {
                if (service == null) throw new ArgumentNullException("service", "service is null");
                _service = service;
                if (loggerService == null) throw new ArgumentNullException("loggerService", "loggerService is null");
                _loggingService = loggerService;
            }
            catch(ArgumentNullException ex)
            {
                _loggingService.LogError($"AddAuthorViewModel ctor can't get a service {ex}");
                MessageBox.Show("An exception just occurred: " + ex.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);        
            }
            _saveAuthorCommand = new RelayCommand<IList>(SaveAuthor);
            MessengerInstance.Register<NotificationMessage>(this, message =>
            {
                if(message.Notification == ConfigurationManager.AppSettings["ClearAddAuthorFlyoutKey"])
                {
                    ClearAddAuthorFlyout();
                }
            });
            MessengerInstance.Register<NotificationMessage>(this, message =>
            {
                if (message.Notification == ConfigurationManager.AppSettings["UpdateAddAuthorFlyoutKey"])
                {
                    Task.Run(() =>
                    {
                        this.AllBooks = new ObservableCollection<Book>(_service.GetAllBooks());
                    });
                }
            });
        }
    }
}