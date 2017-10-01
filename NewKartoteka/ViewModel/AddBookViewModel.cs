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
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NewKartoteka 
{
    public class AddBookViewModel:ViewModelBase, IDataErrorInfo
    {
        private readonly IKartotekaService _service;
        private IDialogCoordinator dialogCoordinator;
        public static readonly Guid Token = Guid.NewGuid();
        private readonly ILoggerService _loggingService;
        private string _name;
        public string Name { get { return _name; } set { _name = value; RaisePropertyChanged("Name"); } }

        private int _year;
        public int Year { get { return _year; } set { _year = value; RaisePropertyChanged("Year"); } }

        private string _description;
        public string Description { get { return _description; } set { _description = value; RaisePropertyChanged("Description"); } }

        private ObservableCollection<Author> _authors;
        public ObservableCollection<Author> Authors { get { return _authors; } set { _authors = value; RaisePropertyChanged("Authors"); } }

        private ObservableCollection<Author> _allauthors;
        public ObservableCollection<Author> AllAuthors { get { return _allauthors; } set { _allauthors = value; RaisePropertyChanged("AllAuthors"); } }

        private RelayCommand<object> _saveBookCommand;
        public ICommand SaveBookCommand
        {
            get
            {
                if (_saveBookCommand == null) _saveBookCommand = new RelayCommand<object>(async (object parameter) =>
                {
                    _loggingService.LogInfo($"SaveBookCommand with {Name} {Year} {Description} {parameter}");
                    IList selection = (IList)parameter;
                    List<Author> newAuthors = selection.Cast<Author>().ToList();
                    Book book = new Book()
                    {
                        Name = Name,
                        Year = Year,
                        Description = Description,
                        authors = new ObservableCollection<Author>()
                    };
                    foreach (Author author in newAuthors)
                    {
                        book.authors.Add(author);
                    }
                    string id = _service.RegisterNewBook(book).ToString();
                    await dialogCoordinator.ShowMessageAsync(this, "Книга добавлена", String.Concat("ID добавленной книги: ", id));
                    ClearAddBookFlyout();
                    MessengerInstance.Send(new NotificationMessage(id),Token);
                });

                return _saveBookCommand;
            }
        }
        private void ClearAddBookFlyout()
        {
            Year = 0;
            Name = null;
            Description = null;
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
                switch(columnName)
                {
                    case "Name":
                        {
                            if (String.IsNullOrEmpty(this.Name))
                                return "Заполните пустую строку";
                            break;
                        }
                    case "Description":
                        {
                            if (String.IsNullOrEmpty(this.Description))
                                return "Заполните пустую строку";
                            break;
                        }
                    case "Year":
                        {
                            if (Year>DateTime.Now.Year)
                                return "Такой год еще не наступил!";
                            break;
                        }
                }

                return string.Empty;
            }
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
            MessengerInstance.Register<NotificationMessage>(this, message =>
            {
                if (message.Notification.ToString() == "ClearAddBookFlyout")
                {
                    ClearAddBookFlyout();
                }
            });
            var messenger = SimpleIoc.Default.GetInstance<Messenger>(KartotekaConstants.AddBookMessengerKey);
            messenger.Register<NotificationMessage>(this, action =>
            {
                this.AllAuthors = new ObservableCollection<Author>(_service.GetAllAuthors());
            });
        }
    }
}