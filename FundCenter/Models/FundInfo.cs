using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FundCenter.Models
{
    public class FundInfo
    {
        public long fund_id { get; set; }

        public string fund_number { get; set; }

        public string fund_name { get; set; }

        public decimal owned_value { get; set; }

        public string current_change_value { get; set; }

        public DateTime last_refresh_time { get; set; }
    }
}