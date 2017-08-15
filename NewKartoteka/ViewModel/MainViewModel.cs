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
using Kartoteka.DAL;
using System.ComponentModel;
using System.Windows.Data;

namespace NewKartoteka.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IKartotekaService _service;
        private ObservableCollection<Book> _books;
        private bool _isNewBookOpen =false;
        private bool _isEditBookOpen = false;
        private ICollectionView _dataGridCollection;
        private string _filterString;
        private RelayCommand _openAddBookWinCommand;
        private RelayCommand<Book> _openEditBookWinCommand;
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
        public ICommand OpenEditBookWinCommand
        {
            get
            {
                if (_openEditBookWinCommand == null) _openEditBookWinCommand = new RelayCommand<Book>((Book BookToEdit) =>
                {
                    IsEditBookOpen = !IsEditBookOpen;
                });

                return _openEditBookWinCommand;
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

        public bool IsEditBookOpen
        {
            get
            {
                return _isEditBookOpen;
            }

            set
            {
                _isEditBookOpen = value;
                RaisePropertyChanged("IsEditBookOpen");
            }
        }
        public ICollectionView DataGridCollection
        {
            get { return _dataGridCollection; }
            set { _dataGridCollection = value; RaisePropertyChanged("DataGridCollection"); }
        }
        public string FilterString
        {
            get { return _filterString; }
            set
            {
                _filterString = value;
                RaisePropertyChanged("FilterString");
                FilterCollection();
            }
        }

        private void FilterCollection()
        {
            if (_dataGridCollection != null)
            {
                _dataGridCollection.Refresh();
            }
        }

        public bool Filter(object obj)
        {
            var data = obj as Book;
            if (data != null)
            {
                if (!string.IsNullOrEmpty(_filterString))
                {
                    return data.Name.Contains(_filterString) || data.Description.Contains(_filterString) || data.Year.ToString().Contains(_filterString) || data.Id.ToString().Contains(_filterString);
                }
                return true;
            }
            return false;
        }
        public MainViewModel(IKartotekaService service)
        {
            if (service == null) throw new ArgumentNullException("service", "service is null");
            _service = service;
            Books = new ObservableCollection<Book>(_service.GetAllBooks());
            DataGridCollection = CollectionViewSource.GetDefaultView(Books);
            DataGridCollection.Filter = new Predicate<object>(Filter);
        }

    }
}