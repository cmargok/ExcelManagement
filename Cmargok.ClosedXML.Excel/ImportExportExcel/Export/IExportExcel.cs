

using ClosedXML.Excel;

namespace SDS.Wapi.commons.utils.ImportExportExcel.Export
{
    public interface IExportExcel //: IExcelCore
    {      
        public byte[] ExportToExcel(IXLWorkbook workbook);
        public byte[] ExportToExcel<T>(IEnumerable<T> list, string WorkSheetName) where T : new();
        public byte[] ExportToExcel<T>(IEnumerable<T> list, string WorkSheetName, ExportPreferences preferences) where T : new();
        public void CreateWorkSheet<T>(IXLWorkbook workbook, IEnumerable<T> list, string WorkSheetName) where T : new();
        public void CreateWorkSheet<T>(IXLWorkbook workbook, IEnumerable<T> list, string WorkSheetName, ExportPreferences preferences) where T : new();
    }
}
