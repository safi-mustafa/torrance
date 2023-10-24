using ClosedXML.Excel;
using System.Data;
using System.Reflection;

namespace ExcelReader
{
    public static class ExcelHelper
    {
        public static List<DataTable> GetDataFromExcel(string filePath)
        {
            try
            {
                var dtList = new List<DataTable>();
                if (System.IO.File.Exists(filePath))
                {
                    //Started reading the Excel file.
                    using (XLWorkbook workbook = new XLWorkbook(filePath))
                    {
                        foreach (var worksheet in workbook.Worksheets)
                        {
                            bool FirstRow = true;
                            //Range for reading the cells based on the last cell used.
                            var dt = new DataTable();
                            string readRange = "1:1";
                            foreach (IXLRow row in worksheet.RowsUsed())
                            {
                                //If Reading the First Row (used) then add them as column name
                                if (FirstRow)
                                {
                                    //Checking the Last cellused for column generation in datatable
                                    readRange = string.Format("{0}:{1}", 1, row.LastCellUsed().Address.ColumnNumber);
                                    foreach (IXLCell cell in row.Cells(readRange))
                                    {
                                        dt.Columns.Add(cell.Value.ToString());
                                    }
                                    FirstRow = false;
                                }
                                else
                                {
                                    //Adding a Row in datatable
                                    dt.Rows.Add();
                                    int cellIndex = 0;
                                    //Updating the values of datatable
                                    foreach (IXLCell cell in row.Cells(readRange))
                                    {
                                        dt.Rows[dt.Rows.Count - 1][cellIndex] = cell.Value.ToString();
                                        cellIndex++;
                                    }
                                }
                            }
                            dtList.Add(dt);
                        }
                        return dtList;
                    }
                }
                return new List<DataTable>();
            }
            catch (Exception ex)
            {
                return new List<DataTable>();
            }
        }

        public static bool AddModel<T>(List<DataTable> dataLists, int ind, int startInd, List<T> itemList) where T : new()
        {
            try
            {
                for (int i = startInd; i < dataLists[ind].Rows.Count; i++)
                {
                    var item = new T();
                    for (int j = 0; j < dataLists[ind].Columns.Count; j++)
                    {
                        var columnName = dataLists[ind].Columns[j].ColumnName.ToString();
                        if (!string.IsNullOrEmpty(columnName))
                        {
                            SetObjectProperty(item, columnName, (dataLists[ind].Rows[i][j]).ToString());
                        }
                    }
                    itemList.Add(item);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        static void SetObjectProperty(object myObject, string propertyName, string value)
        {

            try
            {
                foreach (PropertyInfo pi in myObject.GetType().GetProperties())
                {
                    if (pi.Name == propertyName.Trim())
                    {
                        //casted the property to its actual type dynamically
                        var type = pi.PropertyType;
                        if (type == typeof(float))
                            if (value != "")
                                pi.SetValue(myObject, float.Parse(value));
                        if (type == typeof(string))
                            pi.SetValue(myObject, value);
                        if (type == typeof(double))
                            if (value != "")
                                pi.SetValue(myObject, double.Parse(value));
                        if (type == typeof(int))
                            if (value != "")
                                pi.SetValue(myObject, int.Parse(value));
                        if (type == typeof(Int64))
                            if (value != "")
                                pi.SetValue(myObject, long.Parse(value));
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }
    }
}