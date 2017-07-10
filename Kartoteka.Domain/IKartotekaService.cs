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
        int RegisterNewAuthor(Author NewAuthor);
        int RegisterNewBook(Book NewBook);
        void EditAuthor(Author AuthorToEdit);
        void EditBook(Book BookToEdit);
        void DeleteAuthor(int ID);
        void DeleteBook(int ID);
        Author GetAuthorByID(int ID);
        Book GetBookByID(int ID);
    }
}
