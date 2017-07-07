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
        private KartotekaModel db;
        public EFBooksRepository()
        {
            db = new KartotekaModel();
            Mapper.Initialize(cfg => {
                cfg.CreateMap<Book, BookModel>().MaxDepth(3);
                cfg.CreateMap<BookModel, Book>().MaxDepth(3);
                cfg.CreateMap<Author, AuthorModel>().MaxDepth(3);
                cfg.CreateMap<AuthorModel, Author>().MaxDepth(3);
            });
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
            Book BookToReturn = Mapper.Map<BookModel, Book>(book);
            return BookToReturn;
        }

        public int RegisterNewBook(Book NewBook)
        {
            BookModel bookmodel = Mapper.Map<Book, BookModel>(NewBook);
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
