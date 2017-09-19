using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kartoteka.Domain
{
    public interface IBooksRepository
    {
        List<Book> GetAllBooks();
        int RegisterNewBook(Book newBook);
        void EditBook(Book bookToEdit);
        void DeleteBook(int id);
        Book GetBookByID(int id);
    }
}
