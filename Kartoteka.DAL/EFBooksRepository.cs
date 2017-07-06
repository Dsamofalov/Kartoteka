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
        private KartotekaModel db;
        public EFBooksRepository()
        {
            this.db = new KartotekaModel();
        }
        private Book BookModelToBook(BookModel bookmodel)
        {
            Book book = new Book()
            { Id = bookmodel.Id, Description = bookmodel.Description, Name=bookmodel.Name, Year = bookmodel.Year };
            if (bookmodel.authors != null)
            {
                foreach (AuthorModel author in bookmodel.authors)
                {
                    book.authors.Add(new Author()
                    { Id = author.Id, FirstName = author.FirstName, SecondName = author.SecondName, LastName = author.LastName });
                }
            }
            return book;
        }
        public void DeleteBook(int ID)
        {
            BookModel book = db.books.Find(ID);
            if (book != null)
            db.books.Remove(book);
        }

        public void EditBook(Book BookToEdit)
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
                db.Entry(book).State = EntityState.Modified;
        }

        public List<Book> GetAllBooks()
        {
                List<Book> AllBooks = new List<Book>();
                foreach (BookModel bk in db.books)
                {
                    AllBooks.Add(GetBookByID(bk.Id));
                }
                return AllBooks;
        }

        public Book GetBookByID(int ID)
        {
            BookModel book = db.books.Find(ID);
            Book BookToReturn = BookModelToBook(book);
            return BookToReturn;
        }

        public int RegisterNewBook(Book NewBook)
        {
            BookModel bookmodel = new BookModel()
            { Name = NewBook.Name, Description = NewBook.Description, Year = NewBook.Year };
            if(NewBook.authors!=null)
            { 
                foreach (Author author in NewBook.authors)
                {
                    bookmodel.authors.Add(db.authors.Find(author.Id));
                }
            }
            db.books.Add(bookmodel);
            db.SaveChanges();
            return bookmodel.Id;
        }
        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
