using FundCenter.Logics;
using FundCenter.Utilities;
using FundCenter.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FundCenter.Controllers
{
    

    public class FundsController : Controller
    {
        private ICache _cache;
        private IFundsLogic _fundsLogic;

        public FundsController(ICache cache, IFundsLogic fundsLogic)
        {
            _cache = cache;
            _fundsLogic = fundsLogic;
        }
        //
        // GET: /Funds/
        [HttpGet]
        public async Task<ActionResult> SearchFundByFundNumber(string searchFundNumber)
        {
            return Json(await _fundsLogic.SearchFundByFundNumber(searchFundNumber), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddFundInList(FundViewModel fundViewModel)
        {
            return Json(_fundsLogic.AddFundInList(fundViewModel));
        }

        [HttpGet]
        public ActionResult GetFunds()
        {
            return Json(_fundsLogic.GetFunds(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetFundHistory(string fundNumber)
        {
            return Json(_fundsLogic.GetFundHistory(fundNumber), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetTestResult(string userName)
        {
            var num = 0;
            for (int i = 0; i < 1000; i++)
            {
                num++;
            }
            return Json("edwin su" + DateTime.UtcNow.Second, JsonRequestBehavior.AllowGet);
        }
	}
}