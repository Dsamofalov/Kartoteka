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
using System.Configuration;
using System.Windows.Input;
using System.ComponentModel.DataAnnotations;

namespace NewKartoteka
{
    public class EditBookViewModel : CustomViewModelBase
    {
        private readonly IKartotekaService _service;
        private readonly ILoggerService _loggingService;

        private IDialogCoordinator dialogCoordinator;
        private int Id;
        private string _name;
        private int _year;
        private string _description;
        private ObservableCollection<Author> _authors;
        private ObservableCollection<Author> _allAuthors;
        private RelayCommand _editBookCommand;
        private RelayCommand _openEditAuthorsCommand;
        private RelayCommand<Window> _closeEditAuthorsCommand;
        private RelayCommand _removeAllAuthorsCommand;
        private RelayCommand<IList> _removeAuthorsCommand;
        private RelayCommand<IList> _addAuthorsCommand;
        [Required(ErrorMessage = "Введите название")]
        public string Name { get { return _name; } set { _name = value; RaisePropertyChanged("Name"); } }
        [Required(ErrorMessage = "Введите год")]
        [ValidCurrentYear(ErrorMessage = "Такой год еще не наступил!")]
        public int Year { get { return _year; } set { _year = value; RaisePropertyChanged("Year"); } }
        [Required(ErrorMessage = "Введите описание")]
        public string Description { get { return _description; } set { _description = value; RaisePropertyChanged("Description"); } }

        public ObservableCollection<Author> Authors { get { return _authors; } set { _authors = value; RaisePropertyChanged("Authors"); } }


        public ObservableCollection<Author> AllAuthors { get { return _allAuthors; } set { _allAuthors = value; RaisePropertyChanged("AllAuthors"); } }

        public ICommand EditBookCommand
        {
            get
            {
                return _editBookCommand;
            }
        }
        async private void EditBook()
        {
            _loggingService.LogInfo($"EditBookCommand with book id: {Id} ");
            Book selectedBook = new Book()
            {
                Year = Year,
                Id = Id,
                Name = Name,
                Description = Description,
                authors = new ObservableCollection<Author>(Authors)
            };
            _service.EditBook(selectedBook);
            await dialogCoordinator.ShowMessageAsync(this, "Книга изменена", String.Concat("ID измененной книги: ", selectedBook.Id));
        }
        public ICommand OpenEditAuthorsCommand
        {
            get
            {
                return _openEditAuthorsCommand;
            }
        }
        private void OpenEditAuthors()
        {
            EditListOfAutorsWin newEditWin = new EditListOfAutorsWin();
            newEditWin.ShowDialog();
        }
        public ICommand CloseEditAuthorsCommand
        {
            get
            {
                return _closeEditAuthorsCommand;
            }
        }
        private void CloseEditAuthors(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }
        public ICommand RemoveAllAuthorsCommand
        {
            get
            {
                return _removeAllAuthorsCommand;
            }
        }
        private void RemoveAllAuthors()
        {
            foreach (Author author in Authors)
            {
                AllAuthors.Add(author);
            }
            Authors.Clear();
        }
        public ICommand RemoveAuthorsCommand
        {
            get
            {
                return _removeAuthorsCommand;
            }
        }
        private void RemoveAuthors(IList selection)
        {
            List<Author> newAuthors = selection.Cast<Author>().ToList();
            foreach (Author author in newAuthors)
            {
                Authors.Remove(author);
                AllAuthors.Add(author);
            }
        }
        public ICommand AddAuthorsCommand
        {
            get
            {
                return _addAuthorsCommand;
            }
        }
        private void AddAuthors(IList selection)
        {
            List<Author> newAuthors = selection.Cast<Author>().ToList();
            foreach (Author author in newAuthors)
            {
                Authors.Add(author);
                AllAuthors.Remove(author);
            }
        }
        public void FillInformationAboutBook(NotificationMessage notificationMessage)
        {
            Task task1 = Task.Run(() =>
            {
                Book selectedBook = _service.GetBookByID(int.Parse(notificationMessage.Notification));
                Year = selectedBook.Year;
                Id = selectedBook.Id;
                Name = selectedBook.Name;
                Description = selectedBook.Description;
                Authors = new ObservableCollection<Author>(selectedBook.authors);
                ObservableCollection<Author> TempAuthors = new ObservableCollection<Author>(_service.GetAllAuthors());
                AllAuthors = new ObservableCollection<Author>(TempAuthors.Where(n => !Authors.Any(t => t.Id == n.Id)));
            });
        }
        public EditBookViewModel(IKartotekaService service, ILoggerService loggerService)
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
                _loggingService.LogError($"EditBookViewModel ctor can't get a service {ex}");
                MessageBox.Show("An exception just occurred: " + ex.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
            _editBookCommand = new RelayCommand(EditBook);
            _openEditAuthorsCommand = new RelayCommand(OpenEditAuthors);
            _closeEditAuthorsCommand = new RelayCommand<Window>(CloseEditAuthors);
            _removeAllAuthorsCommand = new RelayCommand(RemoveAllAuthors);
            _removeAuthorsCommand = new RelayCommand<IList>(RemoveAuthors);
            _addAuthorsCommand = new RelayCommand<IList>(AddAuthors);
            var messenger = SimpleIoc.Default.GetInstance<Messenger>(ConfigurationManager.AppSettings["EditBookMessengerKey"]);
            messenger.Register<NotificationMessage>(this, FillInformationAboutBook);
        }
    }
}