
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace SDS.Wapi.commons.utils.ImportExportExcel.ExcelCore
{

    public class ExCore : IExCore
    {
        public bool VerifyFileExtension(IFormFile file) => file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx");


        public IXLWorkbook CreateWorkBook() => new XLWorkbook();   

        protected PropertyInfo[] GetTypeProperties<T>(T entity)
        {
            if(entity != null) return entity.GetType().GetProperties();
            throw new ArgumentNullException("null EXCEL HELPER ERROR");
        }

        protected dynamic ParseDataType(PropertyInfo propertyInfo, string value)
        {
            var type = propertyInfo.PropertyType.ToString();
            if (value == "" || value == string.Empty || value == null) return null!;

            return type switch
            {
                "System.Byte" => byte.Parse(value),
                "System.Int32" => int.Parse(value),
                "System.Boolean" => bool.Parse(value),
                "System.Int16" => short.Parse(value),
                "System.Int64" => long.Parse(value),
                "System.Single" => float.Parse(value),
                "System.Double" => double.Parse(value),
                "System.Decimal" => decimal.Parse(value),
                "System.DateTime" => Convert.ToDateTime(value),
                "System.String" => value,
                _ => null!,
            };
        }
        protected dynamic ParseDataType(PropertyInfo propertyInfo, IXLCell value)
        {
            var type = propertyInfo.PropertyType.ToString();


            if (value == null) return null!;
            return type switch
            {
                "System.Byte" => value.GetValue<byte>(),
                "System.Int32" => value.GetValue<int>(),
                "System.Boolean" => value.GetBoolean(),
                "System.Int16" => value.GetValue<short>(),
                "System.Int64" => value.GetValue<long>(),
                "System.Single" => value.GetValue<float>(),
                "System.Double" => value.GetDouble(),
                "System.Decimal" => value.GetValue<decimal>(),
                "System.DateTime" => value.GetDateTime(),
                "System.String" => value.GetString(),
                _ => null!,
            };
        }


        public IXLWorkbook CreateWorkBook(IFormFile file)
        {
            IXLWorkbook wb;
            using (Stream stream = file.OpenReadStream())
            {
                wb = new XLWorkbook(stream);
            }
            return wb;
        }
    }


}


//switch (type)
//{
//    case "System.Byte":
//        return byte.Parse(value);

//    case "System.Int32":
//        return int.Parse(value);

//    case "System.Boolean":
//        return bool.Parse(value);

//    case "System.Int16":
//        return short.Parse(value);

//    case "System.Int64":
//        return long.Parse(value);

//    case "System.Single":
//        return float.Parse(value);

//    case "System.Double":
//        return double.Parse(value);

//    case "System.Decimal":
//        return decimal.Parse(value);

//    case "System.DateTime":
//        return Convert.ToDateTime(value);

//    case "System.String":
//        return value;

//    default:
//        return null!;
//}