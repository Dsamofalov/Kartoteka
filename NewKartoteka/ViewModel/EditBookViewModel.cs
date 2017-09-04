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
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace NewKartoteka
{
    public class EditBookViewModel : ViewModelBase
    {
        private IDialogCoordinator dialogCoordinator;
        private readonly IKartotekaService _service;

        private ObservableCollection<Author> _authors;
        public ObservableCollection<Author> Authors { get { return _authors; } set { _authors = value; RaisePropertyChanged("Authors"); } }

        private ObservableCollection<Author> _authorstoadd;
        public ObservableCollection<Author> AuthorsToAdd { get { return _authorstoadd; } set { _authorstoadd = value; RaisePropertyChanged("AuthorsToAdd"); } }
        public void FillListsOfAutors(NotificationMessage notificationMessage)
        {
            string notification = notificationMessage.Notification;
            Book SelectedBook = _service.GetBookByID(int.Parse(notification));
            Authors = new ObservableCollection<Author>();
            AuthorsToAdd = new ObservableCollection<Author>();
            ObservableCollection<Author> AllAuthors = new ObservableCollection<Author>(_service.GetAllAuthors());
            foreach (Author author in AllAuthors)
            {
                if(SelectedBook.authors.Contains(author))
                {
                    Authors.Add(author);
                }
                else
                {
                    AuthorsToAdd.Add(author);
                }
            }
        }
        private RelayCommand<object> _editBookCommand;
        public ICommand EditBookCommand
        {
            get
            {
                if (_editBookCommand == null) _editBookCommand = new RelayCommand<object>(async (object parameter) =>
                {
                    
                });

                return _editBookCommand;
            }
        }
        public EditBookViewModel(IKartotekaService service)
        {
            if (service == null) throw new ArgumentNullException("service", "service is null");
            _service = service;
            var messenger = SimpleIoc.Default.GetInstance<Messenger>("EditBookMessenger"); //используй ключ см MainViewModel строка 273
            messenger.Register<NotificationMessage>(this,FillListsOfAutors );
        }
    }
}