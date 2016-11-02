using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Diagnostics;

namespace IBussiness
{
    public class ExportExcel
    {
        public void CreateExcel(System.Data.DataTable dt, string excelPath)
        {
            try
            {
                if (dt == null || dt.Rows.Count == 0) return;
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

                if (xlApp == null)
                {
                    return;
                }
                System.Globalization.CultureInfo CurrentCI = System.Threading.Thread.CurrentThread.CurrentCulture;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                Microsoft.Office.Interop.Excel.Workbooks workbooks = xlApp.Workbooks;
                Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
                Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];
                Microsoft.Office.Interop.Excel.Range range;
                long totalCount = dt.Rows.Count;
                long rowRead = 0;
                float percent = 0;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1] = dt.Columns[i].ColumnName;
                    range = (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, i + 1];
                    range.Interior.ColorIndex = 15;
                    range.Font.Bold = true;
                }
                for (int r = 0; r < dt.Rows.Count; r++)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        worksheet.Cells[r + 2, i + 1] = dt.Rows[r][i].ToString();
                    }
                    rowRead++;
                    percent = ((float)(100 * rowRead)) / totalCount;  //百分比
                }
                xlApp.Visible = false;
                workbook.Saved = true;
                workbook.SaveCopyAs(excelPath);
                workbook.Close(true, Type.Missing, Type.Missing);
                workbook = null;
                xlApp.Quit();
                xlApp = null;
            }
            catch
            {
                
            }
            finally
            {
                GC.Collect();
              
            }

        }

   
        /// <summary>
        /// 打开模板Excel,重新写入 另存Excel 文件格式 xls 97-2003
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="excelPath"></param>
        /// <param name="tempExcelPath"></param>
        public void InsertExcel(System.Data.DataTable dt, string savePath)
        {
            savePath = savePath.Replace("/", "\\");
            Application application = new ApplicationClass();
            application.Visible = false;

            if (File.Exists(savePath))
            {
                File.Delete(savePath);
            }

           //创建 新的Excel
            Workbook workbook = application.Workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
                
            //插入 现有的Excel
            //application.Workbooks._Open(tempExcelPath, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
            Worksheet worksheet = (Worksheet)workbook.Sheets[1];
            int rowNum = 1;
            int excelColumNum = 0;
            try
            {
                int count = dt.Rows.Count;
               

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1] = dt.Columns[i].ColumnName;
                    Range range = (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, i + 1];
                    range.Interior.ColorIndex = 15;
                    range.Font.Bold = true;
                }

                foreach (System.Data.DataRow row in dt.Rows)
                {
                    rowNum++;
                    //Console.WriteLine("当前处理记录:{0}/{1}", rowNum, count);
                    for (int i = 1; i <= dt.Columns.Count; i++)
                    {
                        excelColumNum = i;
                        string text = row[i - 1].ToString();
                        Range range = (Range)worksheet.Cells[rowNum, excelColumNum];
                        range.Value2 = text;
                    }
                }
                workbook.Saved = true;
                workbook.SaveAs(savePath, XlFileFormat.xlExcel8, Missing.Value, Missing.Value, Missing.Value, Missing.Value, XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
               
                workbook.Close(true, Type.Missing, Type.Missing);
                workbook = null;
                application.Quit();
                GC.Collect();
            }
            catch
            {
                workbook.Saved = false;
                workbook.Close(true, Type.Missing, Type.Missing);
                workbook = null;
                application.Quit();
                GC.Collect();
            }
           
        }
    }
}
