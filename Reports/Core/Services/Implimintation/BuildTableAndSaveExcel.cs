using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Core.Domain;
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
        private readonly List<InfoCard> _infoCards;
        private readonly ReportParameters _reportParameters;
        private List<BalancCardNumber> _allBalans;
        private DataTable _dateTable;

        public BuildTableAndSaveExcel()
        {
            _dateTable = new DataTable();
            _allBalans = new List<BalancCardNumber>();
            _reportParameters = CoreContext.ReportParametrsService.GetSettings();
            _infoCards = new List<InfoCard>();
        }

        /// <summary>
        ///     конструирование DataTable по любому списку объектов
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
        ///     сохранение DataTable в excel
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="path"></param>
        public void SaveExel(DataTable dt, string path)
        {
            Log.Inst.WriteToLogDEBUG(string.Format("Start save in excel file in path {0}", path));
            var workbook = new Workbook();
            _dateTable = dt;
            var worksheet = new Worksheet(_dateTable.TableName);
            try
            {
                if (_dateTable.Rows.Count > 0)
                {  for (var i = 0; i < _dateTable.Columns.Count; i++)
                {
                    // Add column header
                    worksheet.Cells[0, i] = new Cell(_dateTable.Columns[i].ColumnName);
                    if (_dateTable.Rows.Count != 0)
                    {
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
                }
                workbook.Worksheets.Add(worksheet);
                var p = Path.Combine(Environment.CurrentDirectory, @"exels");
                workbook.Save(path);
                CoreContext.ViewService.MainView.ShowMessag(string.Format("Файл {0} успешно сохранен.", path));
                }
                else
                {
                    Log.Inst.WriteToLogDEBUG(string.Format("Not DataRow for excel"));
                    CoreContext.ViewService.MainView.ShowMessag(string.Format("Данных нет, нечего сохранять"));
                }
              
            }
            catch (Exception ex)
            {
                Log.Inst.WriteToLogDEBUG(string.Format("Error save Excel {0}", ex.Message));
            }

            Log.Inst.WriteToLogDEBUG(string.Format("End save in excel file in path {0}", path));
            
        }

        /// <summary>
        ///     конструирование DataTable для представления на экран
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private DataTable BuilTableForView(DataTable dt)
        {
            Log.Inst.WriteToLogDEBUG("Start build table for view");
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
            _allBalans = GetCategoriesesBalance(allCard.ToList()); //берем для всех карт их лимит

            var listBalans = CopyListBalance(_allBalans);
            foreach (var i in allCard)
            {
                var infoCard = _infoCards.Where(x => x.NumberCard == i).FirstOrDefault();
                var t =
                    dt.AsEnumerable()
                        .Where(r => r["CardNumbers"].Equals(i))
                        .Where(r => r["TransactionType"].Equals("PayFromWallet")); //берем все транзакции по карте
                var d1 =
                    t.GroupBy(row => ((DateTime) row["TransactionCreateDate"]).ToString(Format))
                        .Reverse()
                        .ToList()
                        .Select(k => k.Key);
                //сгруппируем операции по дням
                foreach (var j in d1) //идем по дням
                {
                    //берем операции за нужный(текущий в цикле) день
                    var thisDayTranz =
                        t.Where(y => (((DateTime) y["TransactionCreateDate"]).ToString(Format)).Equals(j)).ToList();
                    // и сортируем по возрастанию/
                    var thisDayTranzSort =
                        thisDayTranz.OrderBy(x => ((DateTime) x["TransactionCreateDate"]).TimeOfDay).ToList();

                    foreach (var item in thisDayTranzSort) //идем по операцияем за день
                    {
                        var numberCard = item["CardNumbers"];
                        var resultRow = resultTable.NewRow();
                        var oldBalance =
                            listBalans.Where(r => r.NumberCard.Equals(numberCard))
                                .Select(r => r.Sum)
                                .FirstOrDefault();
                        // расчет дотации и кредита для таблицы представления
                        var sum = Math.Abs((decimal) item["TransactionSum"]);
                        var balance = oldBalance - sum;
                        if (balance >= 0)
                        {
                            resultRow["Дотация"] = sum;
                            resultRow["Кредит"] = 0;
                            listBalans.FirstOrDefault(t2 => t2.NumberCard.Equals(numberCard)).Sum = balance;
                        }
                        else
                        {
                            resultRow["Дотация"] = oldBalance;
                            resultRow["Кредит"] = sum - oldBalance;
                            listBalans.FirstOrDefault(t2 => t2.NumberCard.Equals(numberCard)).Sum = 0;
                        }
                        resultRow["Код карты"] = numberCard;
                        var tmpDate = (DateTime) item["TransactionCreateDate"];
                        resultRow["Дата/время"] = tmpDate.ToString("G");
                        resultRow["Дата"] = tmpDate.ToString(Format);
                        resultRow["Категория"] = infoCard.Category;
                        resultRow["Сумма чека"] = Math.Abs((decimal) item["TransactionSum"]);
                        resultRow["Телефон"] = item["PhoneNumber"];
                        var tmp = infoCard.FullName;
                        resultRow["ФИО"] = RemoveSpaces(RemoveTab(infoCard.FullName));
                        resultTable.Rows.Add(resultRow);
                    }
                    listBalans = CopyListBalance(_allBalans); //делаем снова полный лимит
                }
            }
            Log.Inst.WriteToLogDEBUG("End build table for view");
            return resultTable;
        }

        private string RemoveTab(string txt)
        {
            var text = txt.Replace("\t", " ");

            if (text.Contains("\t"))
                RemoveTab(text);
            return text;
        }

        private string RemoveSpaces(string txt)
        {
            var text = txt.Replace("  ", " ");

            if (text.Contains("  "))
                RemoveSpaces(text);
            return text;
        }

        /// <summary>
        ///     копирование списка дотации по значению
        /// </summary>
        /// <param name="oldList"></param>
        /// <returns></returns>
        private List<BalancCardNumber> CopyListBalance(List<BalancCardNumber> oldList)
        {
            var listBalans = new List<BalancCardNumber>();
            _allBalans.ForEach(item => { listBalans.Add(new BalancCardNumber(item.NumberCard, item.Sum)); });
            return listBalans;
        }

        private List<BalancCardNumber> GetCategoriesesBalance(List<string> allCards)
        {
            var listBalance = new List<BalancCardNumber>();
            var balancCatigories = CoreContext.ConfigService.GetConfig().BalanceDictionary;
            foreach (var card in allCards)
            {
                var param = new InfoCard();
                param.NumberCard = card;
                var response = CoreContext.MakerRequest.GetInfoByCard(card, _reportParameters.OrganizationInfoId).Result;
                param.FullName = response.Name + " " + response.Surname;
                var catigories = response.Categories;
                //берем категории которые прописанны в конфиге
                var trueCategories =
                    catigories.Select(item => balancCatigories.FirstOrDefault(t => t.NameCategories == item.Name))
                        .Where(tmp => tmp != null)
                        .ToList();

                if (trueCategories.Count != 0)
                {
                    var max = trueCategories.Max(categories => categories.Sum);
                    listBalance.Add(new BalancCardNumber(card, max));
                    param.Category = trueCategories.Where(t => t.Sum == max).FirstOrDefault().NameCategories;
                }
                else
                {
                    listBalance.Add(new BalancCardNumber(card, 0));
                }
                _infoCards.Add(param);
            }
            return listBalance;
        }
    }
}