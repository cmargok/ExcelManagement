

using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using SDS.Wapi.commons.utils.ImportExportExcel.ExcelCore;
using System.Drawing;
using System.Reflection;
using System.Runtime.Intrinsics.X86;

namespace SDS.Wapi.commons.utils.ImportExportExcel.Export
{
    public class ExportExcel : IExportExcel
    {



        public byte[] ExportToExcel(IXLWorkbook workbook)
        {
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }



        public byte[] ExportToExcel<T>(IEnumerable<T> list, string WorkSheetName) where T : new()
        {
            IXLWorkbook workbook = new XLWorkbook();
            var WorkSheet = workbook.Worksheets.Add(WorkSheetName);
            var propiedades = new T().GetType().GetProperties();
            //int columns = 1;
            //foreach (PropertyInfo prop in propiedades)
            //{
            //    WorkSheet.Cell(1, columns).Value = prop.Name;
            //    WorkSheet.Cell(1, columns).Style.Fill.BackgroundColor = XLColor.PeachOrange;
            //    WorkSheet.Cell(1, columns).Style.Font.Bold = true;
            //    columns++;
            //}

            FirstRowStyle(WorkSheet, propiedades);

            WorkSheet.FirstColumnUsed().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            WorkSheet.Cell(2, 1).Value = list;

            WorkSheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();

        }







        public byte[] ExportToExcel<T>(IEnumerable<T> list, string WorkSheetName, ExportPreferences preferences) where T : new()
        {

            IXLWorkbook workbook = new XLWorkbook();

            var WorkSheet = workbook.Worksheets.Add(WorkSheetName);

            var propiedades = new T().GetType().GetProperties();            

            FirstRowStyle(WorkSheet, propiedades);                   

            WorkSheet.Columns().AdjustToContents();

            WorkSheet.Cell(2, 1).Value = list;

            foreach (var preference in preferences.Preferencias)
            {
                WorkSheet.Column(preference.Column).Style.Alignment.SetHorizontal(preference.Alineacion);
                if (preference.ColumnWidth != 0) WorkSheet.Column(preference.Column).Width = preference.ColumnWidth;
            }


            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();

        }


        private void FirstRowStyle(IXLWorksheet WorkSheet, PropertyInfo[] propiedades)
        {
            int columns = 1;
            foreach (PropertyInfo prop in propiedades)
            {
                WorkSheet.Cell(1, columns).Value = prop.Name;
                WorkSheet.Cell(1, columns).Style.Fill.BackgroundColor = XLColor.PeachOrange;
                WorkSheet.Cell(1, columns).Style.Font.Bold = true;
                columns++;
            }
        }
















        public void CreateWorkSheet<T>(IXLWorkbook workbook, IEnumerable<T> list, string WorkSheetName) where T : new()
        {

            var WorkSheet = workbook.AddWorksheet(WorkSheetName);
            var propiedades = new T().GetType().GetProperties();
            //int columns = 1;

            //foreach (PropertyInfo prop in propiedades)
            //{
            //    WorkSheet.Cell(1, columns).Value = prop.Name;
            //    WorkSheet.Cell(1, columns).Style.Fill.BackgroundColor = XLColor.PeachOrange;
            //    WorkSheet.Cell(1, columns).Style.Font.Bold = true;
            //    columns++;
            //}
            FirstRowStyle(WorkSheet, propiedades);

            WorkSheet.FirstColumnUsed().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            WorkSheet.Column(2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            WorkSheet.Cell(2, 1).Value = list;

            WorkSheet.Columns().AdjustToContents();

           

        }


        public void CreateWorkSheet<T>(IXLWorkbook workbook, IEnumerable<T> list, string WorkSheetName, ExportPreferences preferences) where T : new()
        {


            var WorkSheet = workbook.Worksheets.Add(WorkSheetName);

            var propiedades = new T().GetType().GetProperties();

           // int columns = 1;

            //foreach (PropertyInfo prop in propiedades)
            //{
            //    WorkSheet.Cell(1, columns).Value = prop.Name;
            //    WorkSheet.Cell(1, columns).Style.Fill.BackgroundColor = XLColor.PeachOrange;
            //    WorkSheet.Cell(1, columns).Style.Font.Bold = true;
            //    columns++;
            //}
            FirstRowStyle(WorkSheet, propiedades);

            WorkSheet.Cell(2, 1).Value = list;            

            WorkSheet.Columns().AdjustToContents();

            foreach (var preference in preferences.Preferencias)
            {
                WorkSheet.Column(preference.Column).Style.Alignment.SetHorizontal(preference.Alineacion);
                if (preference.ColumnWidth != 0) WorkSheet.Column(preference.Column).Width = preference.ColumnWidth;
            }
         



        }





    }


    public record ExportPreferences
    {
        public List<StylePreference> Preferencias { get; set; } = new();
    }

    public record StylePreference
    {
        public int Column { get; set; }
        public XLAlignmentHorizontalValues Alineacion { get; set; } = XLAlignmentHorizontalValues.Center;
        public int ColumnWidth { get; set; } = 0;      

    }
}
