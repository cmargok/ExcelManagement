using System.Data;
using static RowsProblem.RPA.Rq4.DataCompareManager;

namespace RowsProblem.RPA.Rq4
{
    //dtos y objetos de trasnferencia
    public class DataCompareDto
    {
        public IEnumerable<DataRow> Son { get; set; }
        public List<string[]> filters { get; set; }
        public string NameColumn { get; set; } = "";
        public List<DuplasIndex> Indexes_Father_Son { get; set; } = new List<DuplasIndex>();
        public List<DuplasIndex> Indexes_Row_Row{ get; set; } = new List<DuplasIndex>();
        public List<DuplasIndexValue> Indexes_FatherFather { get; set; } = new List<DuplasIndexValue>();
        public List<DuplasIndexFechas> Indexes_Father_Son_Dates { get; set; } = new List<DuplasIndexFechas>();

    }

    public class DuplasIndex
    {
        public int IndexSource { get; set; }
        public int IndexTarget { get; set; }
        public DuplasIndex(int indexSource, int indexTarget)
        {
            IndexSource = indexSource;
            IndexTarget = indexTarget;
        }
    }

    public class DuplasIndexFechas
    {
        public int IndexSource { get; set; }
        public int IndexTarget { get; set; }
        public DateCompare Operacion { get; set; } = DateCompare.Igual;
        public DateCompare DiferenciaOperacion { get; set; } = DateCompare.Igual;

        public int DiasDiferencia { get; set; } = 0;
        public DuplasIndexFechas(int indexSource, int indexTarget, DateCompare operacion, DateCompare diferenciaOperacion, int diasDiferencia)
        {
            IndexSource = indexSource;
            IndexTarget = indexTarget;
            Operacion = operacion;
            DiferenciaOperacion = diferenciaOperacion;
            DiasDiferencia = diasDiferencia;
        }
        public DuplasIndexFechas(int indexSource, int indexTarget, DateCompare operacion)
        {
            IndexSource = indexSource;
            IndexTarget = indexTarget;
            Operacion = operacion;
        }
    }

    public class DuplasIndexValue
    {
        public int Index { get; set; }
        public string Value { get; set; }
        public DuplasIndexValue(int index, string value)
        {
            Index = index;
            Value = value;
        }
    }

    public enum DateCompare
    {
        menor,
        menorIgual,
        Mayor,
        MayorIgual,
        Igual,
        DiaHabilSiguiente,
    }



}
