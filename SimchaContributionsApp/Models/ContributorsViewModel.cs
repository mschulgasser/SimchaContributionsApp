using SimchaContributions.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimchaContributionsApp.Models
{
    public class ContributorsViewModel
    {
        public IEnumerable<ContributorWithBalance> Contributors { get; set; }
        public decimal Total { get; set; }
    }
}