using Microsoft.Office.Interop.Excel;
using System.Data;
using System.Runtime.InteropServices;
using DataTable = System.Data.DataTable;
using Range = Microsoft.Office.Interop.Excel.Range;

namespace BasicTest
{
    public static class ExcelService
    {
        //Tips:
        //     Marshal.GetActiveObject仅在.NET Framework中可用
        //     var excelApp = Marshal.GetActiveObject("Excel.Application") as Application;

        /// <summary>
        /// 读取Excel数据到DataTable
        /// </summary>
        public static DataTable ReadExcelToDataTable(string filePath)
        {
            Application excelApp = null;
            Workbook workbook = null;
            DataTable dataTable = new DataTable();

            try
            {
                // 创建Excel应用实例 
                excelApp = new Application { Visible = false, DisplayAlerts = false };
                workbook = excelApp.Workbooks.Open(filePath);

                Worksheet worksheet = workbook.Sheets[1]; // 获取第一个工作表 
                Range usedRange = worksheet.UsedRange;    // 获取实际使用区域 

                // 获取行列数
                int rowCount = usedRange.Rows.Count;
                int colCount = usedRange.Columns.Count;

                // 处理标题行（第一行作为列名）
                for (int col = 1; col <= colCount; col++)
                {
                    string columnName = usedRange.Cells[1, col]?.Value2?.ToString() ?? $"Column_{col}";
                    dataTable.Columns.Add(columnName);
                }

                // 从第二行开始读取数据 
                for (int row = 2; row <= rowCount; row++)
                {
                    DataRow dataRow = dataTable.NewRow();
                    bool hasData = false;

                    for (int col = 1; col <= colCount; col++)
                    {
                        Range cell = usedRange.Cells[row, col];
                        if (cell.Value2 != null)
                        {
                            dataRow[col - 1] = GetCellValue(cell);
                            hasData = true;
                        }
                    }

                    if (hasData) dataTable.Rows.Add(dataRow);
                }

                return dataTable;
            }
            finally
            {
                // 清理COM对象 
                workbook?.Close(false);
                excelApp?.Quit();
                ReleaseComObjects(workbook, excelApp);
            }
        }

        private static object GetCellValue(Range cell)
        {
            switch (cell.Value2)
            {
                case double d when d == (int)d: // 处理整数 
                    return (int)d;
                case double d:                  // 处理浮点数 
                    return d;
                case string s:                  // 字符串
                    return s;
                case DateTime dt:               // 日期（Excel可能返回double）
                    return dt;
                default:                       // 其他类型（布尔值等）
                    return cell.Value2;
            }
        }

        private static void ReleaseComObjects(params object[] comObjects)
        {
            foreach (var obj in comObjects)
            {
                if (obj != null && Marshal.IsComObject(obj))
                    Marshal.ReleaseComObject(obj);
            }
            //GC.Collect();
            //GC.WaitForPendingFinalizers();
        }
    }
}
