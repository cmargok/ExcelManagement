using static RowsProblem.RPA.Rq4.DataCompareManager;

namespace RowsProblem.RPA.Rq4
{
    public class CasesGenerator
    {
        private enum Procesos
        {
            fuerte,
            simpleperofuerte
        }

        public static DataCompareDto SelectCase(DataCompareDto data, string NameProcess)
        {
            data.Indexes_FatherFather.Clear();
            data.Indexes_Father_Son.Clear();
            data.Indexes_Father_Son_Dates.Clear();
            data.NameColumn = "";


            NameProcess = NameProcess.ToLower().Trim();

            var Process = (Procesos)Enum.Parse(typeof(Procesos), NameProcess);

            switch (Process)
            {
                case Procesos.fuerte:

                    //ponerle el nombre de la columna
                    data.NameColumn = "Columna Nueva Prueba";

                    //agregar los index para comparar entre el padre mismo
                    //                               (index ColumnaUno, index ColumnaDos)
                    data.Indexes_FatherFather.Add(new DuplasIndexValue(3, "Premium"));
                    data.Indexes_FatherFather.Add(new DuplasIndexValue(8, "IA"));

                    //agregar los index para comparar entre padre e hijo
                    //                               (index padre, index hijo)
                    data.Indexes_Father_Son.Add(new DuplasIndex(1, 2));
                    data.Indexes_Father_Son.Add(new DuplasIndex(2, 5));



                    //agregar los index para comparar entre padre e hijo con fechas
                    //                               (index padre, index hijo)
                    data.Indexes_Father_Son_Dates.Add(new DuplasIndexFechas(0, 12, DateCompare.menorIgual, DateCompare.menorIgual, 15));
                    //  data.Indexes_Father_Son_Dates.Add(new DuplasIndexFechas(0, 12, DateCompare.DiaHabilSiguiente));


                    //agregar los index del padre versus el padre
                    data.Indexes_Row_Row.Add(new DuplasIndex(1, 2));
                    data.Indexes_Row_Row.Add(new DuplasIndex(2, 5));



                    break;
                    
                case Procesos.simpleperofuerte:


                    break;


                default:
                    throw new ArgumentNullException("No process exists related to");
            }

            return data;
        }
    }
}
