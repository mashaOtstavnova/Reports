using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using ExcelLibrary.SpreadSheet;
using Reports;

namespace Core.Services.Implimintation
{
    /// <summary>
    ///     конструирование dateTable по object
    /// </summary>
    public class BuildTableAndSaveExcel : IBuildTableAndSaveExcel
    {
        private const string Format = "yyyy-MM-dd";
        private DataTable _dateTable;

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

        public void SaveExel(DataTable dt, string path)
        {
            Log.Inst.WriteToLogDEBUG(string.Format("Start save in excel file in path {0}", path));
            var workbook = new Workbook();
            _dateTable = dt;
            var worksheet = new Worksheet(_dateTable.TableName);
            for (var i = 0; i < _dateTable.Columns.Count; i++)
            {
                // Add column header
                worksheet.Cells[0, i] = new Cell(_dateTable.Columns[i].ColumnName);

                // Populate row data
                for (var j = 0; j < _dateTable.Rows.Count; j++)
                {
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

        private DataTable BuilTableForView(DataTable dt)
        {
            ////DataTable dt1 = new DataTable();
            ////DataTable dt2 = new DataTable();
           

            var resultTable = new DataTable("Balance");
            resultTable.Columns.Add("Дата/время", typeof(DateTime));
            resultTable.Columns.Add("Дата", typeof(DateTime));
            resultTable.Columns.Add("Группа карт");
            resultTable.Columns.Add("Код карты", dt.Columns["CardNumbers"].DataType);
            resultTable.Columns.Add("ФИО", typeof(string));
            resultTable.Columns.Add("Сумма чека", dt.Columns["TransactionSum"].DataType);
            resultTable.Columns.Add("Дотация", typeof(decimal));
            resultTable.Columns.Add("Кредит", typeof(decimal));
            resultTable.Columns.Add("Организация", typeof(string));
            resultTable.Columns.Add("Телефон", dt.Columns["PhoneNumber"].DataType);
            resultTable.Columns.Add("Операция", dt.Columns["TransactionType"].DataType);
            var tmp = dt.AsEnumerable().GroupBy(r => r["CardNumbers"]).ToList().Select(r => r.Key);
            var listBalans = tmp.Select(i => new Balanc(i.ToString(), 250)).ToList();
            var list = new List<Tuple<string, DateTime, decimal>>();
            foreach (var i in tmp)
            {
                var t = dt.AsEnumerable().Where(r => r["CardNumbers"].Equals(i)).Where(r => r["TransactionType"].Equals("PayFromWallet"));
                var t1v = t.ToList().Select(r => r["TransactionCreateDate"]).ToList();
                foreach (var item in t)
                {
                    var resultRow = resultTable.NewRow();
                    resultRow["Код карты"] = item["CardNumbers"];
                    var tmpDate = (DateTime)item["TransactionCreateDate"];
                    resultRow["Дата/время"] = tmpDate.ToString("G");
                    resultRow["Дата"] = tmpDate.ToString(Format);
                    resultRow["Сумма чека"] = (decimal)item["TransactionSum"];
                    resultRow["Телефон"] = item["PhoneNumber"];
                    resultRow["Группа карт"] = "";
                    resultRow["ФИО"] = "";
                    resultRow["Организация"] = "";
                    resultRow["Дотация"] = 0;
                    resultRow["Кредит"] = 0;
                    resultTable.Rows.Add(resultRow);

                }
            }
            var g = resultTable.AsEnumerable().GroupBy(r => r["Код карты"]);
            foreach (var i in g)
            {
                var d = i.Cast<DataRow>().OrderByDescending(row => row["Дата/время"]).Reverse();
                foreach (var item in d)
                {
                    var oldBalance = listBalans.Where(r => r.NumberCard.Equals(item["Код карты"])).Select(r => r.Sum).FirstOrDefault();
                    var sum = Math.Abs((decimal)item["Сумма чека"]);
                    var balance = (decimal)oldBalance - sum;
                    if (balance >= 0)
                    {
                        item["Дотация"] = sum;
                        item["Кредит"] = 0;
                        listBalans.FirstOrDefault(t2 => t2.NumberCard.Equals(item["Код карты"])).Sum = balance;
                    }
                    else
                    {
                        item["Дотация"] = oldBalance;
                        item["Кредит"] = sum - oldBalance;
                        listBalans.FirstOrDefault(t2 => t2.NumberCard.Equals(item["Код карты"])).Sum = 0;
                    }
                }
            }

            return resultTable;
        }
        
    }

    public class Balanc
    {
        public string NumberCard { get; set; }
        public decimal Sum { get; set; }

        public Balanc(string numberCard, decimal sum)
        {
            NumberCard = numberCard;
            Sum = sum;
        }
    }
}