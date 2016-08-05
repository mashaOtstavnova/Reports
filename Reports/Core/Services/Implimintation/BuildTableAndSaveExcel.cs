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
        private ReportParameters _reportParameters;
        private List<BalancCardNumber> _allBalans;
        private List<ParametrsForTable> _otherParam;

        public BuildTableAndSaveExcel()
        {
            _dateTable = new DataTable();
            _allBalans = new List<BalancCardNumber>();
            _reportParameters = CoreContext.ReportParametrsService.GetSettings();
            _otherParam = new List<ParametrsForTable>();
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
            var resultTable = new DataTable("Balance");
            resultTable.Columns.Add("Дата/время", typeof (DateTime));
            resultTable.Columns.Add("Дата", typeof (DateTime));
            resultTable.Columns.Add("Код карты", dt.Columns["CardNumbers"].DataType);
            resultTable.Columns.Add("ФИО", typeof (string));
            resultTable.Columns.Add("Категория", typeof (string));
            resultTable.Columns.Add("Сумма чека", dt.Columns["TransactionSum"].DataType);
            resultTable.Columns.Add("Дотация", typeof (decimal));
            resultTable.Columns.Add("Кредит", typeof (decimal));
            resultTable.Columns.Add("Телефон", dt.Columns["PhoneNumber"].DataType);

            var allCard = dt.AsEnumerable().GroupBy(r => r["CardNumbers"]).ToList().Select(r => r.Key.ToString());
            //_allBalans = allCard.Select(i => new BalancCardNumber(i.ToString(), 250)).ToList();
            _allBalans = GetCategoriesesBalance(allCard.ToList());


            foreach (var i in allCard)
            {
                var oParam = _otherParam.Where(x => x.NumberCard == i).FirstOrDefault();
                var t =
                    dt.AsEnumerable()
                        .Where(r => r["CardNumbers"].Equals(i))
                        .Where(r => r["TransactionType"].Equals("PayFromWallet"));
                foreach (var item in t)
                {
                    var resultRow = resultTable.NewRow();
                    resultRow["Код карты"] = item["CardNumbers"];
                    var tmpDate = (DateTime) item["TransactionCreateDate"];
                    resultRow["Дата/время"] = tmpDate.ToString("G");
                    resultRow["Дата"] = tmpDate.ToString(Format);
                    resultRow["Категория"] = oParam.Category;
                    resultRow["Сумма чека"] = (decimal) item["TransactionSum"];
                    resultRow["Телефон"] = item["PhoneNumber"];
                    resultRow["ФИО"] = oParam.FullName;
                    resultRow["Дотация"] = 0;
                    resultRow["Кредит"] = 0;
                    //var balancCardNumber = _allBalans.Where(tt=>tt.NumberCard.Equals(item["CardNumbers"])).FirstOrDefault();
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

        private List<BalancCardNumber> GetCategoriesesBalance(List<string> allCards)
        {
            var listBalance = new List<BalancCardNumber>();
            var balancCatigories = CoreContext.ConfigService.GetConfig().BalanceDictionary;
            foreach (var card in allCards)
            {
                var param = new ParametrsForTable();
                param.NumberCard = card;
                var response = CoreContext.MakerRequest.GetInfoByCard(card, _reportParameters.OrganizationInfoId).Result;
                param.FullName = response.Name +" "+response.Surname;
                var catigories = response.Categories;
                var trueCategories = new List<BalancCategories>();
                foreach (var item in catigories)
                {
                    var tmp = balancCatigories.Where(t => t.NameCategories == item.Name).FirstOrDefault();
                    if (tmp != null)
                    {
                        trueCategories.Add(tmp);
                    }
                    
                }

                if (trueCategories.Count != 0)
                {
                       var max = trueCategories.Max(categories => categories.Sum);
                       listBalance.Add(new BalancCardNumber(card,max));

                    param.Category = trueCategories.Where(t => t.Sum == max).FirstOrDefault().NameCategories;
                    
                }
                else
                {
                    listBalance.Add(new BalancCardNumber(card, 0));
                }
                _otherParam.Add(param);
             
            }
            return listBalance;

        } 
    }
   
}