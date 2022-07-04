using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Provider.Constants;
using Provider.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Provider.Helpers
{
    public class ExcelHelper
    {
        public static void Init()
        {
            Consts.CompanyNames = GetCompanyNames();
            Consts.ExcelDataList = GetDataFromExcel();
        }

        /// <summary>
        /// 获取excel中的公司名称
        /// </summary>
        /// <returns></returns>
        private static List<string> GetCompanyNames()
        {
            var sheet = GetSheet();
            var titleRow = sheet.GetRow(0);
            var companyNames = new List<string>();

            for (var index = 2; index < titleRow.LastCellNum + 1; index++)
            {
                var cell = titleRow.GetCell(index);
                if (cell == null)
                    break;
                var companyName = cell.StringCellValue;
                if (string.IsNullOrEmpty(companyName))
                    break;
                companyNames.Add(companyName);
            }

            return companyNames;
        }

        /// <summary>
        /// 获取excel中具体数据
        /// </summary>
        private static List<ExcelRowModel> GetDataFromExcel()
        {
            var sheet = GetSheet();
            var result = new List<ExcelRowModel>();
            //获取全部数据
            for (var rowIndex = 1; rowIndex < sheet.LastRowNum + 1; rowIndex++)
            {
                var row = sheet.GetRow(rowIndex);
                if (row == null)
                    break;

                var data = new ExcelRowModel();
                data.Date = row.GetCell(0).DateCellValue;
                data.CompositeIndex = row.GetCell(1).NumericCellValue;
                for (var cellIndex = 0; cellIndex < Consts.CompanyNames.Count; cellIndex++)
                {
                    var cell = row.GetCell(cellIndex + 2);
                    if (cell == null)
                        data.CompanyPrices.Add(null);
                    else
                        data.CompanyPrices.Add(row.GetCell(cellIndex + 2).NumericCellValue);
                }
                result.Add(data);
            }
            return result;
        }

        /// <summary>
        /// 获取excel中的数据sheet
        /// </summary>
        /// <returns></returns>
        private static ISheet GetSheet()
        {
            var dataFile = new FileInfo(Consts.ExcelDataFilePath);

            var workbook = new XSSFWorkbook(dataFile);
            return workbook.GetSheetAt(1);
        }
    }
}
