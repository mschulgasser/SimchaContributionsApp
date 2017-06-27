using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimchaContributions.Data
{
    public class Deposit
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int ContributorId { get; set; }
        public DateTime Date { get; set; }
    }
}
