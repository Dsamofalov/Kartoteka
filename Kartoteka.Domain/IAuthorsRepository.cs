using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kartoteka.Domain
{
    public interface IAuthorsRepository
    /// интерфейс, связывающий реализацию на уровне DAL ( методы взаимодействия класса Author с базой данных) 
    /// и уровень бизнес-логики (Domain) через внедрение зависимости этого интерфейса от конкретной реализации
    {
        List<Author> GetAllAuthors();
        int RegisterNewAuthor(Author newAuthor);
        void EditAuthor(Author authorToEdit);
        void DeleteAuthor(int id);
        Author GetAuthorByID(int id);
 
    }
}
