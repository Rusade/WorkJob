using System;
using System.Collections.Generic;
using System.Text;

namespace Provider.Models
{
    public class CompanyRelativeGainResposne
    {
        public List<string> Date { get; set; }
        public List<RelativeGainModel> RelativeGains { get; set; } = new List<RelativeGainModel>();
    }
}
