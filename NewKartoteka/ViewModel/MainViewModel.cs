using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Kartoteka.Domain;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Data;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Forms;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Configuration;

namespace NewKartoteka.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IKartotekaService _service;
        private readonly ILoggerService _loggingService;

        private ObservableCollection<Book> _books;
        private ObservableCollection<Author> _authors;
        private bool _isNewBookOpen = false;
        private bool _isNewAuthorOpen = false;
        private bool _isEditBookOpen = false;
        private bool _isEditAuthorOpen = false;
        private bool _isPreloaderActive = true;
        private bool _isDataGridActive = false;
        private bool _isFoldersPreloaderActive = true;
        private bool _isListOfFoldersActive = false;
        private ICollectionView _booksDataGridCollection;
        private ICollectionView _authorsDataGridCollection;
        private string _filterBooksString;
        private string _filterAuthorsString;
        private RelayCommand _openAddBookWinCommand;
        private RelayCommand _clearAddBookFlyoutCommand;
        private RelayCommand _exportBooksToXlsxCommand;
        private RelayCommand<Book> _openEditBookWinCommand;
        private RelayCommand<Book> _removeBookWinCommand;
        private RelayCommand _openAddAuthorWinCommand;
        private RelayCommand _exportAuthorsToXlsxCommand;
        private RelayCommand _clearAddAuthorFlyoutCommand;
        private RelayCommand<Author> _openEditAuthorWinCommand;
        private RelayCommand<Author> _removeAuthorWinCommand;
        private RelayCommand _prepareForExportBooksToDriveCommand;
        private RelayCommand _prepareForExportAuthorsToDriveCommand;
        private RelayCommand<object> _exportFileToGoogleDriveCommand;
        private RelayCommand _exportFileToRootCommand;
        private List<string> _folders;
        private IDialogCoordinator dialogCoordinator;
        private ChooseGoogleDriveDirWin ChooseDir;
        private string TypeOfExportData;
        private Dictionary<string, string> FoldersAndId;
        private Dispatcher _dispatcher;

        public ObservableCollection<Book> Books
        {
            get { return _books; }
            set { _books = value; RaisePropertyChanged("Books"); }
        }

        public ObservableCollection<Author> Authors
        {
            get { return _authors; }
            set { _authors = value; RaisePropertyChanged("Authors"); }
        }

        public bool IsNewBookOpen
        {
            get { return _isNewBookOpen; }
            set { _isNewBookOpen = value; RaisePropertyChanged("IsNewBookOpen"); }
        }

        public bool IsEditBookOpen
        {
            get { return _isEditBookOpen; }
            set { _isEditBookOpen = value; RaisePropertyChanged("IsEditBookOpen"); }
        }
        public bool IsNewAuthorOpen
        {
            get { return _isNewAuthorOpen; }
            set { _isNewAuthorOpen = value; RaisePropertyChanged("IsNewAuthorOpen"); }
        }
        public bool IsEditAuthorOpen
        {
            get { return _isEditAuthorOpen; }
            set { _isEditAuthorOpen = value; RaisePropertyChanged("IsEditAuthorOpen"); }
        }
        public bool IsPreloaderActive
        {
            get { return _isPreloaderActive; }
            set { _isPreloaderActive = value; RaisePropertyChanged("IsPreloaderActive"); }
        }
        public bool IsDataGridActive
        {
            get { return _isDataGridActive; }
            set { _isDataGridActive = value; RaisePropertyChanged("IsDataGridActive"); }
        }
        public bool IsFoldersPreloaderActive
        {
            get { return _isFoldersPreloaderActive; }
            set { _isFoldersPreloaderActive = value; RaisePropertyChanged("IsFoldersPreloaderActive"); }
        }
        public bool IsListOfFoldersActive
        {
            get { return _isListOfFoldersActive; }
            set { _isListOfFoldersActive = value; RaisePropertyChanged("IsListOfFoldersActive"); }
        }
        public List<string> Folders
        {
            get { return _folders; }
            set { _folders = value; RaisePropertyChanged("Folders"); }
        }
        public ICollectionView BooksDataGridCollection
        {
            get { return _booksDataGridCollection; }
            set { _booksDataGridCollection = value; RaisePropertyChanged("BooksDataGridCollection"); }
        }
        public ICollectionView AuthorsDataGridCollection
        {
            get { return _authorsDataGridCollection; }
            set { _authorsDataGridCollection = value; RaisePropertyChanged("AuthorsDataGridCollection"); }
        }
        public string FilterBooksString
        {
            get { return _filterBooksString; }
            set
            {
                _filterBooksString = value;
                RaisePropertyChanged("FilterBooksString");
                FilterBooksCollection();
            }
        }
        public string FilterAuthorsString
        {
            get { return _filterAuthorsString; }
            set
            {
                _filterAuthorsString = value;
                RaisePropertyChanged("FilterAuthorsString");
                FilterAuthorsCollection();
            }
        }
        public ICommand OpenAddBookWinCommand
        {
            get
            {
                return _openAddBookWinCommand;
            }
        }
        private void OpenAddBookWin()
        {
            Messenger.Default.Send(new NotificationMessage(ConfigurationManager.AppSettings["UpdateAddBookFlyoutKey"]));
            IsNewBookOpen = true;
        }
        public ICommand OpenAddAuthorWinCommand
        {
            get
            {
                return _openAddAuthorWinCommand;
            }
        }
        private void OpenAddAuthorWin()
        {
            Messenger.Default.Send(new NotificationMessage(ConfigurationManager.AppSettings["UpdateAddAuthorFlyoutKey"]));
            IsNewAuthorOpen = true;
        }
        public ICommand ClearAddAuthorFlyoutCommand
        {
            get
            {
                return _clearAddAuthorFlyoutCommand;
            }
        }
        private void ClearAddAuthorFlyout()
        {
            Messenger.Default.Send(new NotificationMessage(ConfigurationManager.AppSettings["ClearAddAuthorFlyoutKey"]));
            IsNewAuthorOpen = false;
        }
        public ICommand ClearAddBookFlyoutCommand
        {
            get
            {
                return _clearAddBookFlyoutCommand;
            }
        }
        private void ClearAddBookFlyout()
        {
            Messenger.Default.Send(new NotificationMessage(ConfigurationManager.AppSettings["ClearAddBookFlyoutKey"]));
            IsNewBookOpen = false;
        }
        public ICommand OpenEditBookWinCommand
        {
            get
            {
                return _openEditBookWinCommand;
            }
        }
        private void OpenEditBookWin(Book bookToEdit)
        {
            IsEditBookOpen = !IsEditBookOpen;
            ViewModelLocator._editBookMessenger.Send(new NotificationMessage(bookToEdit.Id.ToString()));
        }
        public ICommand OpenEditAuthorWinCommand
        {
            get
            {
                return _openEditAuthorWinCommand;
            }
        }
        private void OpenEditAuthorWin(Author authorToEdit)
        {
            IsEditAuthorOpen = !IsEditAuthorOpen;
            ViewModelLocator._editAuthorMessenger.Send(new NotificationMessage(authorToEdit.Id.ToString()));
        }
        public ICommand RemoveBookWinCommand
        {
            get
            {
                return _removeBookWinCommand;
            }
        }
        async private void RemoveBookWin(Book bookToRemove)
        {
            _service.DeleteBook(bookToRemove.Id);
            Books.Remove(bookToRemove);
            FilterBooksCollection();
            await dialogCoordinator.ShowMessageAsync(this, "Книга удалена", String.Concat("ID удаленной книги: ", bookToRemove.Id.ToString()));
        }
        public ICommand RemoveAuthorWinCommand
        {
            get
            {
                return _removeAuthorWinCommand;
            }
        }
        async private void RemoveAuthorWin(Author authorToRemove)
        {
            _service.DeleteAuthor(authorToRemove.Id);
            Authors.Remove(authorToRemove);
            FilterAuthorsCollection();
            await dialogCoordinator.ShowMessageAsync(this, "Автор удален", String.Concat("ID удаленного автора: ", authorToRemove.Id.ToString()));
        }
        public ICommand ExportBooksToXlsxCommand
        {
            get
            {
                return _exportBooksToXlsxCommand;
            }
        }
        async private void ExportBooksToXlsx()
        {
            string filePath = GetPathToExcel();
            if (filePath != null)
            {
                MessageDialogResult result = await dialogCoordinator.ShowMessageAsync(this, "Подтверждение", "Вы действительно хотите перезаписать данный файл? ",
                 MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AnimateShow = false, ColorScheme = MetroDialogColorScheme.Theme });
                if (result == MessageDialogResult.Affirmative)
                {
                    await Task.Run(() =>
                    {
                        var exportData = new ExportData();
                        exportData = _service.ExportBooksData();
                        exportData.FileName = filePath;
                        File.WriteAllBytes(exportData.FileName, exportData.Data);
                    });
                    await dialogCoordinator.ShowMessageAsync(this, "Успешно сохранено", String.Concat("Книги сохранены в файл: ", filePath));
                    _loggingService.LogInfo($" Books saved to {filePath}");
                }
            }
        }
        public ICommand ExportAuthorsToXlsxCommand
        {
            get
            {
                return _exportAuthorsToXlsxCommand;
            }
        }
        async private void ExportAuthorsToXlsx()
        {
            string filePath = GetPathToExcel();
            if (filePath != null)
            {
                MessageDialogResult result = await dialogCoordinator.ShowMessageAsync(this, "Подтверждение", "Вы действительно хотите перезаписать данный файл? ",
                MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AnimateShow = false, ColorScheme = MetroDialogColorScheme.Theme });
                if (result == MessageDialogResult.Affirmative)
                {
                    await Task.Run(() =>
                    {
                        var exportData = new ExportData();
                        exportData = _service.ExportAuthorsData();
                        exportData.FileName = filePath;
                        File.WriteAllBytes(exportData.FileName, exportData.Data);
                    });
                    await dialogCoordinator.ShowMessageAsync(this, "Успешно сохранено", String.Concat("Авторы сохранены в файл: ", filePath));
                    _loggingService.LogInfo($" Authors saved to {filePath}");
                }
            }
        }
        public ICommand PrepareForExportBooksToDriveCommand
        {
            get
            {
                return _prepareForExportBooksToDriveCommand;
            }
        }
        private void PrepareForExportBooksToDrive()
        {
            Task.Run(() =>
            {
                FoldersAndId = new Dictionary<string, string>(_service.GetFolders());
                Folders = new List<string>(FoldersAndId.Values);
                IsFoldersPreloaderActive = false;
                IsListOfFoldersActive = true;
            });
            TypeOfExportData = "Books";
            ChooseDir = new ChooseGoogleDriveDirWin();
            ChooseDir.ShowDialog();
        }
        public ICommand PrepareForExportAuthorsToDriveCommand
        {
            get
            {
                return _prepareForExportAuthorsToDriveCommand;
            }
        }
        private void PrepareForExportAuthorsToDrive()
        {
            Task.Run(() =>
            {
                FoldersAndId = new Dictionary<string, string>(_service.GetFolders());
                Folders = new List<string>(FoldersAndId.Values);
                IsFoldersPreloaderActive = false;
                IsListOfFoldersActive = true;
            });
            TypeOfExportData = "Authors";
            ChooseDir = new ChooseGoogleDriveDirWin();
            ChooseDir.ShowDialog();
        }
        public ICommand ExportFileToRootCommand
        {
            get
            {
                return _exportFileToRootCommand;
            }
        }
        async private void ExportFileToRoot()
        {
            ChooseDir.Close();
            MessageDialogResult result = await dialogCoordinator.ShowMessageAsync(this, "Подтверждение", "Вы действительно хотите сохранить данный файл в корневую папку? ",
                MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AnimateShow = false, ColorScheme = MetroDialogColorScheme.Theme });
            if (result == MessageDialogResult.Affirmative)
            {
                await Task.Run(() =>
                {
                    if (TypeOfExportData == "Books")
                    {
                        _service.ExportBooksToDataDrive("root");
                    }
                    else
                    {
                        _service.ExportAuthorsToDataDrive("root");
                    }
                });
                await dialogCoordinator.ShowMessageAsync(this, "Успешно сохранено", "Данные успешно сохранены в корневую папку");
                _loggingService.LogInfo($" File saved to root");
            }
        }
        public ICommand ExportFileToGoogleDriveCommand
        {
            get
            {
                return _exportFileToGoogleDriveCommand;
            }
        }
        async private void ExportFileToGoogleDrive(object folder)
        {
            IList selection = (IList)folder;
            List<string> folders = selection.Cast<string>().ToList();
            ChooseDir.Close();
            string keyId = null;
            foreach (KeyValuePair<string, string> keyPair in FoldersAndId)
            {
                if (keyPair.Value == folders.First())
                {
                    keyId = keyPair.Key;
                    break;
                }
            }
            MessageDialogResult result = await dialogCoordinator.ShowMessageAsync(this, "Подтверждение", "Вы действительно хотите сохранить данный файл? ",
                MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AnimateShow = false, ColorScheme = MetroDialogColorScheme.Theme });
            if (result == MessageDialogResult.Affirmative)
            {
                if (keyId != null)
                {
                    await Task.Run(() =>
                    {
                        if (TypeOfExportData == "Books")
                        {
                            _service.ExportBooksToDataDrive(keyId);
                        }
                        else
                        {
                            _service.ExportAuthorsToDataDrive(keyId);
                        }
                    });
                    await dialogCoordinator.ShowMessageAsync(this, "Успешно сохранено", String.Concat("Данные успешно сохранены в папку: ", folders.First()));
                    _loggingService.LogInfo($" File saved to {folders.First()}");
                }
            }
        }
        private void FilterBooksCollection()
        {
            if (_booksDataGridCollection != null)
            {
                _booksDataGridCollection.Refresh();
            }
        }
        private void FilterAuthorsCollection()
        {
            if (_authorsDataGridCollection != null)
            {
                _authorsDataGridCollection.Refresh();
            }
        }
        public bool FilterBooks(object obj)
        {
            var data = obj as Book;
            if (data != null)
            {
                if (!string.IsNullOrWhiteSpace(_filterBooksString))
                {
                    return data.Name.Contains(_filterBooksString) || data.Description.Contains(_filterBooksString) || data.Year.ToString().Contains(_filterBooksString) || data.Id.ToString().Contains(_filterBooksString);
                }
                return true;
            }
            return false;
        }
        public bool FilterAuthors(object obj)
        {
            var data = obj as Author;
            if (data != null)
            {
                if (!string.IsNullOrWhiteSpace(_filterAuthorsString))
                {
                    return data.FirstName.Contains(_filterAuthorsString) || data.SecondName.Contains(_filterAuthorsString) || data.LastName.Contains(_filterAuthorsString) || data.Id.ToString().Contains(_filterAuthorsString);
                }
                return true;
            }
            return false;
        }
        private string GetPathToExcel()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Файлы Excel (*.xls; *.xlsx) | *.xls;  *.xlsx";
            openFileDialog.CheckFileExists = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                return openFileDialog.FileName;
            }
            return null;

        }
        public MainViewModel(IKartotekaService service, ILoggerService loggerService)
        {
            try
            {
                if (service == null) throw new ArgumentNullException("service", "service is null");
                _service = service;
                if (loggerService == null) throw new ArgumentNullException("loggerService", "loggerService is null");
                _loggingService = loggerService;
            }
            catch (ArgumentNullException ex)
            {
                _loggingService.LogError($" MainViewModel ctor can't get a service {ex}");
                System.Windows.MessageBox.Show("An exception just occurred: " + ex.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
            _dispatcher = Dispatcher.CurrentDispatcher;
            Task task1 = Task.Run(() =>
            {
                Books = new ObservableCollection<Book>(_service.GetAllBooks());
            });
            Task task2 = Task.Run(() =>
            {
                Authors = new ObservableCollection<Author>(_service.GetAllAuthors());
            });
            dialogCoordinator = DialogCoordinator.Instance;
            MessengerInstance.Register<NotificationMessage>(this, AddAuthorViewModel.Token, message =>
            {
                Authors.Add(_service.GetAuthorByID(int.Parse(message.Notification)));
                FilterAuthorsCollection();
            });
            MessengerInstance.Register<NotificationMessage>(this, AddBookViewModel.Token, message =>
            {
                Books.Add(_service.GetBookByID(int.Parse(message.Notification)));
                FilterBooksCollection();
            });
            _openAddBookWinCommand = new RelayCommand(OpenAddBookWin);
            _openAddAuthorWinCommand = new RelayCommand(OpenAddAuthorWin);
            _clearAddAuthorFlyoutCommand = new RelayCommand(ClearAddAuthorFlyout);
            _clearAddBookFlyoutCommand = new RelayCommand(ClearAddBookFlyout);
            _openEditBookWinCommand = new RelayCommand<Book>(OpenEditBookWin);
            _openEditAuthorWinCommand = new RelayCommand<Author>(OpenEditAuthorWin);
            _removeBookWinCommand = new RelayCommand<Book>(RemoveBookWin);
            _removeAuthorWinCommand = new RelayCommand<Author>(RemoveAuthorWin);
            _exportBooksToXlsxCommand = new RelayCommand(ExportBooksToXlsx);
            _exportAuthorsToXlsxCommand = new RelayCommand(ExportAuthorsToXlsx);
            _prepareForExportBooksToDriveCommand = new RelayCommand(PrepareForExportBooksToDrive);
            _prepareForExportAuthorsToDriveCommand = new RelayCommand(PrepareForExportAuthorsToDrive);
            _exportFileToRootCommand = new RelayCommand(ExportFileToRoot);
            _exportFileToGoogleDriveCommand = new RelayCommand<object>(ExportFileToGoogleDrive);
            task1.ContinueWith((Task t1) =>
            {
                _dispatcher.Invoke(new Action(() =>
                {
                    BooksDataGridCollection = CollectionViewSource.GetDefaultView(Books);
                    BooksDataGridCollection.Filter = new Predicate<object>(FilterBooks);
                }));
                IsDataGridActive = true;
                IsPreloaderActive = false;
            });
            task2.ContinueWith((Task t1) =>
            {
                _dispatcher.Invoke(new Action(() =>
                {
                    AuthorsDataGridCollection = CollectionViewSource.GetDefaultView(Authors);
                    AuthorsDataGridCollection.Filter = new Predicate<object>(FilterAuthors);
                }));
            });

        }
    }
}