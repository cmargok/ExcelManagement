namespace SDS.Wapi.commons.utils.ImportExportExcel.ExcelCore
{
    public class ImportTinyConfiguration
    {
        /// <summary>
        /// determina en que fila iniciar
        /// </summary>
        public int RowInit { get; set; }

        /// <summary>
        /// determina en que columna iniciar
        /// </summary>
        public int ColumnsCount { get; set; }

        /// <summary>
        /// determina en que propiedad iniciar
        /// </summary>
        public int ObjectPropertyInit { get; set; } = 0;


        /// <summary>
        /// determina en cuantas propiedas no hara el check, el numero resto del total de las propiedas, osea no cogeria las ultimas
        /// </summary>
        public int ObjectPropertyEnd { get; set; } = 0;


    }

}
