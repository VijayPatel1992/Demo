using BitMiracle.LibTiff.Classic;
using ConsoleApp1.Base;
using IronXL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;

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
            // Define a DataTable
            DataTable dataTable = new DataTable();
            try
            {

                // Load the Excel file
                WorkBook workbook = WorkBook.Load(FileName);

                // Select the worksheet
                WorkSheet worksheet = workbook.GetWorkSheet(SheetNameToOpen);

                // Loop through the rows and add them to the DataTable
                foreach (var row in worksheet.Rows)
                {
                    if (dataTable.Columns.Count == 0)
                    {
                        // Add columns to the DataTable if they haven't been added yet
                        foreach (var cell in row.Columns)
                        {
                            dataTable.Columns.Add(cell.Value.ToString());
                        }
                    }
                    else
                    {
                        // Add data rows to the DataTable
                        DataRow dataRow = dataTable.NewRow();
                        for (int i = 0; i < row.Columns.Count(); i++)
                        {
                            dataRow[i] = row.Columns[i].Value;
                        }
                        dataTable.Rows.Add(dataRow);
                    }
                }

                // Use the DataTable as needed

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
            return dataTable;
        }

        public static DataTable ConvertExcelToDataTable1(string FileName, string SheetNameToOpen)
        {
            //    try
            //    {
            //        DataTable dtResult = null;
            //        int totalSheet = 0; //No of sheets on excel file  
            //        OleDbConnection objConn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FileName + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1;';");

            //        objConn.Open();
            //        OleDbCommand cmd = new OleDbCommand();
            //        OleDbDataAdapter oleda = new OleDbDataAdapter();
            //        DataSet ds = new DataSet();
            //        DataTable dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            //        string sheetName = string.Empty;
            //        if (dt != null)
            //        {
            //            var tempDataTable = (from dataRow in dt.AsEnumerable()
            //                                 where !dataRow["TABLE_NAME"].ToString().Contains("FilterDatabase")
            //                                 select dataRow).CopyToDataTable();
            //            dt = tempDataTable;
            //            totalSheet = dt.Rows.Count;

            //            for (int i = 0; i < totalSheet; i++)
            //            {
            //                sheetName = dt.Rows[i]["TABLE_NAME"].ToString();

            //                if (dt.Rows[i]["TABLE_NAME"].ToString().Substring(0, sheetName.Length - 1).Equals(SheetNameToOpen))
            //                {
            //                    sheetName = dt.Rows[i]["TABLE_NAME"].ToString();
            //                    break;
            //                }
            //            }
            //        }
            //        cmd.Connection = objConn;
            //        cmd.CommandType = CommandType.Text;
            //        cmd.CommandText = "SELECT * FROM [" + sheetName + "]";
            //        oleda = new OleDbDataAdapter(cmd);
            //        oleda.Fill(ds, "excelData");
            //        dtResult = ds.Tables["excelData"];
            //        objConn.Close();
            //        return dtResult; //Returning Dattable  


            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex);
            //        return null;
            //    }
            return new DataTable();

        }

        public static List<Datacollection> PopulateInCollection(string fileName, string SheetName)
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

        public static void WriteDataInToExcelFile(string FileName, string SheetName, string FieldColumnName, string ValueColumnName, Hashtable FieldAndValueData)
        {
            try
            {
                WorkBook workbook;

                if (File.Exists(Path.Combine(BaseClass.CreatedExcelFilePath, FileName)))
                {
                    workbook = WorkBook.Load(Path.Combine(BaseClass.CreatedExcelFilePath, FileName));
                }
                else
                {
                    workbook = WorkBook.Create(ExcelFileFormat.XLSX);
                }

                WorkSheet worksheet = workbook.CreateWorkSheet(SheetName);

                //Set Column Name
                worksheet.SetCellValue(0, 0, FieldColumnName);
                worksheet.SetCellValue(0, 1, ValueColumnName);


                int i = 1;
                foreach (DictionaryEntry item in FieldAndValueData)
                {
                    worksheet.SetCellValue(i, 0, item.Key);

                    worksheet.SetCellValue(i, 1, item.Value);
                    i++;

                }


                workbook.SaveAs(Path.Combine(BaseClass.CreatedExcelFilePath, FileName));
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex);
            }

        }

        public static void WriteDataTableToExcel(string FileName, string SheetName, DataTable dataTable)
        {

            WorkBook workbook;
            WorkSheet worksheet;

            if (File.Exists(Path.Combine(BaseClass.CreatedExcelFilePath, FileName)))
            {
                workbook = WorkBook.Load(Path.Combine(BaseClass.CreatedExcelFilePath, FileName));
                worksheet = workbook.GetWorkSheet(SheetName);
            }
            else
            {
                workbook = WorkBook.Create(ExcelFileFormat.XLSX);
                worksheet = workbook.CreateWorkSheet(SheetName);
            }


            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                worksheet.SetCellValue(0, i, dataTable.Columns[i].ToString());
            }


            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                for (int j = 0; j < dataTable.Columns.Count; j++)
                {
                    worksheet.SetCellValue(i + 1, j , dataTable.Rows[i][j]);
                }
            }

          
            workbook.SaveAs(Path.Combine(BaseClass.CreatedExcelFilePath, FileName));
        }
    }
}
