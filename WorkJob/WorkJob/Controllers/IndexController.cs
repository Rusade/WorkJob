using Microsoft.AspNetCore.Mvc;
using Provider.Constants;
using Provider.Helpers;
using Provider.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkJob.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("[controller]")]
    public class IndexController : Controller
    {
        private readonly IndexService IndexService;

        /// <summary>
        /// 
        /// </summary>
        public IndexController(IndexService indexService)
        {
            IndexService = indexService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("Companys")]
        public IActionResult GetCompanyNames()
        {
            return Ok(Consts.CompanyNames);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost("Calculate")]
        public IActionResult Calculcate(DateTime startDate, DateTime endDate, string companys)
        {
            var data = IndexService.Calculate(startDate, endDate, companys);
            return Ok(data);
        }

        [HttpPost("ReloadData")]
        public IActionResult ReloadData()
        {
            ExcelHelper.Init();
            return Ok();
        }
    }
}
