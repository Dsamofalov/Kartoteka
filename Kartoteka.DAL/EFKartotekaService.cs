using AutoMapper;
using Kartoteka.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kartoteka.DAL
{
    public class EFKartotekaService : IKartotekaService
    {
        public void DeleteAuthor(int ID)
        {
            using (KartotekaModel db = new KartotekaModel())
            {
                AuthorModel author = db.authors.Find(ID);
                if (author != null)
                    db.authors.Remove(author);
                db.SaveChanges();
            }
        }

        public void EditAuthor(Author AuthorToEdit)
        {
            using (KartotekaModel db = new KartotekaModel())
            {
                AuthorModel author = db.authors.Find(AuthorToEdit.Id);
                author.FirstName = AuthorToEdit.FirstName;
                author.SecondName = AuthorToEdit.SecondName;
                author.LastName = AuthorToEdit.LastName;
                author.books.Clear();
                if (AuthorToEdit.books != null)
                {
                    foreach (Book book in AuthorToEdit.books)
                    {
                        author.books.Add(db.books.Find(book.Id));
                    }
                }
                db.SaveChanges();
            }
        }

        public List<Author> GetAllAuthors()
        {
            using (KartotekaModel db = new KartotekaModel())
            {
                List<Author> AllAuthors = new List<Author>();
                foreach (AuthorModel aut in db.authors)
                {
                    AllAuthors.Add(GetAuthorByID(aut.Id));
                }
                return AllAuthors;
            }
        }

        public Author GetAuthorByID(int ID)
        {
            using (KartotekaModel db = new KartotekaModel())
            {
                AuthorModel author = db.authors.Find(ID);
                Author AuthorToReturn = Mapper.Map<AuthorModel, Author>(author);
                return AuthorToReturn;
            }
        }

        public int RegisterNewAuthor(Author NewAuthor)
        {
            using (KartotekaModel db = new KartotekaModel())
            {
                AuthorModel authormodel = Mapper.Map<Author, AuthorModel>(NewAuthor);
                db.authors.Add(authormodel);
                db.SaveChanges();
                return authormodel.Id;
            }
        }
        public void DeleteBook(int ID)
        {
            using (KartotekaModel db = new KartotekaModel())
            {
                BookModel book = db.books.Find(ID);
                if (book != null)
                    db.books.Remove(book);
                db.SaveChanges();
            }
        }
        public void EditBook(Book BookToEdit)
        {
            using (KartotekaModel db = new KartotekaModel())
            {
                BookModel book = db.books.Find(BookToEdit.Id);
                book.Description = BookToEdit.Description;
                book.Name = BookToEdit.Name;
                book.Year = BookToEdit.Year;
                book.authors.Clear();
                if (BookToEdit.authors != null)
                {
                    foreach (Author author in BookToEdit.authors)
                    {
                        book.authors.Add(db.authors.Find(author.Id));
                    }
                }
                db.SaveChanges();
            }
        }

        public List<Book> GetAllBooks()
        {
            using (KartotekaModel db = new KartotekaModel())
            {
                List<Book> AllBooks = new List<Book>();
                foreach (BookModel bk in db.books)
                {
                    AllBooks.Add(GetBookByID(bk.Id));
                }
                return AllBooks;
            }
        }

        public Book GetBookByID(int ID)
        {
            using (KartotekaModel db = new KartotekaModel())
            {
                BookModel book = db.books.Find(ID);
                Book BookToReturn = Mapper.Map<BookModel, Book>(book);
                return BookToReturn;
            }
        }

        public int RegisterNewBook(Book NewBook)
        {
            using (KartotekaModel db = new KartotekaModel())
            {
                BookModel bookmodel = Mapper.Map<Book, BookModel>(NewBook);
                db.books.Add(bookmodel);
                db.SaveChanges();
                return bookmodel.Id;
            }
        }
    }
}
