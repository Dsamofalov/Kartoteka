using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Kartoteka.Domain;
using NewKartoteka.Model;
using System;
using System.Collections.ObjectModel;
using MahApps.Metro.Controls;
using System.Windows;
using Microsoft.Practices.ServiceLocation;

namespace NewKartoteka.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IKartotekaService _service;
        private ObservableCollection<Book> _books;
        private bool _isNewBookOpen =false;
        public RelayCommand OpenAddBookWinCommand { get; private set; }
        public ObservableCollection<Book> Books
        {
            get
            {
                return _books;
            }

            set
            {
                _books = value;
                RaisePropertyChanged("Books");
            }
        }

        public bool IsNewBookOpen
        {
            get
            {
                return _isNewBookOpen ;
            }

            set
            {
                _isNewBookOpen  = value;
                RaisePropertyChanged("IsNewBookOpen");
            }
        }


        private void OpenAddBookWin()
        {
            IsNewBookOpen = !IsNewBookOpen;
        }
        public AddBookViewModel AddBookVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddBookViewModel>();
            }
        }
        public MainViewModel(IKartotekaService service)
        {
            if (service == null) throw new ArgumentNullException("service", "service is null");
            _service = service;
            this.OpenAddBookWinCommand = new RelayCommand(OpenAddBookWin);
        }

    }
}