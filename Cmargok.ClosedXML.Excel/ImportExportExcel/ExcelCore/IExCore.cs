using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.IO;

namespace SDS.Wapi.commons.utils.ImportExportExcel.ExcelCore
{
    public interface IExCore
    {
        public bool VerifyFileExtension(IFormFile file);    

        public IXLWorkbook CreateWorkBook();

        public IXLWorkbook CreateWorkBook(IFormFile file);






    }
}
