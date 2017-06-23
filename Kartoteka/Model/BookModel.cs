using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kartoteka
{
    public class BookModel : ObservableObject
    {
        [Required]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public virtual ICollection<AuthorModel> authors { get; set; }

        public BookModel()
        {
            this.authors = new HashSet<AuthorModel>();
        }
    }
}
