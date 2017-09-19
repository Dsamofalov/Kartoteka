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
    public class EditBookViewModel : ViewModelBase, IDataErrorInfo
    {
        private IDialogCoordinator dialogCoordinator;
        private readonly IKartotekaService _service;
        private Logger _logger = LogManager.GetCurrentClassLogger();
        private int Id;
        private string _name;
        public string Name { get { return _name; } set { _name = value; RaisePropertyChanged("Name"); } }

        private int _year;
        public int Year { get { return _year; } set { _year = value; RaisePropertyChanged("Year"); } }

        private string _description;
        public string Description { get { return _description; } set { _description = value; RaisePropertyChanged("Description"); } }

        private ObservableCollection<Author> _authors;
        public ObservableCollection<Author> Authors { get { return _authors; } set { _authors = value; RaisePropertyChanged("Authors"); } }


        private ObservableCollection<Author> _allAuthors;
        public ObservableCollection<Author> AllAuthors { get { return _allAuthors; } set { _allAuthors = value; RaisePropertyChanged("AllAuthors"); } }

        
        private RelayCommand _editBookCommand;
        private RelayCommand _openEditAuthorsCommand;
        private RelayCommand<Window> _closeEditAuthorsCommand;
        private RelayCommand _removeAllAuthorsCommand;
        private RelayCommand<object> _removeAuthorsCommand;
        private RelayCommand<object> _addAuthorsCommand;
        public ICommand EditBookCommand
        {
            get
            {
                if (_editBookCommand == null) _editBookCommand = new RelayCommand(async () =>
                {
                    _logger.Info($"EditBookCommand with book id: {Id} ");
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
                });

                return _editBookCommand;
            }
        }
        public ICommand OpenEditAuthorsCommand
        {
            get
            {
                if (_openEditAuthorsCommand == null) _openEditAuthorsCommand = new RelayCommand( () =>
                {
                    EditListOfAutorsWin newEditWin = new EditListOfAutorsWin();
                    newEditWin.ShowDialog();
                });

                return _openEditAuthorsCommand;
            }
        }
        public ICommand CloseEditAuthorsCommand
        {
            get
            {
                if (_closeEditAuthorsCommand == null) _closeEditAuthorsCommand = new RelayCommand<Window>((Window window) =>
                {
                    if (window != null)
                    {
                        window.Close();
                    }
                });

                return _closeEditAuthorsCommand;
            }
        }
        public ICommand RemoveAllAuthorsCommand
        {
            get
            {
                if (_removeAllAuthorsCommand == null) _removeAllAuthorsCommand = new RelayCommand(() =>
                {
                    foreach(Author author in Authors)
                    {
                        AllAuthors.Add(author);
                    }
                    Authors.Clear();
                });

                return _removeAllAuthorsCommand;
            }
        }
        public ICommand RemoveAuthorsCommand
        {
            get
            {
                if (_removeAuthorsCommand == null) _removeAuthorsCommand = new RelayCommand<object>( (object parameter) =>
                {
                    IList selection = (IList)parameter;
                    List<Author> newAuthors = selection.Cast<Author>().ToList();
                    foreach (Author author in newAuthors)
                    {
                        Authors.Remove(author);
                        AllAuthors.Add(author);
                    }
                });

                return _removeAuthorsCommand;
            }
        }
        public ICommand AddAuthorsCommand
        {
            get
            {
                if (_addAuthorsCommand == null) _addAuthorsCommand = new RelayCommand<object>((object parameter) =>
                {
                    IList selection = (IList)parameter;
                    List<Author> newAuthors = selection.Cast<Author>().ToList();
                    foreach (Author author in newAuthors)
                    {
                        Authors.Add(author);
                        AllAuthors.Remove(author);
                    }
                });

                return _addAuthorsCommand;
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
                            if (this.Year > DateTime.Now.Year)
                                return "Такой год еще не наступил!";
                            break;
                        }
                }

                return string.Empty;
            }
        }
        public void FillInformationAboutBook(NotificationMessage notificationMessage)
        {
            string notification = notificationMessage.Notification;
            Book selectedBook = _service.GetBookByID(int.Parse(notification));

            Year = selectedBook.Year;
            Id = selectedBook.Id;
            Name = selectedBook.Name;
            Description = selectedBook.Description;
                Authors = new ObservableCollection<Author>(selectedBook.authors);
            AllAuthors = new ObservableCollection<Author>(_service.GetAllAuthors());
            foreach(Author author in Authors)
            {
                AllAuthors.Remove(author);
            }
        }
        public EditBookViewModel(IKartotekaService service)
        {
            dialogCoordinator = DialogCoordinator.Instance;
            try
            {
                if (service == null) throw new ArgumentNullException("service", "service is null");
                _service = service;
            }
            catch (ArgumentNullException ex)
            {
                _logger.Error($"EditBookViewModel ctor can't get a service {ex}");
                MessageBox.Show("An exception just occurred: " + ex.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
            var messenger = SimpleIoc.Default.GetInstance<Messenger>(KartotekaConstants.EditBookMessengerKey);
            messenger.Register<NotificationMessage>(this,FillInformationAboutBook );
        }
    }
}