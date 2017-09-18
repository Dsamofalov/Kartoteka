using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public void DeleteAuthor(int ID)
        {
            _authorsRep.DeleteAuthor(ID);     
        }

        public void DeleteBook(int ID)
        {
            throw new NotImplementedException();
        }

        public void EditAuthor(Author AuthorToEdit)
        {
            throw new NotImplementedException();
        }

        public void EditBook(Book BookToEdit)
        {
            throw new NotImplementedException();
        }


        public Author GetAuthorByID(int ID)
        {
            throw new NotImplementedException();
        }

        public Book GetBookByID(int ID)
        {
            throw new NotImplementedException();
        }

        public int RegisterNewAuthor(Author NewAuthor)
        {
            throw new NotImplementedException();
        }

        public int RegisterNewBook(Book NewBook)
        {
            throw new NotImplementedException();
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
