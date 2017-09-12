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
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class EditAuthorViewModel : ViewModelBase, IDataErrorInfo
    {
        private IDialogCoordinator dialogCoordinator;
        private readonly IKartotekaService _service;
        private Author _selectedAuthor;
        public Author SelectedAuthor { get { return _selectedAuthor; } set { _selectedAuthor = value; RaisePropertyChanged("SelectedAuthor"); } }


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
                    _service.EditAuthor(SelectedAuthor);
                    await dialogCoordinator.ShowMessageAsync(this, "Автор изменен", String.Concat("ID измененного автора: ", SelectedAuthor.Id));
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
                    foreach (Book book in SelectedAuthor.books)
                    {
                        AllBooks.Add(book);
                    }
                    SelectedAuthor.books.Clear();
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
                        SelectedAuthor.books.Remove(book);
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
                        SelectedAuthor.books.Add(book);
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
                            if (String.IsNullOrEmpty(SelectedAuthor.FirstName))
                                return "Заполните пустую строку";
                            break;
                        }
                    case "SecondName":
                        {
                            if (String.IsNullOrEmpty(SelectedAuthor.SecondName))
                                return "Заполните пустую строку";
                            break;
                        }
                    case "LastName":
                        {
                            if (String.IsNullOrEmpty(SelectedAuthor.LastName))
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
            SelectedAuthor = new Author()
            {
                FirstName = selectedAuthor.FirstName,
                Id = selectedAuthor.Id,
                SecondName = selectedAuthor.SecondName,
                LastName = selectedAuthor.LastName,
                books = new ObservableCollection<Book>(selectedAuthor.books)
            };
            AllBooks = new ObservableCollection<Book>(_service.GetAllBooks());
            foreach (Book book in SelectedAuthor.books)
            {
                AllBooks.Remove(book);
            }
        }
        public EditAuthorViewModel(IKartotekaService service)
        {
            dialogCoordinator = DialogCoordinator.Instance;
            if (service == null) throw new ArgumentNullException("service", "service is null");
            _service = service;
            var messenger = SimpleIoc.Default.GetInstance<Messenger>(KartotekaConstants.EditAuthorMessengerKey);
            messenger.Register<NotificationMessage>(this, FillInformationAboutAuthor);
        }
    }
}