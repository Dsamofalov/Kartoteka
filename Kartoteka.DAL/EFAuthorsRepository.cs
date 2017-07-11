using AutoMapper;
using Kartoteka.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kartoteka.DAL
{
    public class EFAuthorsRepository : IAuthorsRepository
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
    }
}
