using SDS.Wapi.commons.utils.ImportExportExcel.ExcelCore;
using SDS.Wapi.commons.utils.ImportExportExcel.Export;
using SDS.Wapi.commons.utils.ImportExportExcel.Import;

namespace SDS.Wapi.commons.utils.ImportExportExcel
{
    public interface IExcelService
    {
        public IImportExcel ImportTo { get; }
        public IExportExcel ExportTo { get; }
        public IExCore Core { get; }
    }
}
