using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System.Data;
using NPOI.XSSF.UserModel;
using System.Windows.Controls;

namespace WPF_Explorer_Tree.EXCEL
{
   public static class ExcelMain
    {

        public static void Excel_To_DataTable(string path)
        {
            IWorkbook workbook;
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                workbook = new HSSFWorkbook(stream);
            }

            ISheet sheet = workbook.GetSheetAt(0); // zero-based index of your target sheet
            DataTable dt = new DataTable(sheet.SheetName);

            // write header row
            IRow headerRow = sheet.GetRow(0);
            foreach (ICell headerCell in headerRow)
            {
                dt.Columns.Add(headerCell.ToString());
            }

            // write the rest
            int rowIndex = 0;
            foreach (IRow row in sheet)
            {
                // skip header row
                if (rowIndex++ == 0) continue;
                DataRow dataRow = dt.NewRow();
                dataRow.ItemArray = row.Cells.Select(c => c.ToString()).ToArray();
                dt.Rows.Add(dataRow);
            }
        }



       public static DataTable grid_To_DataTable(DataGrid dgv)
        {
            var MaxRows = dgv.Items.Count;
            var MaxColumns = dgv.Columns.Count;
            DataTable dt = new DataTable();
            string val = "";
            TextBlock x;

            foreach (DataGridColumn col in dgv.Columns) dt.Columns.Add(col.Header.ToString());

            for (int r = 0; r < MaxRows - 1; r++)
            {
                DataRow dr = dt.NewRow();
                for (int c = 0; c < MaxColumns - 1; c++)
                {
                    x = dgv.Columns[c].GetCellContent(dgv.Items[r]) as TextBlock;
                    val = x.Text;
                    dr[c] = val;
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }


        //public static void DataTable_To_Excel(string path,DataTable dt)
        //{
        //    IWorkbook workbook = new XSSFWorkbook();
        //    workbook.CreateSheet("Sheet A1");
        //    workbook.CreateSheet("Sheet A2");
        //    workbook.CreateSheet("Sheet A3");

        //    FileStream sw = File.Create(path);
        //    workbook.Write(sw);
        //    sw.Close();
        //}


        /// <summary>
        /// 导出到Excel
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        static public bool ToExcel(string path, DataTable table,string sheetName)
        {
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            IWorkbook workBook = new HSSFWorkbook();

            ISheet sheet = workBook.CreateSheet(sheetName);

            //Tieu de
          //  IRow row = sheet.CreateRow(0);
         //   row.CreateCell(0).SetCellValue("tieude");
        //    sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, table.Columns.Count - 1));
            //row.Height = 500;

            //ICellStyle cellStyle = workBook.CreateCellStyle();
            //IFont font = workBook.CreateFont();
            //font.FontName = "微软雅黑";
            //font.FontHeightInPoints = 17;
            //cellStyle.SetFont(font);
            //cellStyle.VerticalAlignment = VerticalAlignment.Center;
            //cellStyle.Alignment = HorizontalAlignment.Center;
            //row.Cells[0].CellStyle = cellStyle;

            //处理表格列头
            IRow row = sheet.CreateRow(1);
            for (int i = 0; i < table.Columns.Count; i++)
            {
                row.CreateCell(i).SetCellValue(table.Columns[i].ColumnName);
                row.Height = 350;
                sheet.AutoSizeColumn(i);
            }

            //处理数据内容
            for (int i = 0; i < table.Rows.Count; i++)
            {
                row = sheet.CreateRow(2 + i);
                row.Height = 250;
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    row.CreateCell(j).SetCellValue(table.Rows[i][j].ToString());
                    sheet.SetColumnWidth(j, 256 * 15);
                }
            }

            //写入数据流
            workBook.Write(fs);
            fs.Flush();
            fs.Close();

            return true;
        }
    }

}
