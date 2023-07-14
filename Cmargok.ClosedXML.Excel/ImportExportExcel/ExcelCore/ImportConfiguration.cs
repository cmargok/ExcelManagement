namespace SDS.Wapi.commons.utils.ImportExportExcel.ExcelCore
{
    public class ImportConfiguration
    {
        public int RowInit { get; set; } = 1;

        public int ColumnInit { get; set; } = 1;
        public int ColumnDiscount { get; set; } = 0;

        public int ObjectPropertyInit { get; set; } = 0;
        public int ObjectPropertyEnd { get; set; } = 0;

        public int SheetNumber { get; set; } = 1;
    }
}
