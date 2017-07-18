using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Kartoteka.Domain;
using NewKartoteka.Model;
using System;
using System.Collections.ObjectModel;
using MahApps.Metro.Controls;
using System.Windows;
using Microsoft.Practices.ServiceLocation;
using System.Windows.Input;

namespace NewKartoteka.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IKartotekaService _service;
        private ObservableCollection<Book> _books;
        private bool _isNewBookOpen =false;
        private RelayCommand _openAddBookWinCommand;
        public ICommand OpenAddBookWinCommand
        {
            get
            {
                if (_openAddBookWinCommand == null) _openAddBookWinCommand = new RelayCommand(() =>
                 {
                     IsNewBookOpen = !IsNewBookOpen;
                 });

                return _openAddBookWinCommand;
            }
        }


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

        public MainViewModel(IKartotekaService service)
        {
            if (service == null) throw new ArgumentNullException("service", "service is null");
            _service = service;
        }

    }
}