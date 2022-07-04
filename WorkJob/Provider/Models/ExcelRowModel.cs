using System;
using System.Collections.Generic;
using System.Text;

namespace Provider.Models
{
    public class ExcelRowModel
    {
        public DateTime Date { get; set; }
        public double CompositeIndex { get; set; }
        public List<double?> CompanyPrices { get; set; } = new List<double?>();
    }
}
