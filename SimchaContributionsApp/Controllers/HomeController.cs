using SimchaContributions.Data;
using SimchaContributionsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimchaContributionsApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            GetFromDB db = new GetFromDB(Properties.Settings.Default.ConStr);
            IndexViewModel vm = new IndexViewModel();
            vm.Simchas = db.GetSimchas().Select(s => new SimchaWithContributionInfo()
            {
                Simcha = s,
                Total = db.GetTotal(s.Id),
                ContributorCount = db.GetContributorCount(s.Id)
            }).ToList();
            vm.TotalContributors = db.GetTotalContributors();
            return View(vm);
        }
        [HttpPost]
        public ActionResult AddSimcha(Simcha s)
        {
            AddToDB db = new AddToDB(Properties.Settings.Default.ConStr);
            db.AddSimcha(s);
            return Redirect("/");
        }
        public ActionResult Contributors()
        {
            GetFromDB db = new GetFromDB(Properties.Settings.Default.ConStr);
            ContributorsViewModel vm = new ContributorsViewModel();
            vm.Total = db.GetTotalMoney();
            vm.Contributors = db.GetContributors().Select(c => new ContributorWithBalance()
            {
                Contributor = c,
                Balance = db.GetBalance(c.Id)
            });

            return View(vm);
        }
        [HttpPost]
        public ActionResult AddContributor(Contributor c, decimal amount)
        {
            AddToDB db = new AddToDB(Properties.Settings.Default.ConStr);
            int contributorId = db.AddContributor(c);
            Deposit d = new Deposit();
            d.Amount = amount;
            d.ContributorId = contributorId;
            d.Date = DateTime.Now;
            db.AddDeposit(d);
            return Redirect("/Home/Contributors");
        }
        public ActionResult ShowHistory(int id)//contributor id
        {
            GetFromDB db = new GetFromDB(Properties.Settings.Default.ConStr);
            ShowHistoryViewModel vm = new ShowHistoryViewModel();
            vm.Contributor = db.GetContributor(id);
            vm.Transactions = db.GetTransactions(id);
            return View(vm);
        }
        public ActionResult Contributions(int id)//simcha id
        {
            GetFromDB db = new GetFromDB(Properties.Settings.Default.ConStr);
            ContributionViewModel vm = new ContributionViewModel();
            vm.Contributors = db.GetContributors().Select(c => new ContributorWithContribution()
            {
                Contributor = c,
                Balance = db.GetBalance(c.Id),
                IsContributing = db.GetContributorContribution(c.Id, id) != 0 ? true : false,
                Amount = db.GetContributorContribution(c.Id, id) != 0 ? db.GetContributorContribution(c.Id, id) : 5.00
            }).ToList();
            vm.Simcha = db.GetSimcha(id);
            return View(vm);
        }
        [HttpPost]
        public ActionResult UpdateContributors(List<PotentialContribution> contributions)
        {
            AddToDB db = new AddToDB(Properties.Settings.Default.ConStr);
            foreach(PotentialContribution pc in contributions)
            {
                if (pc.IsContributing)
                {
                    db.AddContribution(new Contribution()
                    {
                        ContributorId = pc.ContributorId,
                        SimchaId = pc.SimchaId,
                        Amount = (pc.Amount)
                    });
                }
                else
                {
                    db.DeleteContribution(pc.ContributorId, pc.SimchaId);
                }
            }
            return Redirect("/Home/Index");
        }
        [HttpPost]
        public ActionResult AddDeposit(Deposit d)
        {
            AddToDB db = new AddToDB(Properties.Settings.Default.ConStr);
            db.AddDeposit(d);
            return Redirect("/Home/Contributors");
        }
        public ActionResult EditContributor(Contributor c)
        {
            AddToDB db = new AddToDB(Properties.Settings.Default.ConStr);
            db.EditContributor(c);
            return Redirect("/Home/Contributors");
        }
               
    }
    
}