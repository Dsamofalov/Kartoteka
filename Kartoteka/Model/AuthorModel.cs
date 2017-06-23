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
    public class AuthorModel : ObservableObject
    {
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public virtual ICollection<BookModel> books { get; set; }
        public AuthorModel()
        {
            this.books = new HashSet<BookModel>();
        }

    }
}
