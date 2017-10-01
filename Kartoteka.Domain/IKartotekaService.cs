using DocumentFormat.OpenXml.Packaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kartoteka.Domain
{
    public interface IKartotekaService
    {
        List<Author> GetAllAuthors();
        List<Book> GetAllBooks();
        int RegisterNewAuthor(Author newAuthor);
        int RegisterNewBook(Book newBook);
        void EditAuthor(Author authorToEdit);
        void EditBook(Book bookToEdit);
        void DeleteAuthor(int id);
        void DeleteBook(int id);
        Author GetAuthorByID(int id);
        Book GetBookByID(int id);
        ExportData ExportAuthorsData();
        ExportData ExportBooksData();
        void ExportToDataDrive(string filePath);
        IDataExporter GetExporter(DataExporterType type);
    }
}
