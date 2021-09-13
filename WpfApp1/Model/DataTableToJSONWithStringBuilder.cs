using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;

namespace WpfApp1.Model
{
    class DataTableToJSONwithStringBuilder
    {
        public DataTable table = new DataTable();
        private string _path;
        private bool _timestamp;
       
        public DataTableToJSONwithStringBuilder(DataTable table, string path, bool timestamp)
        {
            this.table = table;
            this._path = path;
            this._timestamp = timestamp;
        }
        public string BuildJSON()
        {
            string _time = DateTime.Now.ToString("yyyyMMddHH");
            StringBuilder JSONString = new StringBuilder();
            if (table.Rows.Count > 0)
            {
                JSONString.Append("[");
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    JSONString.Append("{");
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        if (j < table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == table.Rows.Count - 1)
                    {
                        JSONString.Append("}");
                    }
                    else
                    {
                        JSONString.Append("},");
                    }
                }
                JSONString.Append("]");
            }
            if (_timestamp)
            {
                File.WriteAllText(_path + _time + ".json", JSONString.ToString());

            }else
            File.WriteAllText(_path, JSONString.ToString());
            return _path;
        }
    }
}
