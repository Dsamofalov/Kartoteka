using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kartoteka
{
    public class BookModel : ObservableObject
    {
        private int id;
        private string title;
        private string genre;
        private ObservableCollection<AuthorModel> authors;

        public int ID
        {
            get
            {
                return id;
            }
            set
            {
                Set<int>(() => this.ID, ref id, value);
            }
        }

        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                Set<string>(() => this.Title, ref title, value);
            }
        }

        public string Genre
        {
            get
            {
                return genre;
            }
            set
            {
                Set<string>(() => this.Genre, ref genre, value);
            }
        }

        public ObservableCollection<AuthorModel> Authors
        {
            get
            {
                return authors;
            }

            set
            {
                Set<ObservableCollection<AuthorModel>>(() => this.Authors, ref authors, value);
            }
        }
    }
}
