using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kartoteka.Domain
{
    public interface IBooksRepository : IDisposable
    {
        List<Book> GetAllBooks();
        int RegisterNewBook(Book NewBook);
        void EditBook(Book BookToEdit);
        void DeleteBook(int ID);
        Book GetBookByID(int ID);
        void Save();
    }
}
