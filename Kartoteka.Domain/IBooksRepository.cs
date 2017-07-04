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
        void RegisterNewBook(Book NewBook);
        void EditBook(int ID);
        void DeleteBook(int ID);
        Book GetBookByID(int ID);
    }
}
