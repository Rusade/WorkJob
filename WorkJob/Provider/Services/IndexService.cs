using Provider.Helpers;
using Provider.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Provider.Constants.Consts;

namespace Provider.Services
{
    public class IndexService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Result<CompanyRelativeGainResposne> Calculate(DateTime startDate, DateTime endDate, string companys)
        {
            // check and get paramaters
            endDate = endDate.AddMonths(1).AddDays(-1);
            var checkParamaterResult = CheckParamaterAndSplitCompanys(startDate, endDate, companys);
            if (!checkParamaterResult.IsSuccess)
                return Result<CompanyRelativeGainResposne>.ReturnError(checkParamaterResult.Message);

            //process
            var companyIndexList = checkParamaterResult.Data;
            var calculateDataList = ExcelDataList.Where(d => d.Date >= startDate && d.Date <= endDate).ToList();
            var result = new CompanyRelativeGainResposne { Date = calculateDataList.Select(d => d.Date.ToString("yyyy-MM-dd")).ToList() };
            foreach (var companyIndex in companyIndexList)
                result.RelativeGains.Add(new RelativeGainModel { Name = CompanyNames[companyIndex], Data = new List<double> { 1 } });

            for (var dateIndex = 1; dateIndex < calculateDataList.Count; dateIndex++)
            {
                var indexChange = (calculateDataList[dateIndex].CompositeIndex / calculateDataList[dateIndex - 1].CompositeIndex) - 1d;
                for (var index = 0; index < companyIndexList.Count; index++)
                {
                    var companyChange = 0d;
                    if (calculateDataList[dateIndex].CompanyPrices[companyIndexList[index]] != null && calculateDataList[dateIndex - 1].CompanyPrices[companyIndexList[index]] != null)
                        companyChange = ((double)calculateDataList[dateIndex].CompanyPrices[companyIndexList[index]] / (double)calculateDataList[dateIndex - 1].CompanyPrices[companyIndexList[index]]) - 1d;

                    result.RelativeGains[index].Data.Add((companyChange - indexChange + 1) * result.RelativeGains[index].Data[result.RelativeGains[index].Data.Count - 1]);
                }
            }

            return Result<CompanyRelativeGainResposne>.ReturnSuccess(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Result<List<int>> CheckParamaterAndSplitCompanys(DateTime startDate, DateTime endDate, string companys)
        {
            List<int> companyIndexList = new List<int>();
            if (startDate > ExcelDataList[ExcelDataList.Count - 1].Date)
                return Result<List<int>>.ReturnError($"开始时间需要早于{ExcelDataList[ExcelDataList.Count - 1].Date.ToString("yyyy-MM")}");
            if (endDate < ExcelDataList[0].Date)
                return Result<List<int>>.ReturnError($"结束时间需要晚于{ExcelDataList[0].Date.ToString("yyyy-MM")}");
            //split company
            if (string.IsNullOrEmpty(companys))
            {
                for (var index = 0; index < CompanyNames.Count; index++)
                {
                    companyIndexList.Add(index);
                }
            }
            else
            {
                var companyIndexs = companys.Split(',');
                for (var index = 0; index < companyIndexs.Length; index++)
                {
                    try
                    {
                        var companyIndex = Convert.ToInt32(companyIndexs[index]);
                        companyIndexList.Add(companyIndex);
                    }
                    catch (Exception e)
                    {
                        return Result<List<int>>.ReturnError("公司不合法");
                    }
                }
                companyIndexList = companyIndexList.OrderBy(i => i).ToList();
            }

            return Result<List<int>>.ReturnSuccess(companyIndexList);
        }
    }
}
