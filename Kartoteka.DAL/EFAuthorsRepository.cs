using AutoMapper;
using Kartoteka.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kartoteka.DAL
{
    public class EFAuthorsRepository : IAuthorsRepository
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
    }
}
