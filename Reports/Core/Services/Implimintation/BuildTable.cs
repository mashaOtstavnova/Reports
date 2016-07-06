using System;
using System.Data;
using System.IO;
using System.Linq;
using ExcelLibrary.SpreadSheet;
using Reports;

namespace Core.Services.Implimintation
{
    /// <summary>
    /// конструирования dateTable по object
    /// </summary>
    public class BuildTable : IBuildTable
    {
        private  DataTable _dateTable;

        public BuildTable()
        {
            _dateTable = new DataTable();
        }

        public DataTable BuiltTable(object[] obj)
        {
            Log.Inst.WriteToLogDEBUG(string.Format("Start buil table for {0}", obj.GetType().FullName));
            _dateTable = new DataTable();
            _dateTable.TableName = obj.GetType().GetElementType().Name;
           
            for (var i = 0; i < obj.GetType().GetElementType().GetProperties().Count(); i++)
            {
                var c1 = obj.GetType().GetElementType().GetProperties()[i];
                _dateTable.Columns.Add(c1.Name, c1.PropertyType);
            }
            foreach (var item in obj)
            {
                var r = _dateTable.NewRow();
                for (var i = 0; i < obj.GetType().GetElementType().GetProperties().Count(); i++)
                {
                    var c1 = obj.GetType().GetElementType().GetProperties()[i];
                    r[c1.Name] = item.GetType().GetProperty(c1.Name).GetValue(item, null);
                }
                _dateTable.Rows.Add(r);
            }
            Log.Inst.WriteToLogDEBUG(string.Format("End buil table for {0}", obj.GetType().FullName));
            return _dateTable;
        }

        public void SaveExel(DataTable dt, string path)
        {
            Workbook workbook = new Workbook();
          
            Worksheet worksheet = new Worksheet(dt.TableName);
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                // Add column header
                worksheet.Cells[0, i] = new Cell(dt.Columns[i].ColumnName);

                // Populate row data
                for (int j = 0; j < dt.Rows.Count; j++) { 
                    //Если нулевые значения, заменяем на пустые строки
                    var valueRow = dt.Rows[j][i];
                    if (valueRow.GetType() == typeof (DateTime))
                    {
                        valueRow = valueRow.ToString();
                    }
                    worksheet.Cells[j + 1, i] = new Cell(valueRow == DBNull.Value ? "" : valueRow);
                }
            }
            workbook.Worksheets.Add(worksheet);
            var p = Path.Combine(Environment.CurrentDirectory, @"exels");
            workbook.Save(path);
        }
    }
}