using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kartoteka.Domain
{
    public class ExportData
    /// DTO для записи данных в Excel-файл
    {
        public string FileName { get; set; }
        public string Folder { get; set; }
        public byte[] Data { get; set; }
    }
}
