using System;
using System.Data;
using System.IO;
using System.Linq;
using ExcelLibrary.SpreadSheet;
using Reports;

namespace Core.Services.Implimintation
{
    /// <summary>
    /// конструирование dateTable по object
    /// </summary>
    public class BuildTableAndSaveExcelAndSaveExcel : IBuildTableAndSaveExcel
    {
        private  DataTable _dateTable;

        public BuildTableAndSaveExcelAndSaveExcel()
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
            Log.Inst.WriteToLogDEBUG(string.Format("Start save in excel file in path {0}", path));
            Workbook workbook = new Workbook();
            _dateTable = dt;
            Worksheet worksheet = new Worksheet(_dateTable.TableName);
            for (int i = 0; i < _dateTable.Columns.Count; i++)
            {
                // Add column header
                worksheet.Cells[0, i] = new Cell(_dateTable.Columns[i].ColumnName);

                // Populate row data
                for (int j = 0; j < _dateTable.Rows.Count; j++) { 
                    //Если нулевые значения, заменяем на пустые строки
                    var valueRow = _dateTable.Rows[j][i];
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
            Log.Inst.WriteToLogDEBUG(string.Format("End save in excel file in path {0}", path));
            CoreContext.ViewService.FirstView.ShowMessag(string.Format("Файл {0} успешно сохранен.", path));
        }
    }
}