using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1.Model
{
    class JSONFileParser
    {
        private string _path;

        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        DataTable dataTable = new DataTable();
        char[] delimiter = new char[] { '\t' };

 
        public JSONFileParser()
        {
            
        }
        public void OutPutJSON(string sourcePath)
        {
            StreamReader streamreader = new StreamReader(sourcePath);

            string[] columns = streamreader.ReadLine().Split(delimiter);
            List<string> columnheaders = new List<string>
            {
                "Time",
                "Contract",
                "QTY",
                "Price",
                "Exchange",
                "Market",
                "Delta",
                "Imp Vol",
                "Underlying",
                "Condition",
                "Not Used"
            };
            string destinationPath = sourcePath + ".json";

            var csv = new List<string[]>();
            var lines = System.IO.File.ReadAllLines(sourcePath);
            

            foreach (string columnheader in columnheaders)
            {
                if (dataTable.Columns.Contains(columnheader))
                    return;

                dataTable.Columns.Add(columnheader);
            }

            // ------------------- Use the following to omit headers run to create columns
            //foreach (var column in columns)
            //    dataTable.Columns.Add();

            
            while (streamreader.Peek() > 0)
            {
               
                //DataRow datarow = dataTable.NewRow();

                //datarow.ItemArray = streamreader.ReadLine().Split(delimiter);
                //dataTable.Rows.Add(datarow);
            }
            foreach (DataColumn column in dataTable.Columns)
                Console.Write(column + "\t\t");


            foreach (DataRow row in dataTable.Rows)
            {

                //Console.WriteLine("\"----Row No: " + dataTable.Rows.IndexOf(row) + "----\"");
                //Console.WriteLine(dataTable.Rows.IndexOf(row));
                Console.WriteLine(dataTable.Rows);


                foreach (DataColumn column in dataTable.Columns)
                {


                    //Console.Write(column.ColumnName);


                    //check what columns you need
                    //if (column.ColumnName == "Column2" ||
                    //    column.ColumnName == "Column12" ||
                    //    column.ColumnName == "Column45")
                    //{
                    //Console.Write(" ");
                    Console.Write(row[column] + "\t\t");
                    //}
                }
            }
        }
        public void OutPutDelimited(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("File Not Found",path);
              
            }

            StreamReader streamreader = new StreamReader(path);

                string[] columns = streamreader.ReadLine().Split(delimiter);



            List<string> columnheaders = new List<string>
            {
                "Time",
                "Contract",
                "QTY",
                "Price",
                "Exchange",
                "Market",
                "Delta",
                "Imp Vol",
                "Underlying",
                "Condition",
                "Not Used"
            };


            foreach (string columnheader in columnheaders)
            {
                if (dataTable.Columns.Contains(columnheader))
                    return;
                
                 dataTable.Columns.Add(columnheader);
            }

            // ------------------- Use the following to omit headers run to create columns
            //foreach (var column in columns)
            //    dataTable.Columns.Add();




            while (streamreader.Peek() > 0)
            {
                DataRow datarow = dataTable.NewRow();
                
                datarow.ItemArray = streamreader.ReadLine().Split(delimiter);
                dataTable.Rows.Add(datarow);
            }
            foreach (DataColumn column in dataTable.Columns)
                Console.Write(column +"\t\t");


            foreach (DataRow row in dataTable.Rows)
            {

                //Console.WriteLine("\"----Row No: " + dataTable.Rows.IndexOf(row) + "----\"");
                //Console.WriteLine(dataTable.Rows.IndexOf(row));
                Console.WriteLine(dataTable.Rows);
                

                foreach (DataColumn column in dataTable.Columns)
                {
              
                    
                        //Console.Write(column.ColumnName);
                     
                    
                    //check what columns you need
                    //if (column.ColumnName == "Column2" ||
                    //    column.ColumnName == "Column12" ||
                    //    column.ColumnName == "Column45")
                    //{
                    //Console.Write(" ");
                        Console.Write(row[column] + "\t\t");
                    //}
                }
            }
        }
        public void OutPutText()
        {
            using (FileStream fs = File.OpenRead(_path))
            {
                byte[] b = new byte[1024];
                UTF8Encoding temp = new UTF8Encoding(true);
                while (fs.Read(b, 0, b.Length) > 0)
                {
                    Console.WriteLine(temp.GetString(b));
                }
            }
        }

    }
}
