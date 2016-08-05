using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Core.Domain;
using ExcelLibrary.SpreadSheet;
using Framework;
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
        private List<BalancCardNumber> _allBalans; 

        public BuildTableAndSaveExcel()
        {
            _dateTable = new DataTable();
            _allBalans = new List<BalancCardNumber>(); 
        }
        /// <summary>
        /// конструирование DataTable по любому списку объектов
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
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
        /// <summary>
        /// сохранение DataTable в excel
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="path"></param>
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
        /// <summary>
        /// конструирование DataTable для представления на экран
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private DataTable BuilTableForView(DataTable dt)
        {
            var config = CoreContext.ConfigService.GetConfig();
            var resultTable = new DataTable("Balance");
            resultTable.Columns.Add("Дата/время", typeof (DateTime));
            resultTable.Columns.Add("Дата", typeof (DateTime));
            resultTable.Columns.Add("Группа карт");
            resultTable.Columns.Add("Код карты", dt.Columns["CardNumbers"].DataType);
            resultTable.Columns.Add("ФИО", typeof (string));
            resultTable.Columns.Add("Сумма чека", dt.Columns["TransactionSum"].DataType);
            resultTable.Columns.Add("Дотация", typeof (decimal));
            resultTable.Columns.Add("Кредит", typeof (decimal));
            resultTable.Columns.Add("Организация", typeof (string));
            resultTable.Columns.Add("Телефон", dt.Columns["PhoneNumber"].DataType);
            resultTable.Columns.Add("Операция", dt.Columns["TransactionType"].DataType);
            //resultTable.Columns.Add("Лимит/организация", typeof(string));
            
            var allCard = dt.AsEnumerable().GroupBy(r => r["CardNumbers"]).ToList().Select(r => r.Key);
            _allBalans = allCard.Select(i => new BalancCardNumber(i.ToString(), 250)).ToList();
            
          
            foreach (var i in allCard)
            {
                var t =
                    dt.AsEnumerable()
                        .Where(r => r["CardNumbers"].Equals(i))
                        .Where(r => r["TransactionType"].Equals("PayFromWallet"));
                var t1v = t.ToList().Select(r => r["TransactionCreateDate"]).ToList();foreach (var item in t)
                {
                    var resultRow = resultTable.NewRow();
                    resultRow["Код карты"] = item["CardNumbers"];
                    var tmpDate = (DateTime) item["TransactionCreateDate"];
                    resultRow["Дата/время"] = tmpDate.ToString("G");
                    resultRow["Дата"] = tmpDate.ToString(Format);
                    resultRow["Сумма чека"] = (decimal) item["TransactionSum"];
                    resultRow["Телефон"] = item["PhoneNumber"];
                    resultRow["Группа карт"] = "";
                    resultRow["ФИО"] = "";
                    resultRow["Организация"] = "";
                    resultRow["Дотация"] = 0;
                    resultRow["Кредит"] = 0;
                    //try
                    //{

                    //    resultRow["Лимит/организация"] = CoreContext.MakerRequest.GetInfoByCard(item["CardNumbers"].ToString(), "7969889b-eaa0-11e5-80d8-d8d38565926f").Result.Categories[2].Name;

                    //}
                    //catch (Exception ex)
                    //{

                    //    resultRow["Лимит/организация"] = ex.Message;
                    //}
                    resultTable.Rows.Add(resultRow);
                }
            }
            CalculateBalance(ref resultTable);
            return resultTable;
        }
        /// <summary>
        /// расчет дотации и кредита для таблицы представления
        /// </summary>
        /// <param name="inputTable"></param>
        private void CalculateBalance(ref DataTable inputTable)
        {
            var listBalans = CopyListBalance(_allBalans);
            var tranzactionGroupByCard = inputTable.AsEnumerable().GroupBy(r => r["Код карты"]);
            foreach (var i in tranzactionGroupByCard) //идем по картам
            {
                var d = i.Cast<DataRow>().OrderBy(row => row["Дата/время"]).Reverse().ToList(); //все операции по этой карте
                var d1 = d.GroupBy(row => row["Дата"]).Reverse().ToList().Select(k => k.Key); //сгруппируем операции по дням
                foreach (var j in d1)//идем по дням
                {

                    var thisDayTranz = d.Where(y => y["Дата"].Equals(j)).ToList();//берем операции за нужный(текущий в цикле) день
                    var thisDayTranzSort = thisDayTranz.OrderBy(x => ((DateTime)x["Дата/время"]).TimeOfDay).ToList();// и сортируем по возрастанию
                    foreach (var item in thisDayTranzSort)//идем по операцияем за день
                    {
                        var oldBalance =
                            listBalans.Where(r => r.NumberCard.Equals(item["Код карты"]))
                                .Select(r => r.Sum)
                                .FirstOrDefault();
                        var sum = Math.Abs((decimal)item["Сумма чека"]);
                        var balance = oldBalance - sum;
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
                    listBalans = CopyListBalance(_allBalans); //делаем снова полный лимит
                }
            }
        }
        /// <summary>
        /// копирование списка дотации по значению
        /// </summary>
        /// <param name="oldList"></param>
        /// <returns></returns>
        private List<BalancCardNumber> CopyListBalance(List<BalancCardNumber> oldList)
        {
            var listBalans= new List<BalancCardNumber>();
            _allBalans.ForEach((item) =>
            {
                listBalans.Add(new BalancCardNumber(item.NumberCard, item.Sum));
            });
            return listBalans;
        }
    }
   
}