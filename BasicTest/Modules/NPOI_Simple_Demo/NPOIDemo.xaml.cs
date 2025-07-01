using Google.Protobuf.WellKnownTypes;
using NPOI.SS.Formula;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using System.Windows;
using nnp = NPOI.HSSF.UserModel;
using np = NPOI.XSSF.UserModel;

namespace BasicTest
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class NPOIDemo : Window
    {
        public NPOIDemo()
        {
            InitializeComponent();
        }

        private NPOI.SS.UserModel.ISheet? st;
        private void ReadExcelColumns_Click(object sender, RoutedEventArgs e)
        {
            excelListView.Items.Clear();

            int columnIndex = int.Parse(ColumnIndex.Text) - 1; // NPOI的列索引从0开始
            int startRowIndex = int.Parse(StartRowIndex.Text) - 1; // 用户输入的行索引从1开始，所以需要减1
            int endRowIndex = int.Parse(EndRowIndex.Text) - 1;
            string filePath = ExcelFileSelectControl.ExcelFilePath ?? string.Empty;
            bool readReallyValue = ReadReallyValue.IsChecked ?? false; // 是否读取实际值

            if (string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("Please select an Excel file.");
                return;
            }
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var workbook = new np.XSSFWorkbook(fileStream); // 使用XSSFWorkbook处理.xlsx文件
                //var workbook = new nnp.HSSFWorkbook(fileStream); // 使用HSSFWorkbook处理.xls文件
                st = workbook.GetSheetAt(0); // 获取第一个工作表

                for (int rowIndex = startRowIndex; rowIndex <= endRowIndex; rowIndex++)
                {
                    var row = st.GetRow(rowIndex);
                    if (row == null)
                    {
                        continue;
                    }
                    var cell = row.GetCell(columnIndex);
                    string? cellValue = "Null Cell";
                    if (cell == null)
                    {
                        excelListView.Items.Add(cellValue);
                        continue;
                    }
                    switch (cell.CellType)
                    {
                        case NPOI.SS.UserModel.CellType.String:
                            cellValue = cell.StringCellValue;
                            break;
                        case NPOI.SS.UserModel.CellType.Numeric:
                            // 判断是否为日期格式
                            if (NPOI.SS.UserModel.DateUtil.IsCellDateFormatted(cell))
                            {
                                cellValue = GetFormatDate(cell);
                            }
                            else
                            {
                                cellValue = cell.NumericCellValue.ToString();
                            }
                            break;
                        case NPOI.SS.UserModel.CellType.Boolean:
                            cellValue = cell.BooleanCellValue.ToString();
                            break;
                        case NPOI.SS.UserModel.CellType.Formula:
                            string formula = FormulaCompatible(cell.CellFormula);
                            if (cell.CachedFormulaResultType == NPOI.SS.UserModel.CellType.Error)
                            {
                                byte errorCode = cell.ErrorCellValue;
                                string errorString = FormulaError.ForInt(errorCode).String;
                                cellValue = $"Error Formula: ={formula} ({errorString})";
                            }
                            else
                            {
                                //var cellCached = new XSSFEvaluationCell(cell); // 这里的XSSFEvaluationCell是NPOI的一个类，用于处理公式计算
                                string formulaValue = GetFormulaCellValue(cell);
                                cellValue = $"Formula: ={formula} ({formulaValue})";
                            }
                            break;
                        case NPOI.SS.UserModel.CellType.Unknown:
                            cellValue = "Unknown Cell Type";
                            break;
                        case NPOI.SS.UserModel.CellType.Blank:
                            cellValue = "Blank Cell Type";
                            break;
                        case NPOI.SS.UserModel.CellType.Error:
                            cellValue = cell.ErrorCellValue.ToString();
                            break;
                        default:
                            cellValue = "Unknown Cell Type";
                            break;
                    }
                    excelListView.Items.Add(cellValue); 
                }
            }
        }

        private string GetFormulaCellValue(NPOI.SS.UserModel.ICell cell)
        {
            string cellValue = string.Empty;
            switch (cell.CachedFormulaResultType)
            {
                case NPOI.SS.UserModel.CellType.String:
                    cellValue = cell.StringCellValue;
                    break;
                case NPOI.SS.UserModel.CellType.Numeric:
                    // 判断是否为日期格式
                    if (NPOI.SS.UserModel.DateUtil.IsCellDateFormatted(cell))
                    {
                        cellValue = GetFormatDate(cell);
                    }
                    else
                    {
                        cellValue = cell.NumericCellValue.ToString();
                    }
                    break;
                case NPOI.SS.UserModel.CellType.Boolean:
                    cellValue = cell.BooleanCellValue.ToString();
                    break;
                case NPOI.SS.UserModel.CellType.Formula:
                case NPOI.SS.UserModel.CellType.Unknown:
                case NPOI.SS.UserModel.CellType.Blank:
                case NPOI.SS.UserModel.CellType.Error:
                default:
                    cellValue = "Unknown Formula Value";
                    break;
            }
            return cellValue;
        }

        /// 处理Excel公式中的兼容性问题
        private string FormulaCompatible(string formula)
        {
            if (formula.StartsWith("_xlfn."))
            {
                formula = formula.Substring(6); // 去掉前缀
            }
            return formula;
        }

        private string GetFormatDate(ICell cell)
        {
            string cellValue = string.Empty;
            string? format = cell.CellStyle.GetDataFormatString();
            if (!string.IsNullOrEmpty(format))
            {
                if (format.Contains("y") || format.Contains("Y"))
                {
                    cellValue += cell.DateOnlyCellValue.ToString();
                }
                if (format.Contains("h") || format.Contains("H"))
                {
                    cellValue += " " + cell.TimeOnlyCellValue.ToString();
                }
                cellValue = cellValue.TrimStart();
            }
            else
            {
                cellValue = cell.DateCellValue.ToString();
            }
            return cellValue;
        }

        private void WriteExcel_Click(object sender, RoutedEventArgs e)
        {
            var wk = new np.XSSFWorkbook(); // 创建一个新的工作簿
            var sheet = wk.CreateSheet("第一页");
            var row = sheet.CreateRow(0);
            var cell = row.CreateCell(0);
            var region1 = new NPOI.SS.Util.CellRangeAddress(0, 4, 0, 4);
            sheet.AddMergedRegion(region1);

            var font1 = wk.CreateFont();
            font1.FontName = "Arial";
            font1.FontHeightInPoints = 15;
            font1.IsBold = true;
            font1.IsItalic = true;
            font1.Color = NPOI.SS.UserModel.IndexedColors.Red.Index; // 设置字体颜色为红色

            var style1 = wk.CreateCellStyle();
            style1.SetFont(font1);
            style1.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            style1.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            style1.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
            style1.FillForegroundColor = NPOI.SS.UserModel.IndexedColors.LightBlue.Index; // 设置填充颜色为浅蓝色
            cell.CellStyle = style1;

            cell.SetCellValue("Hello, Excel!");

            using (var fileData = new FileStream("output.xlsx", FileMode.Create, FileAccess.Write))
            {
                wk.Write(fileData);
            }

            MessageBox.Show("Excel file has been created successfully!");
        }
    }
}
