using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using Kartoteka.Domain;
using MahApps.Metro.Controls.Dialogs;
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
        private Book _selectedBook;
        public Book SelectedBook { get { return _selectedBook; } set { _selectedBook = value; RaisePropertyChanged("SelectedBook"); } }


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
                    _service.EditBook(SelectedBook);
                    await dialogCoordinator.ShowMessageAsync(this, "Книга изменена", String.Concat("ID измененной книги: ", SelectedBook.Id));
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
                    newEditWin.Show();
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
                    foreach(Author author in SelectedBook.authors)
                    {
                        AllAuthors.Add(author);
                    }
                    SelectedBook.authors.Clear();
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
                        SelectedBook.authors.Remove(author);
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
                        SelectedBook.authors.Add(author);
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
                            if (String.IsNullOrEmpty(SelectedBook.Name))
                                return "Заполните пустую строку";
                            break;
                        }
                    case "Description":
                        {
                            if (String.IsNullOrEmpty(SelectedBook.Description))
                                return "Заполните пустую строку";
                            break;
                        }
                    case "Year":
                        {
                            if (SelectedBook.Year > DateTime.Now.Year)
                                return "Такой год еще не наступил!";
                            break;
                        }
                }

                return string.Empty;
            }
        }
        public void FillInformationAboutAuthor(NotificationMessage notificationMessage)
        {
            string notification = notificationMessage.Notification;
            Book selectedBook = _service.GetBookByID(int.Parse(notification));
            SelectedBook = new Book()
            {
                Year = selectedBook.Year,
                Id = selectedBook.Id,
                Name = selectedBook.Name,
                Description = selectedBook.Description,
                authors = new ObservableCollection<Author>(selectedBook.authors)
            };
            AllAuthors = new ObservableCollection<Author>(_service.GetAllAuthors());
            foreach(Author author in SelectedBook.authors)
            {
                AllAuthors.Remove(author);
            }
        }
        public EditBookViewModel(IKartotekaService service)
        {
            dialogCoordinator = DialogCoordinator.Instance;
            if (service == null) throw new ArgumentNullException("service", "service is null");
            _service = service;
            var messenger = SimpleIoc.Default.GetInstance<Messenger>(KartotekaConstants.EditBookMessengerKey);
            messenger.Register<NotificationMessage>(this,FillInformationAboutAuthor );
            AllAuthors = new ObservableCollection<Author>(_service.GetAllAuthors());
        }
    }
}