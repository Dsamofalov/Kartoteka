using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Kartoteka.Domain;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace NewKartoteka
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class AddAuthorViewModel : ViewModelBase
    {
        private readonly IKartotekaService _service;
        private IDialogCoordinator dialogCoordinator;
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

        private RelayCommand<object> _saveAuthorCommand;
        public ICommand SaveAuthorCommand
        {
            get
            {
                if (_saveAuthorCommand == null) _saveAuthorCommand = new RelayCommand<object>(async (object parameter) =>
                {
                    IList selection = (IList)parameter;
                    List<Book> newbooks = selection.Cast<Book>().ToList();
                    Author author = new Author()
                    {
                        FirstName = FirstName,
                        SecondName = SecondName,
                        LastName = LastName,
                        books = new ObservableCollection<Book>()
                    };
                    foreach (Book book in newbooks)
                    {
                        author.books.Add(book);
                    }
                    await dialogCoordinator.ShowMessageAsync(this, "Автор добавлен", String.Concat("ID добавленного автора: ", _service.RegisterNewAuthor(author).ToString()));
                    MessengerInstance.Send(new NotificationMessage("RefreshListOfAuthors"));
                });

                return _saveAuthorCommand;
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
        public AddAuthorViewModel(IKartotekaService service)
        {
            dialogCoordinator = DialogCoordinator.Instance;
            if (service == null) throw new ArgumentNullException("service", "service is null");
            _service = service;
            this.AllBooks = new ObservableCollection<Book>(_service.GetAllBooks());
        }
    }
}