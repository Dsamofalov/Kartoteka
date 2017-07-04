using System;
using System.Collections.Generic;
using System.Text;

namespace Kartoteka.Domain
{
    public class Book
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public  List<Author> authors { get; set; }
    }
}
