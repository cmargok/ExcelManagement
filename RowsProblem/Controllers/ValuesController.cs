

using Cmargok.ExcelDataReader.ImportExcel.ExcelCore.Import;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using RowsProblem.RPA;
using RowsProblem.RPA.Rq4;
using System.Data;
using System.Diagnostics;
using System.Globalization;

namespace RowsProblem.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        private readonly IImport _excel;


        public ValuesController(IImport excel)
        {
            _excel = excel;        
        }

        [HttpGet]
        public IActionResult Get(string A)
        {
            var h = GetData(A);

      

            return Ok(h);
        }


        private DateTime GetDataV1(string A)
        {
            DateTime DateA;

            if (!DateTime.TryParseExact(A, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateA))
            {
                if (!DateTime.TryParseExact(A, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateA))
                {
                    if (!DateTime.TryParseExact(A, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateA))
                    {
                        if (!DateTime.TryParseExact(A, "d/M/yyyy h:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateA))
                        {    
                            if (!DateTime.TryParseExact(A, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateA))
                            {
                                if (!DateTime.TryParseExact(A, "d/M/yyyy h:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateA))
                                {
                                    DateA = DateTime.ParseExact(A, "dd/MM/yy", CultureInfo.InvariantCulture);
                                }
                                
                            }
                        }
                    }
                }
            }
            return DateA;
        }

        private static readonly List<string> DateFormats = new List<string>
        {
            "dd/MM/yyyy",
            "M/d/yyyy h:mm:ss tt",
            "d/M/yyyy h:mm:ss tt",
            "MM/dd/yyyy",
            "d/M/yyyy h:mm",
            "dd/MM/yy",
        };

        private DateTime GetData(string A)
        {
            DateTime DateA;

            if(A.Contains("\"")) A = A.Replace("\"", "/");

            foreach (var format in DateFormats)
            {
                if (DateTime.TryParseExact(A, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateA))
                {
                    return DateA;
                }
            }
            
            throw new ArgumentException("Could not parse date: " + A);
        }





        [HttpPost]
        public async Task<IActionResult> memae()
        {
          /*  HH.HHh("pulgas","sin retorno");*/ return Ok();
        }









        /*

        [HttpPost]
        public async Task<IActionResult> meme(IFormFile file)
        {
            var time = new Stopwatch();
            time.Start();
            // var mom = exe(file);
   
          //  time.Start();

            var Exceles = _excel.GetDataSet(file);   

            var Padre = Exceles.Tables[0];
            var tabla1 = Exceles.Tables[1];      
           /* List<string> LsFestivos = new();

            CallRuleEleven(Padre, 3, "A", ">", "60", 1, "top,premium", 4);
 


            var h = VerifyFieldsDepurado(Padre,tabla1, "fuerte", "11,IVFCOL658756576456");
            
            time.Stop();
            //Exceles = null;
            //tabla1 = null;

            return Ok($"Tiempo total transcurrido {(time.ElapsedMilliseconds / 1000)} Segundos");
        }*/
        
        private DataTable VerifyFieldsDepurado(DataTable Father, DataTable Son, string NameProcess, string filters)
        {
            DataCompareManager dataCompareManager = new DataCompareManager();
            //LeerArchivoFestivos();

            //var listFestivos = LsFestivos;
            var listFestivos = new List<DateTime>();
            return dataCompareManager.ComparingDataDeeply(Father, Son, NameProcess, filters, listFestivos);
        }

        private DataTable CallRuleEleven(DataTable table, int index, string filterValue, string operadorStr, string Edad, int indexEdad, string segmento, int indexSegmento)
        {
           
            var segmentos = segmento.Split(',').ToList();

            if(segmentos.Count == 0  ) { 
                segmentos.Add(segmento);
            }
            DatatableManager datatableManager = new();

            return datatableManager.Rule11(table, index, filterValue, operadorStr, Edad, indexEdad, segmentos, indexSegmento);

        }

     


        private (DataTable, int, long) exe(IFormFile file)
        {
            var time = new Stopwatch();
            time.Start();

            var datatableManager = new DatatableManager();


            var Exceles = _excel.GetDataSet(file);


            int tamañoTabla1 = Exceles.Tables[0].Rows.Count;

            var Padre = ReadData(Exceles.Tables[0], 2, 7, 8);

            var Padre2 = Exceles.Tables[0];
            var tabla1 = Exceles.Tables[1];
            var tabla2 = Exceles.Tables[2];
            var tabla3 = Exceles.Tables[3];
            var tabla4 = Exceles.Tables[4];


            var n = CombineData(Padre, tabla1, tabla2, tabla3, tabla4);


            Exceles = null;
            tabla1 = null;
            tabla2 = null;
            tabla3 = null;
            //Padre = null;

            datatableManager = null;
            time.Stop();


            return (n, tamañoTabla1, time.ElapsedMilliseconds);


        }

        private DataTable ReadData(DataTable table, int FechaColumn, int NombreColumn, int DocumentoColumn)
        {
            char splitCharacter = ';';

            string NombreColumnaHora = "Hora";

            var DataManager = new DatatableManager();


            return DataManager.SplitDatatableByParams(table, FechaColumn, NombreColumn, DocumentoColumn, splitCharacter, NombreColumnaHora);
        }

        private DataTable CombineData(DataTable FatherTable, DataTable FirstTable, DataTable SecondTable, DataTable ThirdTable, DataTable FourthTable)
        {
            //parametros para modificar en caso tal...-----------------------------------------------------------------
            int indexFilterFirstTable = 5; //Columna para filtrar en la tabla 1
            string GetDataFromFirstTableColumns = "9,10,12"; //De cuales columnas traermos la data de la tabla 1          
            List<string> NamesColumnsFirstTable = new List<string>() { "Area de bogota", "Codigo de area", "CedulaDeHijo" };

            int indexFilterSecondTable = 0;//Columna para filtrar en la tabla 2
            string GetDataFromSecondTableColumns = "1,3";//De cuales columnas traermos la data de la tabla 2
            List<string> NamesColumnsSecondTable = new List<string>() { "Perfil", "Edad" };

            int indexFilterThirdTable = 0; //Columna para filtrar en la tabla 3
            string GetDataFromThirdTableColumns = "1,4";//De cuales columnas traermos la data de la tabla 3
            List<string> NamesColumnsThirdTable = new List<string>() { "Nombre completo", "Gestor" };

            int indexFilterFourthTable = 1; //Columna para filtrar en la tabla 3
            string GetDataFromFourthTableColumns = "0,2";//De cuales columnas traermos la data de la tabla 3
            List<string> NamesColumnsFourthTable = new List<string>() { "Codigo gestor", "Codigo oficina" };

            //index de la columna para coger el valor e ir a buscar en las otras tablas
            int IndexFatherTableToSearchInFirstTable = 5;
            int IndexFatherTableToSearchInSecondTable = 26;
            int IndexFatherTableToSearchInThirdTable = 8;
            int IndexFatherTableToSearchInFourthTable = 8;


            //----------NO TOCAR-------------------

            var DataManager = new DatatableManager();

            var configs = new DataConfigs(FirstTable, SecondTable, ThirdTable, FourthTable);

            configs.Indexes = DataManager.MergeIndex(indexFilterFirstTable, indexFilterSecondTable, indexFilterThirdTable, indexFilterFourthTable);
            configs.Columnas = DataManager.MergeColumnas(GetDataFromFirstTableColumns, GetDataFromSecondTableColumns, GetDataFromThirdTableColumns, GetDataFromFourthTableColumns);
            configs.FatherIndexes = DataManager.MergeIndex(IndexFatherTableToSearchInFirstTable, IndexFatherTableToSearchInSecondTable, IndexFatherTableToSearchInThirdTable, IndexFatherTableToSearchInFourthTable);
            configs.TablePadreColumnsCount = FatherTable.Columns.Count;
            DataManager.SetColumnsName(FatherTable, FatherTable.Columns.Count, NamesColumnsFirstTable, NamesColumnsSecondTable, NamesColumnsThirdTable, NamesColumnsFourthTable);
            var hg = DataManager.TakeDataFromChildrenTables(FatherTable, configs);

            DataManager.DeleteDuplicateRow(hg, 0);

            return hg;

        }





        private void EliminarFilasVacias(DataTable dt)
        {
            var filas_A_Revisar = 2;

            int rowCount = dt.Rows.Count - 1;

            for (int i = rowCount; i == rowCount - filas_A_Revisar; i--)
            {
                if (i <= 0) continue; // Si no hay suficientes filas, salta esta iteración

                DataRow dr = dt.Rows[i];

                bool tieneDatos = false;

                for (int j = 0; j <= rowCount; j++)
                {
                    var item = dr[j].ToString();

                    if (item != null && item.Trim() != "")
                    {
                        tieneDatos = true;
                        break;
                    }                   
                }

                if (!tieneDatos)
                {
                    dt.Rows.RemoveAt(i);
                }

            }
        }



    }





}
