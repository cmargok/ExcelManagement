
using ClosedXML.Excel;
using SDS.Wapi.commons.utils.ImportExportExcel.ExcelCore;
using System.Data;

    namespace SDS.Wapi.commons.utils.ImportExportExcel.Import
{
    public interface IImportExcel
    {

        public IEnumerable<T> ImportToIEnumarableOf<T>(IXLWorkbook Workbook, ImportConfiguration mapInfo) where T : new() ;

        public List<T> ImportToListOf<T>(IXLWorkbook Workbook, ImportConfiguration mapInfo) where T : new() ;

        public List<T> ImportToListOfFilter<T>(IXLWorkbook Workbook, ImportConfiguration mapInfo, string filter) where T : new();






       
    }
}
