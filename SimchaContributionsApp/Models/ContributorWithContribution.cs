using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimchaContributionsApp.Models
{
    public class ContributorWithContribution : ContributorWithBalance
    {
        public bool IsContributing { get; set; }
        public double Amount { get; set; }
    }
}