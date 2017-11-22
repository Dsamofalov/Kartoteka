using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kartoteka.Domain
{
    public interface IBooksRepository
    /// интерфейс, связывающий реализацию на уровне DAL ( методы взаимодействия класса Book с базой данных)
    /// и уровень бизнес-логики (Domain) через внедрение зависимости этого интерфейса от конкретной реализации
    {
        List<Book> GetAllBooks();
        int RegisterNewBook(Book newBook);
        void EditBook(Book bookToEdit);
        void DeleteBook(int id);
        Book GetBookByID(int id);
    }
}
