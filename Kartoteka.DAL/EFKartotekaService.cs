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
        public void DeleteAuthor(int id)
        {
            using (KartotekaModel db = new KartotekaModel())
            {
                AuthorModel author = db.authors.Find(id);
                if (author != null)
                    db.authors.Remove(author);
                db.SaveChanges();
            }
        }

        public void EditAuthor(Author authorToEdit)
        {
            using (KartotekaModel db = new KartotekaModel())
            {
                AuthorModel author = db.authors.Find(authorToEdit.Id);
                author.FirstName = authorToEdit.FirstName;
                author.SecondName = authorToEdit.SecondName;
                author.LastName = authorToEdit.LastName;
                author.books.Clear();
                if (authorToEdit.books != null)
                {
                    foreach (Book book in authorToEdit.books)
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
                List<Author> allAuthors = new List<Author>();
                foreach (AuthorModel aut in db.authors)
                {
                    Author authorToReturn = Mapper.Map<AuthorModel, Author>(aut);
                    allAuthors.Add(authorToReturn);
                }
                return allAuthors;
            }
        }

        public Author GetAuthorByID(int id)
        {
            using (KartotekaModel db = new KartotekaModel())
            {
                AuthorModel author = db.authors.Find(id);
                Author authorToReturn = Mapper.Map<AuthorModel, Author>(author);
                return authorToReturn;
            }
        }

        public int RegisterNewAuthor(Author newAuthor)
        {
            using (KartotekaModel db = new KartotekaModel())
            {
                AuthorModel authorModel = Mapper.Map<Author, AuthorModel>(newAuthor);
                db.authors.Add(authorModel);
                db.SaveChanges();
                return authorModel.Id;
            }
        }
        public void DeleteBook(int id)
        {
            using (KartotekaModel db = new KartotekaModel())
            {
                BookModel book = db.books.Find(id);
                if (book != null)
                    db.books.Remove(book);
                db.SaveChanges();
            }
        }
        public void EditBook(Book bookToEdit)
        {
            using (KartotekaModel db = new KartotekaModel())
            {
                BookModel book = db.books.Find(bookToEdit.Id);
                book.Description = bookToEdit.Description;
                book.Name = bookToEdit.Name;
                book.Year = bookToEdit.Year;
                book.authors.Clear();
                if (bookToEdit.authors != null)
                {
                    foreach (Author author in bookToEdit.authors)
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
                List<Book> allBooks = new List<Book>();
                foreach (BookModel bk in db.books)
                {
                    Book bookToReturn = Mapper.Map<BookModel, Book>(bk);
                    allBooks.Add(bookToReturn);
                }
                return allBooks;
            }
        }

        public Book GetBookByID(int id)
        {
            using (KartotekaModel db = new KartotekaModel())
            {
                BookModel book = db.books.Find(id);
                Book bookToReturn = Mapper.Map<BookModel, Book>(book);
                return bookToReturn;
            }
        }

        public int RegisterNewBook(Book newBook)
        {
            using (KartotekaModel db = new KartotekaModel())
            {
                BookModel bookModel = Mapper.Map<Book, BookModel>(newBook);
                db.books.Add(bookModel);
                db.SaveChanges();
                return bookModel.Id;
            }
        }
    }
}
