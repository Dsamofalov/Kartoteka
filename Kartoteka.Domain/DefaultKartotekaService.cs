using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kartoteka.Domain
{
    public class DefaultKartotekaService:IKartotekaService
    {
        private readonly IAuthorsRepository _authorsRep;
        private readonly IBooksRepository _booksRep;
        private readonly ILoggerService _loggingService;
        private readonly IGoogleDriveService _googleService;
        private XLSXDataExporter _exporter;

        public DefaultKartotekaService(IAuthorsRepository authorsRep,
            IBooksRepository booksRep,
            ILoggerService loggerService, IGoogleDriveService googleService)
        {
            try
            {
                if (authorsRep == null) throw new ArgumentNullException("authorsRep", "authorsRep is null");
                if (booksRep == null) throw new ArgumentNullException("booksRep", "booksRep is null");
                if (loggerService == null) throw new ArgumentNullException("loggerService", "loggerService is null");
                if (googleService == null) throw new ArgumentNullException("googleService", "googleService is null");

                _authorsRep = authorsRep;
                _booksRep = booksRep;
                _loggingService = loggerService;
                _googleService = googleService;
                _exporter = new XLSXDataExporter(); 
            }
            catch (ArgumentNullException ex)
            {
                _loggingService.LogError($" DefaultKartotekaService ctor can't get a service {ex}");
                MessageBox.Show("An exception just occurred: " + ex.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
        }
        public List<Author> GetAllAuthors()
        {
            try
            {
                _loggingService.LogInfo("Request all authors");
                return _authorsRep.GetAllAuthors();
            }
            catch (Exception e)
            {
                MessageBox.Show("An exception just occurred: " + e.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);
                _loggingService.LogError($"Exception in GetAllAuthors() { e.Message}");
                return null;
            }
        }

        public List<Book> GetAllBooks()
        {
            try
            {
                _loggingService.LogInfo("Request all books");
                return _booksRep.GetAllBooks();
            }
            catch (Exception e)
            {
                MessageBox.Show("An exception just occurred: " + e.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);
                _loggingService.LogError($"Exception in GetAllBooks() { e.Message}");
                return null;
            }
        }
        public void DeleteAuthor(int id)
        {
            try
            {
                _loggingService.LogInfo($"Remove the author with id {id}");
                _authorsRep.DeleteAuthor(id);
            }
            catch (Exception e)
            {
                MessageBox.Show("An exception just occurred: " + e.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);
                _loggingService.LogError($"Exception in DeleteAuthor(id {id}) { e.Message}");
            }     
        }

        public void DeleteBook(int id)
        {
            try
            {
                _loggingService.LogInfo($"Remove the book with id {id}");
                _booksRep.DeleteBook(id);
            }
            catch (Exception e)
            {
                MessageBox.Show("An exception just occurred: " + e.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);
                _loggingService.LogError($"Exception in DeleteBook(id {id}) { e.Message}");
            }
        }

        public void EditAuthor(Author authorToEdit)
        {
            try
            {
                _loggingService.LogInfo($"Edit the author with id {authorToEdit.Id}");
                _authorsRep.EditAuthor(authorToEdit);
            }
            catch (Exception e)
            {
                MessageBox.Show("An exception just occurred: " + e.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);
                _loggingService.LogError($"Exception in EditAuthor(id {authorToEdit.Id}) { e.Message}");
            }

        }

        public void EditBook(Book bookToEdit)
        {
            try
            {
                _loggingService.LogInfo($"Edit the book with id {bookToEdit.Id}");
                _booksRep.EditBook(bookToEdit);
            }
            catch (Exception e)
            {
                MessageBox.Show("An exception just occurred: " + e.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);
                _loggingService.LogError($"Exception in EditAuthor(id {bookToEdit.Id}) { e.Message}");
            }
        }


        public Author GetAuthorByID(int id)
        {
            try
            {
                _loggingService.LogInfo($"Request the author with id {id}");
                return _authorsRep.GetAuthorByID(id);
            }
            catch (Exception e)
            {
                MessageBox.Show("An exception just occurred: " + e.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);
                _loggingService.LogError($"Exception in GetAuthorByID(id {id}) { e.Message}");
                return null;
            }
        }

        public Book GetBookByID(int id)
        {
            try
            {
                _loggingService.LogInfo($"Request the book with id {id}");
                return _booksRep.GetBookByID(id);
            }
            catch (Exception e)
            {
                MessageBox.Show("An exception just occurred: " + e.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);
                _loggingService.LogError($"Exception in GetBookByID(id {id}) { e.Message}");
                return null;
            }
        }

        public int RegisterNewAuthor(Author newAuthor)
        {
            try
            {
                _loggingService.LogInfo($"Register new author");
                return _authorsRep.RegisterNewAuthor(newAuthor);
            }
            catch (Exception e)
            {
                MessageBox.Show("An exception just occurred: " + e.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);
                _loggingService.LogError($"Exception in RegisterNewAuthor { e.Message}");
                return -1;
            }
        }

        public int RegisterNewBook(Book newBook)
        {
            try
            {
                _loggingService.LogInfo($"Register new book");
                return _booksRep.RegisterNewBook(newBook);
            }
            catch (Exception e)
            {
                MessageBox.Show("An exception just occurred: " + e.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);
                _loggingService.LogError($"Exception in RegisterNewBook { e.Message}");
                return -1;
            }
        }

        public ExportData ExportAuthorsData()
        {
            _loggingService.LogInfo($"Export authors to Excel file");
            var authors = _authorsRep.GetAllAuthors();
            return _exporter.AuthorsExport(authors);
        }
        public ExportData ExportBooksData()
        {
            _loggingService.LogInfo($"Export books to Excel file");
            var books = _booksRep.GetAllBooks();
            return _exporter.BooksExport(books);
        }
        public void ExportBooksToDataDrive(string folder)
        {
            _loggingService.LogInfo($"Sending file to Google Drive");
            var service = _googleService.Authorization();
            var books = _booksRep.GetAllBooks();
            ExportData uploadFile = _exporter.BooksExport(books);
            uploadFile.FileName = "Books";
            uploadFile.Folder = folder;
            _googleService.UploadFile(service, uploadFile);
        }
        public void ExportAuthorsToDataDrive(string folder)
        {
            _loggingService.LogInfo($"Sending file to Google Drive");
            var service = _googleService.Authorization();
            var authors = _authorsRep.GetAllAuthors();
            ExportData uploadFile = _exporter.AuthorsExport(authors);
            uploadFile.FileName = "Authors";
            uploadFile.Folder = folder;
            _googleService.UploadFile(service, uploadFile);
        }
        public Dictionary<string, string> GetFolders()
        {
            _loggingService.LogInfo($"Getting folders from Google Drive");
            var service = _googleService.Authorization();
            Dictionary<string, string> folders = new Dictionary<string, string>();
            _googleService.GetFolders(service, folders);
            return folders;
        }
    }
}
