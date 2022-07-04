using System;
using System.Collections.Generic;
using System.Text;

namespace Provider.Models
{
    public class RelativeGainModel
    {
        public string Name { get; set; }
        public string Type { get { return "line"; } }
        public List<double> Data { get; set; } = new List<double>();
    }
}
