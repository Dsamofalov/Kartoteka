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
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Ioc;
using MahApps.Metro.Controls.Dialogs;
using NLog;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Packaging;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.Collections;
using System.Linq;

namespace NewKartoteka.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IKartotekaService _service;
        private ObservableCollection<Book> _books;
        private ObservableCollection<Author> _authors;
        private bool _isNewBookOpen =false;
        private bool _isNewAuthorOpen = false;
        private bool _isEditBookOpen = false;
        private bool _isEditAuthorOpen = false;
        private bool _isPreloaderActive = true;
        private bool _isDataGridActive = false;
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
        private List <string> _folders;
        private IDialogCoordinator dialogCoordinator;
        private readonly ILoggerService _loggingService;
        private ChooseGoogleDriveDirWin ChooseDir;
        private string TypeOfExportData;
        private Dictionary<string, string> FoldersAndId;

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
        public ObservableCollection<Author> Authors
        {
            get
            {
                return _authors;
            }

            set
            {
                _authors = value;
                RaisePropertyChanged("Authors");
            }
        }

        public bool IsNewBookOpen
        {
            get
            {
                return _isNewBookOpen;
            }

            set
            {
                _isNewBookOpen = value;
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
        public bool IsNewAuthorOpen
        {
            get
            {
                return _isNewAuthorOpen;
            }

            set
            {
                _isNewAuthorOpen = value;
                RaisePropertyChanged("IsNewAuthorOpen");
            }
        }
        public bool IsEditAuthorOpen
        {
            get
            {
                return _isEditAuthorOpen;
            }

            set
            {
                _isEditAuthorOpen = value;
                RaisePropertyChanged("IsEditAuthorOpen");
            }
        }
        public bool IsPreloaderActive
        {
            get
            {
                return _isPreloaderActive;
            }

            set
            {
                _isPreloaderActive = value;
                RaisePropertyChanged("IsPreloaderActive");
            }
        }
        public bool IsDataGridActive
        {
            get
            {
                return _isDataGridActive;
            }

            set
            {
                _isDataGridActive = value;
                RaisePropertyChanged("IsDataGridActive");
            }
        }
        public List<string> Folders
        { get {  return _folders;} set {  _folders = value;} }
        public ICommand OpenAddBookWinCommand
        {
            get
            {
                if (_openAddBookWinCommand == null) _openAddBookWinCommand = new RelayCommand(() =>
                 {
                     IsNewBookOpen = true;
                     ViewModelLocator._addBookMessenger.Send<NotificationMessage>(new NotificationMessage(""));
                 });

                return _openAddBookWinCommand;
            }
        }
        public ICommand OpenAddAuthorWinCommand
        {
            get
            {
                if (_openAddAuthorWinCommand == null) _openAddAuthorWinCommand = new RelayCommand(() =>
                {
                    IsNewAuthorOpen = true;
                    ViewModelLocator._addAuthorMessenger.Send<NotificationMessage>(new NotificationMessage(""));
                });

                return _openAddAuthorWinCommand;
            }
        }
        public ICommand ClearAddAuthorFlyoutCommand
        {
            get
            {
                if (_clearAddAuthorFlyoutCommand == null) _clearAddAuthorFlyoutCommand = new RelayCommand(() =>
                {
                    Messenger.Default.Send(new NotificationMessage("ClearAddAuthorFlyout"));
                    IsNewAuthorOpen = false;
                });

                return _clearAddAuthorFlyoutCommand;
            }
        }
        public ICommand ClearAddBookFlyoutCommand
        {
            get
            {
                if (_clearAddBookFlyoutCommand == null) _clearAddBookFlyoutCommand = new RelayCommand(() =>
                {
                    Messenger.Default.Send(new NotificationMessage("ClearAddBookFlyout"));
                    IsNewBookOpen = false;
                });

                return _clearAddBookFlyoutCommand;
            }
        }
        public ICommand OpenEditBookWinCommand
        {
            get
            {
                if (_openEditBookWinCommand == null) _openEditBookWinCommand = new RelayCommand<Book>((Book bookToEdit) =>
                {
                    IsEditBookOpen = !IsEditBookOpen;
                    ViewModelLocator._editBookMessenger.Send<NotificationMessage>(new NotificationMessage(bookToEdit.Id.ToString()));
                });
                return _openEditBookWinCommand;
            }
        }
        public ICommand OpenEditAuthorWinCommand
        {
            get
            {
                if (_openEditAuthorWinCommand == null) _openEditAuthorWinCommand = new RelayCommand<Author>((Author authorToEdit) =>
                {
                    IsEditAuthorOpen = !IsEditAuthorOpen;
                    ViewModelLocator._editAuthorMessenger.Send<NotificationMessage>(new NotificationMessage(authorToEdit.Id.ToString()));
                });
                return _openEditAuthorWinCommand;
            }
        }
        public ICommand RemoveBookWinCommand
        {
            get
            {
                if (_removeBookWinCommand == null) _removeBookWinCommand = new RelayCommand<Book>(async (Book bookToRemove) =>
                {
                    _service.DeleteBook(bookToRemove.Id);
                    Books.Remove(bookToRemove);
                    FilterBooksCollection();

                    await dialogCoordinator.ShowMessageAsync(this, "Книга удалена", String.Concat("ID удаленной книги: ", bookToRemove.Id.ToString()));

                });
                return _removeBookWinCommand;
            }
        }
        public ICommand RemoveAuthorWinCommand
        {
            get
            {
                if (_removeAuthorWinCommand == null) _removeAuthorWinCommand = new RelayCommand<Author>(async (Author authorToRemove) =>
                {
                    _service.DeleteAuthor(authorToRemove.Id);
                    Authors.Remove(authorToRemove);
                    FilterAuthorsCollection();
                    await dialogCoordinator.ShowMessageAsync(this, "Автор удален", String.Concat("ID удаленного автора: ", authorToRemove.Id.ToString()));
                });
                return _removeAuthorWinCommand;
            }
        }
        public ICommand ExportBooksToXlsxCommand
        {
            get
            {
                if (_exportBooksToXlsxCommand == null) _exportBooksToXlsxCommand = new RelayCommand(async () =>
                {
                    string filePath = GetPathToExcel();
                    if (filePath != null)
                    {
                        MessageDialogResult result = await dialogCoordinator.ShowMessageAsync(this, "Подтверждение", "Вы действительно хотите перезаписать данный файл? ",
                         MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AnimateShow = false, ColorScheme = MetroDialogColorScheme.Theme });
                        if (result == MessageDialogResult.Affirmative)
                        {
                            var exportData = new ExportData();
                            exportData = _service.ExportBooksData();
                            exportData.FileName = filePath;
                            File.WriteAllBytes(exportData.FileName, exportData.Data);
                            await dialogCoordinator.ShowMessageAsync(this, "Успешно сохранено", String.Concat("Книги сохранены в файл: ", filePath));
                            _loggingService.LogInfo($" Books saved to {filePath}");
                        }
                    }
                });

                return _exportBooksToXlsxCommand;
            }
        }
        public ICommand ExportAuthorsToXlsxCommand
        {
            get
            {
                if (_exportAuthorsToXlsxCommand == null) _exportAuthorsToXlsxCommand = new RelayCommand(async () =>
                {
                    string filePath = GetPathToExcel();
                    if (filePath!=null)
                    {
                        MessageDialogResult result = await dialogCoordinator.ShowMessageAsync(this, "Подтверждение", "Вы действительно хотите перезаписать данный файл? ",
                        MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() {  AnimateShow = false, ColorScheme = MetroDialogColorScheme.Theme });
                        if (result == MessageDialogResult.Affirmative)
                        {
                            var exportData = new ExportData();
                            exportData = _service.ExportAuthorsData();
                            exportData.FileName = filePath;
                            File.WriteAllBytes(exportData.FileName, exportData.Data);
                            await dialogCoordinator.ShowMessageAsync(this, "Успешно сохранено", String.Concat("Авторы сохранены в файл: ", filePath));
                            _loggingService.LogInfo($" Authors saved to {filePath}");
                        }
                    }
                });

                return _exportAuthorsToXlsxCommand;
            }
        }
        public ICommand PrepareForExportBooksToDriveCommand
        {
            get
            {
                if (_prepareForExportBooksToDriveCommand == null) _prepareForExportBooksToDriveCommand = new RelayCommand( () =>
                {
                    FoldersAndId = new Dictionary<string, string>(_service.GetFolders());
                    TypeOfExportData = "Books";
                    Folders = new List<string>(FoldersAndId.Values);
                    ChooseDir = new ChooseGoogleDriveDirWin();
                    ChooseDir.ShowDialog(); 
                });
                return _prepareForExportBooksToDriveCommand;
            }
        }
        public ICommand PrepareForExportAuthorsToDriveCommand
        {
            get
            {
                if (_prepareForExportAuthorsToDriveCommand == null) _prepareForExportAuthorsToDriveCommand = new RelayCommand(() =>
                {
                    FoldersAndId = new Dictionary<string, string>(_service.GetFolders());
                    TypeOfExportData = "Authors";
                    Folders = new List<string>(FoldersAndId.Values);
                    ChooseDir = new ChooseGoogleDriveDirWin();
                    ChooseDir.ShowDialog();
                });
                return _prepareForExportAuthorsToDriveCommand;
            }
        }
        public ICommand ExportFileToRootCommand
        {
            get
            {
                if (_exportFileToRootCommand == null) _exportFileToRootCommand = new RelayCommand(async () =>
                {
                    ChooseDir.Close();
                    MessageDialogResult result = await dialogCoordinator.ShowMessageAsync(this, "Подтверждение", "Вы действительно хотите сохранить данный файл в корневую папку? ",
                        MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings() { AnimateShow = false, ColorScheme = MetroDialogColorScheme.Theme });
                    if (result == MessageDialogResult.Affirmative)
                    {
                            if (TypeOfExportData == "Books")
                            {
                                _service.ExportBooksToDataDrive("root");
                            }
                            else
                            {
                                _service.ExportAuthorsToDataDrive("root");
                            }
                            await dialogCoordinator.ShowMessageAsync(this, "Успешно сохранено","Данные успешно сохранены в корневую папку");
                            _loggingService.LogInfo($" File saved to root");
                    }
                });

                return _exportFileToRootCommand;
            }
        }
        public ICommand ExportFileToGoogleDriveCommand
        {
            get
            {
                if (_exportFileToGoogleDriveCommand == null) _exportFileToGoogleDriveCommand = new RelayCommand<object>(async (object folder) =>
                {
                    IList selection = (IList)folder;
                    List<string> folders = selection.Cast<string>().ToList();
                    ChooseDir.Close();
                    string keyId=null;
                    foreach(KeyValuePair<string,string> keyPair in FoldersAndId)
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
                            if (TypeOfExportData == "Books")
                            {
                                _service.ExportBooksToDataDrive(keyId);
                            }
                            else
                            {
                                _service.ExportAuthorsToDataDrive(keyId);
                            }
                            await dialogCoordinator.ShowMessageAsync(this, "Успешно сохранено", String.Concat("Данные успешно сохранены в папку: ", folders.First()));
                            _loggingService.LogInfo($" File saved to {folders.First()}");
                        }
                    }
                });

                return _exportFileToGoogleDriveCommand;
            }
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
                if (!string.IsNullOrEmpty(_filterBooksString))
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
                if (!string.IsNullOrEmpty(_filterAuthorsString))
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
            openFileDialog.Filter = "Файлы Excel (*.xls; *.xlsx) | *.xls; *.xlsx";
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

            Task  task1 = new Task(() => 
            {
                Books = new ObservableCollection<Book>(_service.GetAllBooks());
                BooksDataGridCollection = CollectionViewSource.GetDefaultView(Books);
                BooksDataGridCollection.Filter = new Predicate<object>(FilterBooks);
                IsPreloaderActive = false;
                IsDataGridActive = true;
            } );
            Task task2 = new Task(() =>
            {
                Authors = new ObservableCollection<Author>(_service.GetAllAuthors());
                AuthorsDataGridCollection = CollectionViewSource.GetDefaultView(Authors);
                AuthorsDataGridCollection.Filter = new Predicate<object>(FilterAuthors);
            });
            task1.Start();
            task2.Start();
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
        }

    }
}