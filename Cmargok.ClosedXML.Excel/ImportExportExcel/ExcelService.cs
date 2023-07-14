using SDS.Wapi.commons.utils.ImportExportExcel.ExcelCore;
using SDS.Wapi.commons.utils.ImportExportExcel.Export;
using SDS.Wapi.commons.utils.ImportExportExcel.Import;

namespace SDS.Wapi.commons.utils.ImportExportExcel
{
    public class ExcelService : IExcelService
    {
        public IImportExcel ImportTo { get; }
        public IExCore Core { get; }
        public IExportExcel ExportTo { get; }

        public ExcelService(IImportExcel _Import, IExCore _Core, IExportExcel _ExportTo)
        {
            ImportTo = _Import;
            Core = _Core;
            ExportTo = _ExportTo;
        }

       


    }

   


    
}
