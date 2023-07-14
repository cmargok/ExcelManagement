using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using SDS.Wapi.commons.utils.ImportExportExcel.ExcelCore;
using System.Data;
using System.Reflection;

namespace SDS.Wapi.commons.utils.ImportExportExcel.Import
{
   

    public class ImportExcel : ExCore, IImportExcel
    {
        
        public IEnumerable<T> ImportToIEnumarableOf<T>(IXLWorkbook Workbook, ImportConfiguration mapInfo) where T : new() => GetListOf<T>(Workbook, mapInfo);   

        public List<T> ImportToListOf<T>(IXLWorkbook Workbook, ImportConfiguration mapInfo) where T : new() => GetListOf<T>(Workbook, mapInfo);


        public List<T> ImportToListOfFilter<T>(IXLWorkbook Workbook, ImportConfiguration mapInfo, string filter) where T : new() => GetListOfByFirstColumnFilter<T>(Workbook, mapInfo, filter);   




        private List<T> GetListOfByFirstColumnFilter<T>(IXLWorkbook Workbook, ImportConfiguration mapInfo, string filter) where T : new()
        {
            T entity = new();

            var entityProperties = GetTypeProperties(entity);        

            var FileRows = Workbook.Worksheet(mapInfo.SheetNumber).RowsUsed(r => r.FirstCell().GetString() == filter).Skip(mapInfo.RowInit);

            int PropertiesCount = entityProperties.Length - mapInfo.ObjectPropertyEnd;

            int ExcelColumns = Workbook.Worksheet(mapInfo.SheetNumber).ColumnsUsed().Count() - mapInfo.ColumnDiscount;

            CheckLength(PropertiesCount, ExcelColumns);

            var ListEntities = CastTo(mapInfo, ref entity, entityProperties, FileRows, PropertiesCount);

            return ListEntities;
        }
















        private List<T> GetListOf<T>(IXLWorkbook Workbook, ImportConfiguration mapInfo) where T : new()
        {
            T entity = new();

            var entityProperties = GetTypeProperties(entity);    
            

            var FileRows = Workbook.Worksheet(mapInfo.SheetNumber).RowsUsed().Skip(mapInfo.RowInit);

            int PropertiesCount = entityProperties.Length - mapInfo.ObjectPropertyEnd;

            int ExcelColumns = Workbook.Worksheet(mapInfo.SheetNumber).ColumnsUsed().Count() - mapInfo.ColumnDiscount;
           
            CheckLength(PropertiesCount, ExcelColumns);

            var ListEntities = CastTo(mapInfo, ref entity, entityProperties, FileRows, PropertiesCount);

            return ListEntities;
        }



        private static void CheckLength(int properties, int columns)
        {
            if (properties > columns) throw new IndexOutOfRangeException("Columns Length is less than Properties Count");
        }


        private List<T> CastTo<T>(ImportConfiguration mapInfo, ref T entity, PropertyInfo[] entityProperties, IEnumerable<IXLRow> FileRows, int ColumnsCount) where T : new()
        {

            List<T> ListEntities = new();
            int ColumnInit = mapInfo.ColumnInit;

            foreach (var row in FileRows)
            {
                for (int i = mapInfo.ObjectPropertyInit; i < ColumnsCount; i++)
                {
                    var field = entityProperties[i];
                    field.SetValue(entity, ParseDataType(field, row.Cell(ColumnInit)));
                    ColumnInit++;
                }
                ColumnInit = mapInfo.ColumnInit;
                ListEntities.Add(entity);
                entity = new T();
            }
            return ListEntities;
        }









    }
}
