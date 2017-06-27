using SimchaContributions.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimchaContributionsApp.Models
{
    public class PotentialContribution : Contribution
    {
        public bool IsContributing { get; set; }
    }
}