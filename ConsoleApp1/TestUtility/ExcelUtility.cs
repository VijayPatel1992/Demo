using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.TestUtility
{
    public class ExcelUtility
    {

        static List<Datacollection> dataCol = new List<Datacollection>();

        public class Datacollection
        {
            public int rowNumber { get; set; }
            public string colName { get; set; }
            public string colValue { get; set; }
        }
        public static DataTable ConvertExcelToDataTable(string FileName, string SheetNameToOpen)
        {
            try
            {
                DataTable dtResult = null;
                int totalSheet = 0; //No of sheets on excel file  
                OleDbConnection objConn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FileName + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1;';");

                objConn.Open();
                OleDbCommand cmd = new OleDbCommand();
                OleDbDataAdapter oleda = new OleDbDataAdapter();
                DataSet ds = new DataSet();
                DataTable dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string sheetName = string.Empty;
                if (dt != null)
                {
                    var tempDataTable = (from dataRow in dt.AsEnumerable()
                                         where !dataRow["TABLE_NAME"].ToString().Contains("FilterDatabase")
                                         select dataRow).CopyToDataTable();
                    dt = tempDataTable;
                    totalSheet = dt.Rows.Count;

                    for (int i = 0; i < totalSheet; i++)
                    {
                        sheetName = dt.Rows[i]["TABLE_NAME"].ToString();

                        if (dt.Rows[i]["TABLE_NAME"].ToString().Substring(0, sheetName.Length - 1).Equals(SheetNameToOpen))
                        {
                            sheetName = dt.Rows[i]["TABLE_NAME"].ToString();
                            break;
                        }
                    }
                }
                cmd.Connection = objConn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM [" + sheetName + "]";
                oleda = new OleDbDataAdapter(cmd);
                oleda.Fill(ds, "excelData");
                dtResult = ds.Tables["excelData"];
                objConn.Close();
                return dtResult; //Returning Dattable  


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }

        }
        public static List<Datacollection>  PopulateInCollection(string fileName, string SheetName)
        {
            dataCol.Clear();

            DataTable table = ConvertExcelToDataTable(fileName, SheetName);

            //Iterate through the rows and columns of the Table
            for (int row = 1; row <= table.Rows.Count; row++)
            {
                for (int col = 0; col < table.Columns.Count; col++)
                {
                    Datacollection dtTable = new Datacollection()
                    {
                        rowNumber = row,
                        colName = table.Columns[col].ColumnName,
                        colValue = table.Rows[row - 1][col].ToString()
                    };

                    //Add all the details for each row
                    dataCol.Add(dtTable);

                }
            }

            return dataCol;

        }
    }
}
