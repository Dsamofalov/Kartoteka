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
            if (authorsRep == null) throw new ArgumentNullException("authorsRep", "authorsRep is null");
            if (booksRep == null) throw new ArgumentNullException("booksRep", "booksRep is null");
            if (loggerService == null) throw new ArgumentNullException("loggerService", "loggerService is null");

            _authorsRep = authorsRep;
            _booksRep = booksRep;
            _loggingService = loggerService;
        }
        public List<Author> GetAllAuthors()
        {
            _loggingService.LogInfo("Запрос всех авторов");
            return _authorsRep.GetAllAuthors();
        }

        public List<Book> GetAllBooks()
        {
            _loggingService.LogInfo("Запрос всех книг");
            return _booksRep.GetAllBooks();
        }
        public void DeleteAuthor(int id)
        {
            _loggingService.LogInfo($"Удаление автора с id {id}");
            _authorsRep.DeleteAuthor(id);     
        }

        public void DeleteBook(int id)
        {
            _loggingService.LogInfo($"Удаление книги с id {id}");
            _booksRep.DeleteBook(id);
        }

        public void EditAuthor(Author authorToEdit)
        {
            _loggingService.LogInfo($"Изменение автора с id {authorToEdit.Id}");
            _authorsRep.EditAuthor(authorToEdit);
        }

        public void EditBook(Book bookToEdit)
        {
            _loggingService.LogInfo($"Изменение книги с id {bookToEdit.Id}");
            _booksRep.EditBook(bookToEdit);
        }


        public Author GetAuthorByID(int id)
        {
            _loggingService.LogInfo($"Запрос автора с id {id}");
            return _authorsRep.GetAuthorByID(id);
        }

        public Book GetBookByID(int id)
        {
            _loggingService.LogInfo($"Запрос книги с id {id}");
            return _booksRep.GetBookByID(id);
        }

        public int RegisterNewAuthor(Author newAuthor)
        {
            _loggingService.LogInfo($"Регистрация нового автора");
            return _authorsRep.RegisterNewAuthor(newAuthor);
        }

        public int RegisterNewBook(Book newBook)
        {
            _loggingService.LogInfo($"Регистрация новой книги");
            return _booksRep.RegisterNewBook(newBook);
        }

        public ExportData ExportAuthorsData()
        {
            _loggingService.LogInfo($"Экспорт авторов в Excel file");
            var exporter = GetExporter(DataExporterType.XLSX);
            var authors = _authorsRep.GetAllAuthors();
            return exporter.AuthorsExport(authors);
        }
        public ExportData ExportBooksData()
        {
            _loggingService.LogInfo($"Экспорт книг в Excel file");
            var exporter = GetExporter(DataExporterType.XLSX);
            var books = _booksRep.GetAllBooks();
            return exporter.BooksExport(books);
        }

        public IDataExporter GetExporter(DataExporterType type)
        {
            switch(type)
            {
                case DataExporterType.XLSX:
                    return new XLSXDataExporter();
                default:
                    return null;
            }
        }
    }
}
