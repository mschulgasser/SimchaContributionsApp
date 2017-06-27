using SimchaContributions.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimchaContributionsApp.Models
{
    public class ShowHistoryViewModel
    {
        public List<Transaction> Transactions { get; set; }
        public Contributor Contributor { get; set; }
    }
}