using System.Data;
using System.Linq;

namespace Core.Services.Implimintation
{
    public class BuildTable : IBuildTable
    {
        private readonly DataTable _dateTable;

        public BuildTable()
        {
            _dateTable = new DataTable();
        }

        public DataTable BuiltTable(object[] obj)
        {
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
            
            return _dateTable;
        }
    }
}