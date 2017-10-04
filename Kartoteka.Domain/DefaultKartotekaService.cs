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

        public DefaultKartotekaService(IAuthorsRepository authorsRep,
            IBooksRepository booksRep,
            ILoggerService loggerService)
        {
            try
            {
                if (authorsRep == null) throw new ArgumentNullException("authorsRep", "authorsRep is null");
                if (booksRep == null) throw new ArgumentNullException("booksRep", "booksRep is null");
                if (loggerService == null) throw new ArgumentNullException("loggerService", "loggerService is null");

                _authorsRep = authorsRep;
                _booksRep = booksRep;
                _loggingService = loggerService;
            }
            catch (ArgumentNullException ex)
            {
                _loggingService.LogError($" DefaultKartotekaService ctor can't get a service {ex}");
                System.Windows.MessageBox.Show("An exception just occurred: " + ex.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
        }
        public List<Author> GetAllAuthors()
        {
            _loggingService.LogInfo("Request all authors");
            return _authorsRep.GetAllAuthors();
        }

        public List<Book> GetAllBooks()
        {
            _loggingService.LogInfo("Request all books");
            return _booksRep.GetAllBooks();
        }
        public void DeleteAuthor(int id)
        {
            _loggingService.LogInfo($"Remove the author with id {id}");
            _authorsRep.DeleteAuthor(id);     
        }

        public void DeleteBook(int id)
        {
            _loggingService.LogInfo($"Remove the book with id {id}");
            _booksRep.DeleteBook(id);
        }

        public void EditAuthor(Author authorToEdit)
        {
            _loggingService.LogInfo($"Edit the author with id {authorToEdit.Id}");
            _authorsRep.EditAuthor(authorToEdit);
        }

        public void EditBook(Book bookToEdit)
        {
            _loggingService.LogInfo($"Edit the book with id {bookToEdit.Id}");
            _booksRep.EditBook(bookToEdit);
        }


        public Author GetAuthorByID(int id)
        {
            _loggingService.LogInfo($"Request the author with id {id}");
            return _authorsRep.GetAuthorByID(id);
        }

        public Book GetBookByID(int id)
        {
            _loggingService.LogInfo($"Request the book with id {id}");
            return _booksRep.GetBookByID(id);
        }

        public int RegisterNewAuthor(Author newAuthor)
        {
            _loggingService.LogInfo($"Register new author");
            return _authorsRep.RegisterNewAuthor(newAuthor);
        }

        public int RegisterNewBook(Book newBook)
        {
            _loggingService.LogInfo($"Register new book");
            return _booksRep.RegisterNewBook(newBook);
        }

        public ExportData ExportAuthorsData()
        {
            _loggingService.LogInfo($"Export authors to Excel file");
            var exporter = GetExporter(DataExporterType.XLSX);
            var authors = _authorsRep.GetAllAuthors();
            return exporter.AuthorsExport(authors);
        }
        public ExportData ExportBooksData()
        {
            _loggingService.LogInfo($"Export books to Excel file");
            var exporter = GetExporter(DataExporterType.XLSX);
            var books = _booksRep.GetAllBooks();
            return exporter.BooksExport(books);
        }
        public void ExportBooksToDataDrive(string folder)
        {
            _loggingService.LogInfo($"Sending file to Google Drive");
            var service = GoogleDrive.Authorization();
            var exporter = GetExporter(DataExporterType.XLSX);
            var books = _booksRep.GetAllBooks();
            ExportData uploadFile = exporter.BooksExport(books);
            GoogleDrive.UploadFile(service, "Books", uploadFile,folder);
        }
        public void ExportAuthorsToDataDrive(string folder)
        {
            _loggingService.LogInfo($"Sending file to Google Drive");
            var service = GoogleDrive.Authorization();
            var exporter = GetExporter(DataExporterType.XLSX);
            var authors = _authorsRep.GetAllAuthors();
            ExportData uploadFile = exporter.AuthorsExport(authors);
            GoogleDrive.UploadFile(service, "Authors", uploadFile, folder);
        }
        public Dictionary<string, string> GetFolders()
        {
            _loggingService.LogInfo($"Getting folders from Google Drive");
            var service = GoogleDrive.Authorization();
            Dictionary<string, string> folders = new Dictionary<string, string>();
            GoogleDrive.GetFolders(service, folders);
            return folders;
        }
        public IDataExporter GetExporter(DataExporterType type)
        {
            switch (type)
            {
                case DataExporterType.XLSX:
                    return new XLSXDataExporter();
                default:
                    throw new Exception("DataExporterType not supported");
            }
        }
    }
}
