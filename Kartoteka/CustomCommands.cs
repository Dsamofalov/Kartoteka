using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kartoteka
{
   public static class CustomCommands
    {
        public static List<BookModel> GetBooksFromList(object parameter)
        {
            IList selection = (IList)parameter;
            List<BookModel> newbooks = selection.Cast<BookModel>().ToList();
            return newbooks;
        }
        public static List<AuthorModel> GetAuthorsFromList(object parameter)
        {
            IList selection = (IList)parameter;
            List<AuthorModel> newauthors = selection.Cast<AuthorModel>().ToList();
            return newauthors;
        }
        public static void AddAuthors(BookModel SelectedBook, List<AuthorModel> newauthors, DataBaseModel DbContext)
        {
            foreach (AuthorModel newauthor in newauthors)
            {
                SelectedBook.authors.Add(DbContext.authors.Find(newauthor.Id));
            }
        }
        public static void RemoveAuthors(BookModel SelectedBook, List<AuthorModel> newauthors, DataBaseModel DbContext)
        {
            foreach (AuthorModel newauthor in newauthors)
            {
                SelectedBook.authors.Remove(DbContext.authors.Find(newauthor.Id));
            }
        }
        public static void AddBooks(AuthorModel SelectedAuthor, List<BookModel> newbooks, DataBaseModel DbContext)
        {
            foreach (BookModel newbook in newbooks)
            {
                SelectedAuthor.books.Add(DbContext.books.Find(newbook.Id));
            }
        }
        public static void RemoveBooks(AuthorModel SelectedAuthor, List<BookModel> newbooks, DataBaseModel DbContext)
        {
            foreach (BookModel newbook in newbooks)
            {
                SelectedAuthor.books.Remove(DbContext.books.Find(newbook.Id));
            }
        }
        public static void CloseWindow(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }
        public static bool IsFilled(string firststring, string secondstring)
        {
            if ((String.IsNullOrWhiteSpace(firststring)) || (String.IsNullOrEmpty(secondstring)))
            {
                return false;
            }
            else return true;
        }
        public static bool IsFilled(string firststring)
        {
            if (String.IsNullOrWhiteSpace(firststring))
            {
                return false;
            }
            else return true;
        }
    }
}
