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
            return _booksRep.GetAllBooks();
        }
        public void DeleteAuthor(int id)
        {
            _authorsRep.DeleteAuthor(id);     
        }

        public void DeleteBook(int id)
        {
            _booksRep.DeleteBook(id);
        }

        public void EditAuthor(Author authorToEdit)
        {
            _authorsRep.EditAuthor(authorToEdit);
        }

        public void EditBook(Book bookToEdit)
        {
            _booksRep.EditBook(bookToEdit);
        }


        public Author GetAuthorByID(int id)
        {
            return _authorsRep.GetAuthorByID(id);
        }

        public Book GetBookByID(int id)
        {
            return _booksRep.GetBookByID(id);
        }

        public int RegisterNewAuthor(Author newAuthor)
        {
            return _authorsRep.RegisterNewAuthor(newAuthor);
        }

        public int RegisterNewBook(Book newBook)
        {
            return _booksRep.RegisterNewBook(newBook);
        }

        public void ExportData()
        {
            var exporter = GetExporter(DataExporterType.CSV);
            var authors = _authorsRep.GetAllAuthors();
            exporter.AuthorsExport(authors);
        }

        public IDataExporter GetExporter(DataExporterType type)
        {
            switch(type)
            {
                case DataExporterType.CSV:
                    return new CSVDataExporter();
                default:
                    return null;
            }
        }
    }
}
