using SimchaContributions.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimchaContributionsApp.Models
{
    public class IndexViewModel
    {
        public List<SimchaWithContributionInfo> Simchas { get; set; }
        public int TotalContributors { get; set; }
    }
}