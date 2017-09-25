using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Kartoteka.Domain
{
    public class XLSXDataExporter : IDataExporter
    {
        public ExportData AuthorsExport(List<Author> authors)
        {
            var exportData = new ExportData();

            using (var ms = new MemoryStream())
                {
                var spreedDoc = SpreadsheetDocument.Create(ms, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook);
                WorkbookPart workbookpart = spreedDoc.AddWorkbookPart();
                workbookpart.Workbook = new Workbook();
                WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
                var sheetData = new SheetData();
                worksheetPart.Worksheet = new Worksheet(sheetData);
                Sheets sheets = spreedDoc.WorkbookPart.Workbook.
                    AppendChild<Sheets>(new Sheets());
                Sheet sheet = new Sheet()
                {
                    Id = spreedDoc.WorkbookPart.
                    GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = "Authors"
                };
                int rowindex = 1;
                foreach (var author in authors)
                {
                    Row row = new Row();
                    row.RowIndex = (uint)rowindex;

                    if (rowindex == 1) //Header 
                    {
                        row.AppendChild(AddCellWithText("Id"));
                        row.AppendChild(AddCellWithText("FirstName"));
                        row.AppendChild(AddCellWithText("SecondName"));
                        row.AppendChild(AddCellWithText("LastName"));
                    }
                    else //Data 
                    {
                        row.AppendChild(AddCellWithNumber(author.Id));
                        row.AppendChild(AddCellWithText(author.FirstName));
                        row.AppendChild(AddCellWithText(author.SecondName));
                        row.AppendChild(AddCellWithText(author.LastName));
                    }

                    sheetData.AppendChild(row);
                    rowindex++;
                }
                sheets.Append(sheet);
                spreedDoc.Close();
                exportData.Data = ms.ToArray();
                return exportData;
            }
        }
        public ExportData BooksExport(List<Book> books)
        {
            var exportData = new ExportData();

            using (var ms = new MemoryStream())
            {
                var spreedDoc = SpreadsheetDocument.Create(ms, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook);
                WorkbookPart workbookpart = spreedDoc.AddWorkbookPart();
                workbookpart.Workbook = new Workbook();
                WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
                var sheetData = new SheetData();
                worksheetPart.Worksheet = new Worksheet(sheetData);
                Sheets sheets = spreedDoc.WorkbookPart.Workbook.
                    AppendChild<Sheets>(new Sheets());
                Sheet sheet = new Sheet()
                {
                    Id = spreedDoc.WorkbookPart.
                    GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = "Books"
                };

                    int rowindex = 1;
                    foreach (var book in books)
                    {
                        Row row = new Row();
                        row.RowIndex = (uint)rowindex;

                        if (rowindex == 1) //Header 
                        {
                            row.AppendChild(AddCellWithText("Id"));
                            row.AppendChild(AddCellWithText("Year"));
                            row.AppendChild(AddCellWithText("Name"));
                            row.AppendChild(AddCellWithText("Description"));
                        }
                        else //Data 
                        {
                            row.AppendChild(AddCellWithNumber(book.Id));
                            row.AppendChild(AddCellWithNumber(book.Year));
                            row.AppendChild(AddCellWithText(book.Name));
                            row.AppendChild(AddCellWithText(book.Description));
                        }

                        sheetData.AppendChild(row);
                        rowindex++;
                    }
                sheets.Append(sheet);
                spreedDoc.Close();
                exportData.Data = ms.ToArray();
                return exportData;
            }
        }

            static Cell AddCellWithText(string text)
        {
                Cell c1 = new Cell();
                c1.DataType = CellValues.InlineString;
                InlineString inlineString = new InlineString();
                Text t = new Text();
                t.Text = text;
                inlineString.AppendChild(t);

                c1.AppendChild(inlineString);

                return c1;
            }
            static Cell AddCellWithNumber(int number)
        {
                Cell c1 = new Cell();
                c1.DataType = CellValues.Number;
                c1.CellValue = new CellValue(Convert.ToString(number));
                return c1;
        }
    }
}
