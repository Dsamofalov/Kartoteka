
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
        void RegisterNewAuthor(Author NewAuthor);
        void RegisterNewBook(Book NewBook);
        void EditAuthor(int ID);
        void EditBook(int ID);
        void DeleteAuthor(int ID);
        void DeleteBook(int ID);
        Author GetAuthorByID(int ID);
        Book GetBookByID(int ID);
    }
}
