using DocumentFormat.OpenXml.Office2013.ExcelAc;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using static RowsProblem.RPA.Rq4.DataCompareManager;

namespace RowsProblem.RPA.Rq4 
{
    public class DataCompareManager
    { //****************************
        private List<DateTime> _Festivos = new List<DateTime>();
        //****************************
        public DataTable ComparingDataDeeply(DataTable Father, DataTable Son, string NameProcess, string filters, List<DateTime> Festivos)
        {
            _Festivos.Clear();
            _Festivos = Festivos;

            var data = new DataCompareDto()
            {               
                Son = Son.AsEnumerable(),
                filters = ProcessFilter(filters),
            };

            CasesGenerator.SelectCase(data, NameProcess);

            Father.Columns.Add(data.NameColumn);           
            var ColumnsLeght = Father.Columns.Count - 1;        
        
            SetDefaultValue(Father, ColumnsLeght,"NO DEPURADO");


            if (data.filters.Count == 0)
            {
                int RowCounter = 1;

                foreach (DataRow Row in Father.Rows)
                {
                    if (RowCounter > 1)
                    {
                        WriteResponseInTable(Father, RowCounter-1, ColumnsLeght, ManageAndCompare(Row, data));
                    }
                    RowCounter++;
                }
            }
            else
            {
                var FilteresRows = FilteringBy(Father, data);

                foreach (var Row in FilteresRows)
                {
                    WriteResponseInTable(Father, Row.Index, ColumnsLeght, ManageAndCompare(Row.Row, data));
                }
            }
            Father.Rows[0][ColumnsLeght] = data.NameColumn;


            return Father;
        }

        private void SetDefaultValue(DataTable table, int lenght,string value)
        {
            foreach (DataRow row in table.Rows)
            {
                row[lenght] = value;
            }
        }

        private void WriteResponseInTable(DataTable Table, int FilaNumero, int ColumnsLeght, bool depurado)
        {
            if (depurado)
            {
                Table.Rows[FilaNumero][ColumnsLeght] = "DEPURADO";
            }    
        }
        public bool EstaVacio(DataRow row)
        {

            for (int i = 0; i < 5; i++)
            {
                var value = row[i].ToString();

                if (string.IsNullOrEmpty(value))
                {
                    return true;
                }
            }
            return false;
        }
           

        private bool ManageAndCompare(DataRow Row, DataCompareDto data) 
        {       
            if(EstaVacio(Row)) { return false; }             


            if (data.Indexes_FatherFather.Count > 0)
            {
                foreach (var i in data.Indexes_FatherFather)
                {         
                    var Value = Row[i.Index].ToString();

                    var Value2 = i.Value;

                    if (string.IsNullOrEmpty(Value) || string.IsNullOrEmpty(Value2)) return false;                    

                    if (!CompareStringIgnoringCase(Value,Value2))
                    {
                        return false;
                    }
                }
            }

            if(data.Indexes_Row_Row.Count > 0)
            {
                foreach (var i in data.Indexes_Row_Row)
                {
                    var Value = Row[i.IndexSource].ToString();

                    var Value2 = Row[i.IndexTarget].ToString();

                    if (string.IsNullOrEmpty(Value) || string.IsNullOrEmpty(Value2)) return false;

                    if (!CompareStringIgnoringCase(Value, Value2))
                    {
                        return false;
                    }
                }
            }





            var query = data.Son;

            if (data.Indexes_Father_Son.Count > 0)
            {      
                foreach (var i in data.Indexes_Father_Son)
                {
                    var FatherValue = Row[i.IndexSource].ToString();

                    if (string.IsNullOrEmpty(FatherValue)) return false;

                    query = query.Where(x => CompareStringIgnoringCase(x[i.IndexTarget].ToString(),FatherValue));
                } 
            }



            if (data.Indexes_Father_Son_Dates.Count > 0)
            {
                foreach (var i in data.Indexes_Father_Son_Dates)
                {                                    
                    string FatherValue = Row[i.IndexSource].ToString();

                    if (string.IsNullOrEmpty(FatherValue)) return false;

                    query = query.Where(x =>
                    {

                        var SonValue = x[i.IndexTarget].ToString();

                        if (string.IsNullOrEmpty(SonValue)) return false;

                        return CompareDate(FatherValue, SonValue, i.Operacion,i.DiferenciaOperacion, i.DiasDiferencia);
                    });
                }                
            }     
            query = query.ToList();
            return query.Any();
        }

        private IEnumerable<RowsAndIndex> FilteringBy(DataTable Father, DataCompareDto data)
        {
            IEnumerable<RowsAndIndex> Rows = new List<RowsAndIndex>();

            int Index;
            string filterValue;
            string filterValue2;
            int Counter = 1;

            if (Counter == 1)
            {
                var values = data.filters[0];
                if (values.Length == 2)
                {
                    Index = int.Parse(values[0]);
                    filterValue = values[1];
                    Rows = Father.AsEnumerable()
                        .Select((row, index) => new { Row = row, Index = index })
                        .Where(x => x.Row[Index].ToString() == filterValue)
                        .Select(x => new RowsAndIndex
                        {
                            Index = x.Index,
                            Row = x.Row
                        });
                }
                else if (values.Length > 2)
                {
                    Index = int.Parse(values[0]);
                    filterValue = values[1];
                    filterValue2 = values[2];
                    Rows = Father.AsEnumerable()
                        .Select((row, index) => new { Row = row, Index = index })
                        .Where(x => x.Row[Index].ToString() == filterValue || x.Row[Index].ToString() == filterValue2)
                        .Select(x => new RowsAndIndex
                        {
                            Index = x.Index,
                            Row = x.Row
                        });
                }
                Counter++;
            }

            if (data.filters.Count > 1 && Counter > 1)
            {
                var values = data.filters[1];

                if (values.Length == 2)
                {
                    Index = int.Parse(values[0]);
                    filterValue = values[1];
                    Rows = Rows.Where(c => c.Row[Index].ToString() == filterValue);
                }
                else if (values.Length > 2)
                {
                    Index = int.Parse(values[0]);
                    filterValue = values[1];
                    filterValue2 = values[2];
                    Rows = Rows.Where(c => c.Row[Index].ToString() == filterValue || c.Row[Index].ToString() == filterValue2);
                }
            }
            return Rows.ToList();
        }

        private bool CompareDate(string Padre, string Hijo, DateCompare operacion,DateCompare DiferenciaOperacion, int diasDiferencia)
        {
            DateTime DatePadre = GetData(Padre.TrimStart().TrimEnd());
            DateTime DateHijo = GetData(Hijo.TrimStart().TrimEnd());

            switch (operacion)
            {
                case DateCompare.menor:                  
                    return DatePadre < DateHijo && ValidateDiferencia(DatePadre, DateHijo, DiferenciaOperacion, diasDiferencia);
                case DateCompare.menorIgual:            
                    return DatePadre <= DateHijo && ValidateDiferencia(DatePadre, DateHijo, DiferenciaOperacion, diasDiferencia);
                case DateCompare.Igual:
                    return DatePadre == DateHijo;
                case DateCompare.Mayor:            
                    return DatePadre > DateHijo && ValidateDiferencia(DatePadre, DateHijo, DiferenciaOperacion, diasDiferencia);
                case DateCompare.MayorIgual:
                    return DatePadre >= DateHijo && ValidateDiferencia(DatePadre, DateHijo, DiferenciaOperacion, diasDiferencia);
                case DateCompare.DiaHabilSiguiente:
                    return ValidarSiguienteDiaHabil(DatePadre, DateHijo);
                default:
                    throw new NotImplementedException();
            }
        }


        private bool ValidateDiferencia(DateTime DatePadre, DateTime DateHijo, DateCompare operacion, int diasDiferencia)
        {
            switch (operacion)
            {
                case DateCompare.menor:           
                    return (DateHijo - DatePadre).Days < diasDiferencia;
                case DateCompare.menorIgual:                   
                    return (DateHijo - DatePadre).Days <= diasDiferencia;
                case DateCompare.Igual:
                    return (DateHijo - DatePadre).Days == diasDiferencia;
                case DateCompare.Mayor:           
                    return (DatePadre - DateHijo).Days > diasDiferencia;
                case DateCompare.MayorIgual:
                    return (DatePadre - DateHijo).Days >= diasDiferencia;
                default:
                    throw new NotImplementedException();
            }
        }

        private DateTime GetData(string A)
        {
            DateTime DateA;

            if (!DateTime.TryParseExact(A, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateA))
            {
                if (!DateTime.TryParseExact(A, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateA))
                {
                    if (!DateTime.TryParseExact(A, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateA))
                    {
                        if(! DateTime.TryParseExact(A, "d/M/yyyy h:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateA))
                        {
                            DateA = DateTime.ParseExact(A, "d/M/yyyy h:mm", CultureInfo.InvariantCulture);
                        }                       
                    }
                }
            }
            return DateA;
        }    

        private List<string[]> ProcessFilter(string filters)
        {
            List<string[]> eachFilter = new List<string[]>();

            if (string.IsNullOrEmpty(filters))
            {
                return eachFilter;
            }

            if (filters.Contains(';'))
            {
                var divide = filters.Split(';');                 

                for (int i = 0; i < divide.Length; i++)
                {
                    if (divide[i].Contains(',')) { }
                    eachFilter.Add(divide[i].Split(','));
                }
            }
            else if (filters.Contains(','))
            {
                eachFilter.Add(filters.Split(','));
            }
           
            return eachFilter;
        }

        private bool CompareStringIgnoringCase( string a, string b)
        {
            a = a.TrimStart().TrimEnd();
            b = b.TrimStart().TrimEnd();

            return a.Equals(b, StringComparison.OrdinalIgnoreCase);

        }

        private class RowsAndIndex
        {
            public DataRow Row { get; set; }
            public int Index { get; set; }
        }


        private bool ValidarSiguienteDiaHabil(DateTime datePadre, DateTime dateHijo)
        {
            var NextBussinessDay = dateHijo;           

            if (NextBussinessDay.DayOfWeek == DayOfWeek.Friday)
            {
                NextBussinessDay = NextBussinessDay.AddDays(3);
            }
            else if (NextBussinessDay.DayOfWeek == DayOfWeek.Saturday)
            {
                NextBussinessDay = NextBussinessDay.AddDays(2);
            }
            else if (NextBussinessDay.DayOfWeek == DayOfWeek.Sunday)
            {
                NextBussinessDay = NextBussinessDay.AddDays(1);
            }

            while (IsFestivo(NextBussinessDay))
            {
                NextBussinessDay = NextBussinessDay.AddDays(1);
            }

            return datePadre.Date == NextBussinessDay.Date;
        }


        //****************************
        private bool IsFestivo(DateTime fecha)
        {
           var result = _Festivos.Any(f => f.Date ==  fecha.Date);

            return result;
        }
       
      
    }

   

}
