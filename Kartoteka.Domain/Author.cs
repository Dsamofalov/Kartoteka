using System;
using System.Collections.Generic;
using System.Text;

namespace Kartoteka.Domain
{
    public class Author
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SecondName { get; set; }
        public List<Book> books { get; set; }
    }
}
