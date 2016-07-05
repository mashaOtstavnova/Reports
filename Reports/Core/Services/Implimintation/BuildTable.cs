using System.Data;
using System.Linq;
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
    }
}