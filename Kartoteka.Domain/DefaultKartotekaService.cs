using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kartoteka.Domain
{
    class DefaultKartotekaService:IKartotekaService
    {
        private readonly IAuthorsRepository _authorsRep;
        private readonly IBooksRepository _booksRep;
        public DefaultKartotekaService(IAuthorsRepository authorsRep, IBooksRepository booksRep)
        {
            if (authorsRep == null) throw new ArgumentNullException("authorsRep", "authorsRep is null");
            else _authorsRep = authorsRep;
            if (booksRep == null) throw new ArgumentNullException("booksRep", "booksRep is null");
            else _booksRep = booksRep;
        }

        public void DeleteAuthor(int ID)
        {
            throw new NotImplementedException();
        }

        public void DeleteBook(int ID)
        {
            throw new NotImplementedException();
        }

        public void EditAuthor(Author AuthorToEdit)
        {
            throw new NotImplementedException();
        }

        public void EditBook(Book BookToEdit)
        {
            throw new NotImplementedException();
        }

        public List<Author> GetAllAuthors()
        {
            throw new NotImplementedException();
        }

        public List<Book> GetAllBooks()
        {
            throw new NotImplementedException();
        }

        public Author GetAuthorByID(int ID)
        {
            throw new NotImplementedException();
        }

        public Book GetBookByID(int ID)
        {
            throw new NotImplementedException();
        }

        public int RegisterNewAuthor(Author NewAuthor)
        {
            throw new NotImplementedException();
        }

        public int RegisterNewBook(Book NewBook)
        {
            throw new NotImplementedException();
        }
    }
}
