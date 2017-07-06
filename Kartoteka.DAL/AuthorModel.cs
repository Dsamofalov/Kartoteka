using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kartoteka.DAL
{
    public class AuthorModel
    {
        [Required]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SecondName { get; set; }
        public virtual ICollection<BookModel> books { get; set; }
        public AuthorModel()
        {
            this.books = new HashSet<BookModel>();
        }
    }
}
