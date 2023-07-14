using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cmargok.ExcelDataReader.ImportExcel.ExcelCore.Import
{
    public interface IImport
    {
        public List<T> ImportToListOf<T>(IFormFile Excelfile, ImportConfiguration mapInfo) where T : new();

        public IEnumerable<T> ImportToIEnumarableOf<T>(IFormFile Excelfile, ImportConfiguration mapInfo) where T : new();

        public Task<IEnumerable<T>> ImportToIEnumarableOfAsync<T>(IFormFile Excelfile, ImportConfiguration mapInfo) where T : new();

        public List<Target> ImportToListOf<Target>(DataSet dataSet, int Table, ImportConfiguration mapInfo) where Target : new();

        public List<Target> ImportToListOfwithListOfStrings<Target>(DataSet dataSet, int Table, int ExcelrowInit) where Target : new();








        public DataSet GetDataSet(IFormFile file);
    }
}
