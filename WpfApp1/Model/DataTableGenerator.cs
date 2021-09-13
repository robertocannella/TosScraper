using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model
{
    class DataTableGenerator
    {
        private string _path;
        private List<string> _columnHeaders;
        private char[] delimiter = new char[] { '\t' };
        public DataTable dataTable = new DataTable();

        public DataTableGenerator(string SourcePath)
        {
            _path = SourcePath;

        }
        public DataTableGenerator(string SourcePath, List<string> ColumnHeaders)
        {
            _path = SourcePath;
            _columnHeaders = ColumnHeaders;
        }
        public DataTable GenerateDataTable()
        {
            System.IO.StreamReader streamreader = new System.IO.StreamReader(_path);

            if (_columnHeaders != null)
            {
                Console.WriteLine("ColumnHeaders Exist");
                foreach (string columnheader in _columnHeaders)
                {
                    dataTable.Columns.Add(columnheader);
                }
            }
            else
            {
                string[] columns = streamreader.ReadLine().Split(delimiter);
                Console.WriteLine("NO COLUMN HEADERS");
                foreach (var column in columns)
                    dataTable.Columns.Add();
            }

            while (streamreader.Peek() > 0)
            {
                DataRow datarow = dataTable.NewRow();
                datarow.ItemArray = streamreader.ReadLine().Split(delimiter);
                dataTable.Rows.Add(datarow);
            }

            return dataTable;
            
        }
        public void PrintDataTable()
        {
            foreach (DataRow row in dataTable.Rows)
            {
                Console.WriteLine(dataTable.Rows);

                foreach (DataColumn column in dataTable.Columns)
                {

                    Console.Write(row[column] + "\t\t");

                }
            }
        }
    }
}
