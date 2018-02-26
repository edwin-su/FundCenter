using FundCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FundCenter.ViewModels
{
    public class FundViewModel
    {
        public string FundNumber { get; set; }

        public string FundName { get; set; }

        public decimal OwnedValue { get; set; }

        public string CurrentChangeValue { get; set; }

        public string LastRefreshTime { get; set; }
    }
}