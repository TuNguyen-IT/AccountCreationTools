using AutomationTesting.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AutomationTesting.Services
{
    public class ExcelService
    {
        private static string ExcelOutputPath { get; set; }
        private static string ExcelInputPath { get; set; }

        static ExcelService()
        {
            var output = ConfigurationManager.AppSettings["ExcelOutputUrl"];
            var input = ConfigurationManager.AppSettings["ExcelInputUrl"];
            if (!output.EndsWith("\\"))
            {
                output += "\\";
            }

            if (!input.EndsWith("\\"))
            {
                input += "\\";
            }
            var excelName = ConfigurationManager.AppSettings["ExcelName"];

            ExcelOutputPath = output + excelName + ".xlsx";
            ExcelInputPath = input + excelName + ".xlsx";
        }

        public static List<Account> ReadDataFromExcel()
        {
            List<Account> lst = new List<Account>();
            try
            {
                // mở file excel
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                ExcelPackage excel = null;

                if (!File.Exists(ExcelInputPath))
                {
                    return null;
                }
                else
                {
                    excel = new ExcelPackage(new FileInfo(ExcelInputPath));
                }

                ExcelWorksheet workSheet = excel.Workbook.Worksheets.FirstOrDefault();

                for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                {
                    try
                    {
                        int j = 2;

                        string userName = workSheet.Cells[i, j++].Value.ToString();
                        string password = workSheet.Cells[i, j++].Value.ToString();
                        lst.Add(new Account { Email = userName, Password = password });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return lst;
        }

        public static void SaveExcel(string email, string pw)
        {
            var account = new Account()
            {
                Email = email,
                Password = pw
            };

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excel;
            if (!File.Exists(ExcelOutputPath))
            {
                excel = new ExcelPackage();
                excel.Workbook.Worksheets.Add("Sheet1");
                excel.SaveAs(new FileInfo(ExcelOutputPath));
            }

             excel = new ExcelPackage(new FileInfo(ExcelOutputPath));
            if (excel == null)
            {
                excel = new ExcelPackage();
            }
            // name of the sheet
            var workSheet = excel.Workbook.Worksheets.FirstOrDefault();

            workSheet.TabColor = System.Drawing.Color.Black;
            workSheet.DefaultRowHeight = 12;

            // Setting the properties of the header
            workSheet.Row(1).Height = 20;
            workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Row(1).Style.Font.Bold = true;

            // Header of the Excel sheet
            workSheet.Cells[1, 1].Value = "No.";
            workSheet.Cells[1, 2].Value = "User Name";
            workSheet.Cells[1, 3].Value = "Password";

            // get number of rows in the sheet
            int rows = workSheet.Dimension != null ? workSheet.Dimension.Rows + 1 : 1;

            workSheet.Cells[rows, 1].Value = workSheet.Dimension != null ? workSheet.Dimension.Rows : 1;
            workSheet.Cells[rows, 2].Value = email;
            workSheet.Cells[rows, 3].Value = pw;

            // By default, the column width is not 
            // set to auto fit for the content
            // of the range, so we are using
            // AutoFit() method here. 
            workSheet.Column(1).AutoFit();
            workSheet.Column(2).AutoFit();
            workSheet.Column(3).AutoFit();


            // Create excel file on physical disk 
            FileStream objFileStrm = File.Create(ExcelOutputPath);
            objFileStrm.Close();

            // Write content to excel file 
            File.WriteAllBytes(ExcelOutputPath, excel.GetAsByteArray());
            //Close Excel package
            excel.Dispose();

        }

        
    }
}
