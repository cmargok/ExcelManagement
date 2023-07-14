using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cmargok.ExcelDataReader.ImportExcel.ExcelCore.Import
{
    public class Import : ExcelCore, IImport
    {


        public List<T> ImportToListOf<T>(IFormFile Excelfile, ImportConfiguration mapInfo) where T : new() => GetList<T>(Excelfile, mapInfo);

        public IEnumerable<T> ImportToIEnumarableOf<T>(IFormFile Excelfile, ImportConfiguration mapInfo) where T : new() => GetList<T>(Excelfile, mapInfo);

        public async Task<IEnumerable<T>> ImportToIEnumarableOfAsync<T>(IFormFile Excelfile, ImportConfiguration mapInfo) where T : new() => await GetListTask<T>(Excelfile, mapInfo);

        public List<Target> ImportToListOfwithListOfStrings<Target>(DataSet dataSet, int Table, int ExcelrowInit) where Target : new()
        {
            Target entity = new();

            var entityProperties = GetTypeProperties(entity);

            List<Target> ListEntities = new();

            while (ExcelrowInit < dataSet.Tables[Table].Rows.Count)
            {
                entityProperties[0].SetValue(entity, ParseDataType(entityProperties[0], dataSet.Tables[Table].Rows[ExcelrowInit][0].ToString()!));

                List<string> listaString = new();

                for (int j = 1; j < dataSet.Tables[Table].Columns.Count; j++)
                {
                    string temp = dataSet.Tables[Table].Rows[ExcelrowInit][j].ToString()!;

                    if (temp.Length > 0) listaString.Add(temp);

                    else break;
                }
                entityProperties[1].SetValue(entity, listaString);

                ListEntities.Add(entity);
                entity = new Target();
                ExcelrowInit++;
            }
            return ListEntities;
        }

        public List<Target> ImportToListOf<Target>(DataSet dataSet, int Table, ImportConfiguration mapInfo) where Target : new()
        {
            Target entity = new();
            var entityProperties = GetTypeProperties(entity);
            int columns = 0;
            List<Target> listEntities = new();

            while (mapInfo.RowInit < dataSet.Tables[Table].Rows.Count)
            {
                for (int i = mapInfo.ObjectPropertyInit; i < entityProperties.Length - mapInfo.ObjectPropertyEnd; i++)
                {
                    var field = entityProperties[i];
                    field.SetValue(entity, ParseDataType(field, dataSet.Tables[Table].Rows[mapInfo.RowInit][columns].ToString()!));
                    columns++;
                    if (columns == mapInfo.ColumnsCount) columns = 0;
                }

                listEntities.Add(entity);
                entity = new Target();
                mapInfo.RowInit++;
            }
            return listEntities;

        }






























        public DataSet GetDataSet(IFormFile file)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            IExcelDataReader reader = null!;

            DataSet dataSet = new();

            using (var stream = file.OpenReadStream())
            {

                if (file.FileName.EndsWith(".xls")) reader = ExcelReaderFactory.CreateBinaryReader(stream);

                else if (file.FileName.EndsWith(".xlsx")) reader = ExcelReaderFactory.CreateOpenXmlReader(stream);

                if (reader == null) return null!;

                dataSet = reader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = false
                    }
                });
            }
            return dataSet;
        }

        protected Task<List<T>> GetListTask<T>(IFormFile Excelfile, ImportConfiguration mapInfo) where T : new()
        {
            DataSet dataSet = GetDataSet(Excelfile);
            T entity = new();
            var entityProperties = GetTypeProperties(entity);
            int columns = 0;

            List<T> listEntities = new();

            while (mapInfo.RowInit < dataSet.Tables[0].Rows.Count)
            {
                for (int i = mapInfo.ObjectPropertyInit; i < entityProperties.Length - mapInfo.ObjectPropertyEnd; i++)
                {
                    var field = entityProperties[i];
                    field.SetValue(entity, ParseDataType(field, dataSet.Tables[0].Rows[mapInfo.RowInit][columns].ToString()!));
                    columns++;
                    if (columns == mapInfo.ColumnsCount) columns = 0;
                }

                listEntities.Add(entity);
                entity = new T();
                mapInfo.RowInit++;
            }
            return Task.FromResult(listEntities);
        }

        protected List<T> GetList<T>(IFormFile Excelfile, ImportConfiguration mapInfo) where T : new()
        {
            DataSet dataSet = GetDataSet(Excelfile);
            T entity = new();
            var entityProperties = GetTypeProperties(entity);
            int columns = 0;
            List<T> ListEntities = new();

            while (mapInfo.RowInit < dataSet.Tables[0].Rows.Count)
            {
                for (int i = mapInfo.ObjectPropertyInit; i < entityProperties.Length - mapInfo.ObjectPropertyEnd; i++)
                {
                    var field = entityProperties[i];
                    field.SetValue(entity, ParseDataType(field, dataSet.Tables[0].Rows[mapInfo.RowInit][columns].ToString()!));
                    columns++;
                    if (columns == mapInfo.ColumnsCount) columns = 0;

                }
                ListEntities.Add(entity);
                entity = new T();
                mapInfo.RowInit++;
            }
            return ListEntities;
        }
    }
}
