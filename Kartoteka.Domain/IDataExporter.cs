﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kartoteka.Domain
{
    public interface IDataExporter
    {
        ExportData AuthorsExport(List<Author> authors);
        ExportData BooksExport(List<Book> books);
    }
}
