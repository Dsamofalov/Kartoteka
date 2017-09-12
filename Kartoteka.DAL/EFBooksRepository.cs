using AutoMapper;
using Kartoteka.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kartoteka.DAL
{
    public class EFBooksRepository : IBooksRepository
    {

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
                ICollection<AuthorModel> authors = new List<AuthorModel>();
                foreach (AuthorModel authormodel in bookModel.authors)
                {
                    authors.Add(db.authors.Find(authormodel.Id));
                }
                bookModel.authors = authors;
                db.books.Add(bookModel);
                db.SaveChanges();
                return bookModel.Id;
            }
        }
    }
}
