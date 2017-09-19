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
        int RegisterNewAuthor(Author newAuthor);
        void EditAuthor(Author authorToEdit);
        void DeleteAuthor(int id);
        Author GetAuthorByID(int id);
 
    }
}
