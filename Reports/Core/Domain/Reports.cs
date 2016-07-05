using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    /// <summary>
    /// сущность для отображения нужных параметров в dateGrid
    /// </summary>
    public class Reports
    {
        public string GuestId { get; set; }                  //Идентификатор гостя.
        public string GuestName { get; set; }                //Имя гостя.
        public string GuestPhone { get; set; }               //Телефон гостя.
        public string GuestCardTrack { get; set; }           //Номер карты гостя.
        public string GuestCategoryNames { get; set; }       //Список категорий гостя через запятую.
        public string EmployeeNumber { get; set; }           //Табельный номер гостя.
        public decimal BalanceOnPeriodStart { get; set; }    //Баланс на кошельке гостя на начало отчетного периода.
        public decimal BalanceOnPeriodEnd { get; set; }      //Баланс на кошельке гостя на конец отчетного периода.
        public decimal BalanceRefillSum { get; set; }        //Сумма пополнений кошелька пользователя за указанный период.
        public decimal PayFromWalletSum { get; set; }        //Сумма оплат с кошелька пользователя за указанный период.
        public decimal BalanceResetSum { get; set; }         //Сумма списаний с кошелька пользователя за указанный период.
        public decimal PaidOrdersCount { get; set; }         //Количество оплаченных пользователем заказов по заданной программе корпоративного питания за указанный период.
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
