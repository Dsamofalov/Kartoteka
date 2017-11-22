using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using Kartoteka.DAL;
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
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NewKartoteka 
{
    public class AddBookViewModel:CustomViewModelBase
    {
        public const string Token = "bffac408-3e7a-49e0-8fd0-5f20706457fc";

        private readonly IKartotekaService _service;
        private readonly ILoggerService _loggingService;

        private IDialogCoordinator dialogCoordinator;
        private string _name;
        private int _year;
        private string _description;
        private ObservableCollection<Author> _authors;
        private ObservableCollection<Author> _allauthors;
        private RelayCommand<IList> _saveBookCommand;
        [Required(ErrorMessage = "Введите название")]
        public string Name { get { return _name; } set { _name = value; RaisePropertyChanged("Name"); } }
        [Required(ErrorMessage = "Введите год")]
        [ValidCurrentYear(ErrorMessage ="Такой год еще не наступил!")]
        public int Year { get { return _year; } set { _year = value; RaisePropertyChanged("Year"); } }
        [Required(ErrorMessage = "Введите описание")]
        public string Description { get { return _description; } set { _description = value; RaisePropertyChanged("Description"); } }

        public ObservableCollection<Author> Authors { get { return _authors; } set { _authors = value; RaisePropertyChanged("Authors"); } }

        public ObservableCollection<Author> AllAuthors { get { return _allauthors; } set { _allauthors = value; RaisePropertyChanged("AllAuthors"); } }

        public ICommand SaveBookCommand
        {
            get
            {
                return _saveBookCommand;
            }
        }
        async private void SaveBook(IList selection)
        {
            _loggingService.LogInfo($"SaveBookCommand with {Name} {Year} {Description} ");
            Book book = new Book()
            {
                Name = Name,
                Year = Year,
                Description = Description,
                authors = new ObservableCollection<Author>()
            };
            foreach (Author author in selection)
            {
                book.authors.Add(author);
            }
            string id = _service.RegisterNewBook(book).ToString();
            await dialogCoordinator.ShowMessageAsync(this, "Книга добавлена", String.Concat("ID добавленной книги: ", id));
            ClearAddBookFlyout();
            MessengerInstance.Send(new NotificationMessage(id), Token);
        }
        private void ClearAddBookFlyout()
        {
            Year = 0;
            Name = null;
            Description = null;
        }

        public AddBookViewModel(IKartotekaService service, ILoggerService loggerService)
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
                _loggingService.LogError($"AddBookViewModel ctor can't get a service {ex}");
                MessageBox.Show("An exception just occurred: " + ex.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
            _saveBookCommand = new RelayCommand<IList>(SaveBook);
            MessengerInstance.Register<NotificationMessage>(this, message =>
            {
                if (message.Notification == ConfigurationManager.AppSettings["ClearAddBookFlyoutKey"])
                {
                    ClearAddBookFlyout();
                }
            });
            MessengerInstance.Register<NotificationMessage>(this, message =>
            {
                if (message.Notification == ConfigurationManager.AppSettings["UpdateAddBookFlyoutKey"])
                {
                    Task.Run(() =>
                    {
                        this.AllAuthors = new ObservableCollection<Author>(_service.GetAllAuthors());
                    });
                }
            });
        }
    }
}