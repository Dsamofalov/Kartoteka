using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kartoteka.Domain
{
    public interface IAuthorsRepository
    {
        List<Author> GetAllAuthors();
        void RegisterNewAuthor(Author NewAuthor);
        void EditAuthor(int ID);
        void DeleteAuthor(int ID);
        Author GetAuthorByID(int ID);
    }
}
