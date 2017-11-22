using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kartoteka.Domain
{
    public interface IDataExporter
    /// интерфейс, содержащий абстрактные методы для экспортёров данных.
    {
        ExportData AuthorsExport(List<Author> authors);
        ExportData BooksExport(List<Book> books);
    }
}
