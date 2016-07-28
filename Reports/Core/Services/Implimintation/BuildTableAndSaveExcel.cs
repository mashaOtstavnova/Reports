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
    public class BuildTableAndSaveExcel : IBuildTableAndSaveExcel
    {
        private  DataTable _dateTable;
        private const string Format = "yyyy-MM-dd";
        public BuildTableAndSaveExcel()
        {
            _dateTable = new DataTable();
        }

        public DataTable BuiltTable(object[] obj)
        {
            Log.Inst.WriteToLogDEBUG(string.Format("Start build table for {0}", obj.GetType().FullName));
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

            return BuilTableForView(_dateTable);
        }

        private DataTable BuilTableForView(DataTable dt)
        {
            var resultTable = new DataTable();
            //DataTable dt1 = new DataTable();
            //DataTable dt2 = new DataTable();
            resultTable.Columns.Add("Дата/время");
            resultTable.Columns.Add("Группа карт");
            resultTable.Columns.Add("Код карты", dt.Columns["CardNumbers"].DataType);
            resultTable.Columns.Add("ФИО", typeof(string));
            resultTable.Columns.Add("Сумма", dt.Columns["TransactionSum"].DataType);
            resultTable.Columns.Add("Дотация", typeof(decimal));
            resultTable.Columns.Add("Кредит", typeof(decimal));
            resultTable.Columns.Add("Организаия", typeof(string));
            resultTable.Columns.Add("Операция", dt.Columns["TransactionType"].DataType);
          
           
           
            resultTable.Columns.Add("Дата");
          
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["TransactionType"].Equals("PayFromWallet"))
                {
                    var resultRow = resultTable.NewRow();
                    resultRow["Код карты"] = dt.Rows[i]["CardNumbers"];
                    resultRow["Группа карт"] = "";
                    resultRow["ФИО"] = "";
                    resultRow["Сумма"] = Math.Abs((decimal)dt.Rows[i]["TransactionSum"]);
                    resultRow["Операция"] = dt.Rows[i]["TransactionType"];
                    resultRow["Организаия"] = "";
                    //resultRow["TransactionCreateDate"] = dt.Rows[i]["TransactionCreateDate"];
                    DateTime tmp = (DateTime)dt.Rows[i]["TransactionCreateDate"];
                    resultRow["Дата/время"] = tmp.ToString();
                    resultRow["Дата"] = tmp.ToString(Format);
                    resultRow["Дотация"] = 250;
                    var credit = (decimal) resultRow["Дотация"] - Math.Abs((decimal) dt.Rows[i]["TransactionSum"]);
                    if (credit > 0)
                    {
                        resultRow["Кредит"] = 0;
                    }
                    else
                    {
                         resultRow["Кредит"] = Math.Abs(credit);
                    }
                   
                    resultTable.Rows.Add(resultRow);
                }

            }

            return resultTable;
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