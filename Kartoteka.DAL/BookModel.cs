using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kartoteka.DAL
{
    public class BookModel
    /// DTO для передачи экземпляров класса Book в базу данных и обратно
    {
        [Required]
        public int Id { get; set; }
        public int Year { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<AuthorModel> authors { get; set; }
        public BookModel()
        {
            this.authors = new HashSet<AuthorModel>();
        }
    }
}
