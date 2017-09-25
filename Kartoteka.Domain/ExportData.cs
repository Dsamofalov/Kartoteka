using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kartoteka.Domain
{
    public class ExportData
    {
        public string FileName { get; set; }
        public byte[] Data { get; set; }
    }
}
