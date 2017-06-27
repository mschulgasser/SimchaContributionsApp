using SimchaContributions.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimchaContributionsApp.Models
{
    public class SimchaWithContributionInfo
    {
        public Simcha Simcha { get; set; }
        public decimal Total { get; set; }
        public int ContributorCount { get; set; }
    }
}