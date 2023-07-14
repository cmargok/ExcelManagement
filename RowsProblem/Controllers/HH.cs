using OfficeOpenXml;



namespace RowsProblem.Controllers
{
    public class HH{


        public static void HHh(IFormFile file, string nombre, string valor)
        {



            using (var Libro = new ExcelPackage(file.OpenReadStream()))
            {
                var hoja = Libro.Workbook.Worksheets[0];

             //   ChangeValue(hoja, nombre, valor);

                Libro.Save();

            }
        }
        public void ChangeSheetValue(string nombre, string valor)
        {
            using (var Libro = new ExcelPackage(AbrirWorkBook(pathpath)))
            {
                //aqui defines el nombre de la hoja a verificar
                var hoja = Libro.Workbook.Worksheets["Setts"];

                // Obtener el índice de la columna que quieres filtrar (en este caso, la columna 0)
                int columnaFiltrar = 0;

                // Buscar la fila que tiene el valor de "nombre" en la columna correspondiente
                var fila = hoja.Cells["A1:A" + hoja.Dimension.End.Row].FirstOrDefault(c => c.Value?.ToString() == nombre)?.Start.Row;

                // Si se encontró la fila, asignar el valor a la celda correspondiente
                if (fila.HasValue)
                {
                    var celda = hoja.Cells[fila.Value, columnaFiltrar + 2];
                    celda.Value = valor;
                }
                Libro.Save();
            }
        }

        private string pathpath = @"C:\Users\cmarg\Documents\fREDY\rq 6\Book1.xlsx";

        private FileInfo AbrirWorkBook(string path)
        {
            return new FileInfo(path);
        }


        public  void ChangeValue(ExcelWorksheet hoja, string nombre, string valor)
        { 
                   
        }
    }





}
