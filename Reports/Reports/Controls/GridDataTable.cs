using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraVerticalGrid;

namespace Reports.Controls
{
    public partial class GridDataTable : UserControl
    {
        public GridDataTable()
        {
            InitializeComponent();

        }

        public void PrintDT(DataTable dt)
        {
            gridControl1.DataSource = null;

            gridControl1.DataSource = dt;
            gridView1.ClearGrouping();
            gridView1.Columns["Дата"].GroupIndex = 0;
            
            AddSummaryColumn(new List<string>() { "Кредит", "Сумма чека", "Дотация" });

            gridView1.Columns["Дата/время"].DisplayFormat.FormatType = FormatType.DateTime;
            gridView1.Columns["Дата/время"].DisplayFormat.FormatString = "G";
            //настройки фильтра            
            gridView1.OptionsView.ShowFooter = true;
            gridView1.OptionsView.ShowAutoFilterRow = true;
            gridView1.OptionsMenu.ShowAutoFilterRowItem = true;

        }
        private void AddSummaryColumn(List<string> listColumName )
        {
            gridView1.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;
            foreach (var column in listColumName)
            {
                GridGroupSummaryItem item1 = new GridGroupSummaryItem();
                item1.FieldName = column;
                item1.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                item1.DisplayFormat = "{0:c2}";
                item1.ShowInGroupColumnFooter = gridView1.Columns[column];

                gridView1.Columns[column].Summary.Clear();
                gridView1.GroupSummary.Add(item1);
                
                gridView1.Columns[column].Summary.Add(DevExpress.Data.SummaryItemType.Sum, column, "{0:c2}");
            }
           
        }
    }
}