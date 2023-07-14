using System.Data;

namespace RowsProblem.RPA
{
    public class DatatableManager
    {
        public void GuardarEnArchivo(string variable1, int variable2, string variable3)
        {
            string contenido = $"{variable1},{variable2},{variable3}";
            File.WriteAllText(@"C:\logtemp\TemporalT.txt", contenido);
        }

        public DataTable MergeIntoOneDt(DataTable dt1, DataTable dt2)
        {
            DataTable dtMerge = new DataTable();

            // Añadimos las columnas de la primera tabla
            foreach (DataColumn column in dt1.Columns)
            {
                dtMerge.Columns.Add(column.ColumnName, column.DataType);
            }

            // Añadimos las columnas de la segunda tabla (si no existen ya en la primera)
            foreach (DataColumn column in dt2.Columns)
            {
                if (!dtMerge.Columns.Contains(column.ColumnName))
                {
                    dtMerge.Columns.Add(column.ColumnName, column.DataType);
                }
            }

            // Recorremos las filas de la primera tabla y las añadimos a la nueva tabla
            foreach (DataRow row in dt1.Rows)
            {

                dtMerge.ImportRow(row);
              
            }
            dtMerge.AcceptChanges();

            // Recorremos las filas de la segunda tabla y las añadimos a la nueva tabla
            foreach (DataRow row in dt2.Rows)
            {
                dtMerge.ImportRow(row);
            }
            dtMerge.AcceptChanges();
            return dtMerge;
        }



        private bool GetOperadorBy(string a, int edad, string operadorStr)
        {
            if(a == null || edad == 0 || operadorStr == null) return false;

            int aValue;
            int bValue = edad;
            try
            {
                if (!int.TryParse(a, out aValue))
                {
                    return false; 
                }
            }
            catch
            {
                return false;
            }         

            if (operadorStr == "<")
            {
                return aValue < bValue;

            }
            else if (operadorStr == ">")
            {
                return aValue > bValue;

            }
            else if (operadorStr == "==")
            {
                return a == edad.ToString();
            }
            else if (operadorStr == ">=")
            {
                return aValue >= bValue;

            }
            else if (operadorStr == "<=")
            {
                return aValue <= bValue;
            }
            else
            {
                throw new ArgumentException("Operador desconocido: " + operadorStr);
            }
        }





        public DataTable Rule11(DataTable table, int index, string filterValue, string operadorStr, string Edad, int indexEdad, List<string> segmento, int indexSegmento)
        {
            var registers = table.AsEnumerable(); //convertimos a lista "lista principal"

            var filteredRows = registers.Where(c => c[index].ToString().Contains(filterValue)); // extraemos las filas que tiene el filtro "filtervalue"

            IEnumerable<DataRow> remainsRows = new List<DataRow>();

            if (filteredRows.Any()) //verficamos si hay filas que cumplan el filtro
            {
                remainsRows = registers.Except(filteredRows); // restamos de la lista principal las filas que coinciden con el filtro y las metemos en una lista resultante


                //aqui verificamos  que debe cumplir con el segmento y que en la columna edad tenga algo difente a No aplica
                filteredRows = filteredRows.Where(c => segmento.Contains(c[indexSegmento].ToString()) && !c[indexEdad].ToString().Equals("No aplica", StringComparison.OrdinalIgnoreCase));


                int edadInt = int.Parse(Edad); //convertimos la edad para comparar a entero


                //usamos el metodo GetOperadorBy para comparar las edades y el tipo de comparacion dato por
                //la variable operadorStr "<,>,==,<= o >=", para verificar si cumple o no la condicion
                filteredRows = filteredRows.Where(c => GetOperadorBy(c[indexEdad].ToString(), edadInt, operadorStr));
            }
            else
            {
                return table; // si el filtro no encontro nada devolvemos el table de entrada sin cambios.
            }


            DataTable moveOut = table.Clone(); //creamos la tabla de salida con unacopia en estructura de la tabla que entra

            moveOut.BeginLoadData(); // ponemos la tabla en modo copia"performance"

            if (remainsRows.Any())  //si hay filas resultantes las copiamos las filas de la lista resultante a la tabla que creamos arriba 
            {
                moveOut = remainsRows.CopyToDataTable(); //las copiamos
            }


            foreach (DataRow fila in filteredRows) // hacemos un ciclo para tener acceso a cada fila de la lista de las filas filtradas y que aprobaron todas las condicionales
            {
                moveOut.ImportRow(fila); //copiamos la fila a la tabla de salida
            }
            moveOut.AcceptChanges(); // aceptamos cambios en la table de salida

            moveOut.EndLoadData(); //cerramos la copia de datos


            return moveOut; // retornamos la tabla de salida

        }





        public DataTable SplitDatatableByParams(DataTable table, int FechaColumn, int NombreColumn, int DocumentoColumn, char splitCharacter, string NombreColumnaHora)
        {
            int rowCounter = 0;
            table.Columns.Add(NombreColumnaHora);
            DataTable AnewTable = table.Clone();
            int ColumnsCount = AnewTable.Columns.Count - 1;

            //table.Rows[rowCounter][ColumnsCount] = NombreColumnaHora;

            //AnewTable.ImportRow(table.Rows[rowCounter]);

           // rowCounter++;

            while (rowCounter < table.Rows.Count)
            {

                string[] values = SplittedValues(table, rowCounter, FechaColumn, splitCharacter);

                if (values != null && values.Length > 1)
                {
                    for (int f = 0; f < values.Length; f++)
                    {
                        string[] dateTime = SplitAndFormatDateby(values[f]);
                        SetFechaAndHour(table, rowCounter, FechaColumn, ColumnsCount, dateTime);
                        AnewTable.ImportRow(table.Rows[rowCounter]);
                    }
                }
                else if (values == null)
                {
                    values = SplittedValues(table, rowCounter, NombreColumn, splitCharacter);
                    if (values != null)
                    {
                        string[] LinkedValues = SplittedValues(table, rowCounter, DocumentoColumn, splitCharacter);

                        string[] dateTime = SplitAndFormatDateby(table.Rows[rowCounter][FechaColumn].ToString());

                        SetFechaAndHour(table, rowCounter, FechaColumn, ColumnsCount, dateTime);

                        if (LinkedValues.Length == values.Length)
                        {

                            for (int f = 0; f < values.Length; f++)
                            {
                                table.Rows[rowCounter][NombreColumn] = values[f];
                                table.Rows[rowCounter][DocumentoColumn] = LinkedValues[f];
                                AnewTable.ImportRow(table.Rows[rowCounter]);
                            }
                        }
                    }
                    else
                    {
                        string[] dateTime = SplitAndFormatDateby(table.Rows[rowCounter][FechaColumn].ToString());
                        SetFechaAndHour(table, rowCounter, FechaColumn, ColumnsCount, dateTime);
                        AnewTable.ImportRow(table.Rows[rowCounter]);
                    }
                }
                else
                {
                    string[] dateTime = SplitAndFormatDateby(table.Rows[rowCounter][FechaColumn].ToString());
                    SetFechaAndHour(table, rowCounter, FechaColumn, ColumnsCount, dateTime);
                    AnewTable.ImportRow(table.Rows[rowCounter]);
                }
                rowCounter++;
                AnewTable.AcceptChanges();
            }
            return AnewTable;
        }




        public DataTable TakeDataFromChildrenTables(DataTable FatherTable, DataConfigs data)
        {
            var SetTables = MergeTablesIntoList(data.FirstTable, data.SecondTable, data.ThirdTable, data.FourthTable);

            FatherTable.BeginLoadData();
            List<string> valuesTo = new List<string>();

            foreach (var row in YieldingRows(FatherTable))
            {
                int ColumsCounter = data.TablePadreColumnsCount;

                AddValuesTo(row, valuesTo, data);

                for (int i = 0; i < SetTables.Count; i++)
                {
                    if (String.IsNullOrEmpty(valuesTo[i]))
                    {
                        AddValuesTo(row, valuesTo, data);
                    }

                    MoveDataFrom_ToFather(row, ColumsCounter, valuesTo[i], SetTables[i], data.Indexes[i], data.Columnas[i]);
                   
                    ColumsCounter += data.Columnas[i].Length;
                }
              
                FatherTable.AcceptChanges();
            }
            FatherTable.EndLoadData();
            return FatherTable;
        }

        private void AddValuesTo(DataRow row,List<string> valuesTo, DataConfigs data)
        {
            valuesTo.Clear();
            for (int k = 0; k < data.FatherIndexes.Count; k++)
            {
                valuesTo.Add(row[data.FatherIndexes[k]].ToString());
            }

        }



        public List<int> MergeIndex(params int[] indexes)
        {
            return indexes.ToList();
        }

        public List<int[]> MergeColumnas(params string[] Columns)
        {
            var IntValues = new List<int[]>();
            foreach (var value in Columns)
            {
                IntValues.Add(ParseToInt(value.Split(',')));
            }
            return IntValues;
        }

        public void SetColumnsName(DataTable table, int TablePadreColumnsCount, params List<string>[] names)
        {
            /*
              for (int i = 0; i < TablePadreColumnsCount; i++)
                 {
                     if (!string.IsNullOrEmpty(table.Rows[0][i].ToString()))
                     {
                         table.Columns[i].ColumnName = table.Rows[0][i].ToString();
                         continue;
                     }
                     table.Columns[i].ColumnName = "Column " + i;
             }*/
            int privateCounter = TablePadreColumnsCount;

            for (int i = 0; i < names.Length; i++)
            {
                foreach (string name in names[i])
                {
                    table.Columns.Add(name);
                    //table.Rows[0][privateCounter] = name;
                    privateCounter++;
                }
            }
        }

        public void DeleteDuplicateRow(DataTable table, int RowIndex)
        {
            table.Rows[RowIndex].Delete();
            table.AcceptChanges();
        }

        private void MoveDataFrom_ToFather(DataRow Row, int TablePadreColumnsCount, string filterValue, DataTable TargetTable, int index, int[] Columns)
        {
            IEnumerable<DataRow> tablaFiltradag;
            if (!string.IsNullOrEmpty(filterValue))
            {
                tablaFiltradag = TargetTable.AsEnumerable().Where(c => c[index].ToString() == filterValue);

            }
            else
            {
                tablaFiltradag = null;
            }

            if (tablaFiltradag != null && tablaFiltradag.Count() > 0)
            {
                var reg = tablaFiltradag.ElementAt(0);
                for (int i = 0; i < Columns.Length; i++)
                {
                    Row[TablePadreColumnsCount + i] = reg[Columns[i]];
                }
            }
            else
            {
                for (int i = 0; i < Columns.Length; i++)
                {
                    var oldvalue = Row[TablePadreColumnsCount + i];
                    var newValue = Convert.ChangeType(oldvalue, Type.GetType("System.String"));
                    Row[TablePadreColumnsCount + i] = "No aplica";
                }
            }


        }

        //dynamic function
        private IEnumerable<DataRow> YieldingRows(DataTable FatherTable)
        {
            //  bool FirstRow = true;
            foreach (DataRow row in FatherTable.Rows)
            {
                /* if (FirstRow)
                 {
                     FirstRow = false;
                     continue;
                 }*/
                yield return row;
            }
        }

        //dynamic function
        private static List<DataTable> MergeTablesIntoList(params DataTable[] tables)
        {
            List<DataTable> newTable = new List<DataTable>();
            for (int i = 0; i < tables.Length; i++)
            {
                newTable.Add(tables[i].Copy());
            }
            return newTable;
        }

        private static int[] ParseToInt(string[] array)
        {
            int[] ints = new int[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                ints[i] = int.Parse(array[i]);
            }
            return ints;
        }


        //-----------separando los valores repetidos
        private string[] SplittedValues(DataTable table, int Row, int Index, char splitCharacter)
        {
            bool IsSplit;
            IsSplit = table.Rows[Row][Index].ToString().Contains(splitCharacter);
            if (IsSplit)
            {
                string[] values = table.Rows[Row][Index].ToString().Split(splitCharacter);
                return values;
            }
            return null;
        }

        //----------manejando fecha y hora ------------
        private string[] SplitAndFormatDateby(string value)
        {
            string[] SplittedValues = new string[2];
            bool IsSpace = value.Contains(' ');
            if (IsSpace)
            {
                SplittedValues = value.Split(' ');
            }
            else
            {
                string date = value.Substring(0, 10);
                string time = value.Substring(11, 8);
                string[] StringReverses = date.Split('-');
                SplittedValues[0] = StringReverses[2] + '/' + StringReverses[1] + '/' + StringReverses[0];
                SplittedValues[1] = time;
            }
            return SplittedValues;
        }

        private void SetFechaAndHour(DataTable table, int rowCounter, int FechaColumn, int LastColumnToHour, string[] dateTime)
        {
            table.Rows[rowCounter][FechaColumn] = dateTime[0];
            table.Rows[rowCounter][LastColumnToHour] = dateTime[1];
        }

    }

//}
//private Func<string, string, bool> GetOperador(string operadorStr)
//{
//    int aValue = 0;
//    int bValue = 0;

//    if (operadorStr == "<")
//    {
//        return (a, b) =>
//        {
//            if (int.TryParse(a, out int aValue))
//            {
//                if (int.TryParse(b, out int bValue))
//                {
//                    return aValue < bValue;
//                }
//            }
//            return false;
//        };
//    }
//    else if (operadorStr == ">")
//    {
//        return (a, b) =>
//        {
//            if (int.TryParse(a, out int aValue))
//            {
//                if (int.TryParse(b, out int bValue))
//                {
//                    return aValue > bValue;
//                }
//            }
//            return false;
//        };
//    }
//    else if (operadorStr == "==")
//    {
//        return (a, b) => a == b;
//    }
//    else if (operadorStr == ">=")
//    {
//        return (a, b) =>
//        {
//            if (int.TryParse(a, out int aValue))
//            {
//                if (int.TryParse(b, out int bValue))
//                {
//                    return aValue >= bValue;
//                }
//            }
//            return false;
//        };
//    }
//    else if (operadorStr == "<=")
//    {
//        return (a, b) =>
//        {
//            if (int.TryParse(a, out int aValue))
//            {
//                if (int.TryParse(b, out int bValue))
//                {
//                    return aValue <= bValue;
//                }
//            }
//            return false;
//        };
//    }
//    else
//    {
//        throw new ArgumentException("Operador desconocido: " + operadorStr);
//    }
}