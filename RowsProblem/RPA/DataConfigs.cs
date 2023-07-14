using System.Data;

namespace RowsProblem.RPA
{
    public class DataConfigs
    {
        public DataTable FirstTable { get; set; }
        public DataTable SecondTable { get; set; }
        public DataTable ThirdTable { get; set; }
        public DataTable FourthTable { get; set; }
        public List<int> Indexes { get; set; }
        public List<int[]> Columnas { get; set; }
        public List<int> FatherIndexes { get; set; }
        public int TablePadreColumnsCount { get; set; }

        public DataConfigs(DataTable firstTable, DataTable secondTable, DataTable thirdTable, DataTable fourthTable)
        {
            FirstTable = firstTable;
            SecondTable = secondTable;
            ThirdTable = thirdTable;
            FourthTable = fourthTable;
        }
    }

}
