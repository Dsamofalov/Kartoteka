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
        int RegisterNewAuthor(Author NewAuthor);
        void EditAuthor(Author AuthorToEdit);
        void DeleteAuthor(int ID);
        Author GetAuthorByID(int ID);
 
    }
}
