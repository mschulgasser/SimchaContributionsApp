using SimchaContributions.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimchaContributionsApp.Models
{
    public class ContributionViewModel
    {
        public List<ContributorWithContribution> Contributors { get; set; }
        public Simcha Simcha { get; set; }
    }
}