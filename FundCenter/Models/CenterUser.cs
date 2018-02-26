using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FundCenter.Models
{
    public class CenterUser
    {
        public long id { get; set; }

        public string username { get; set; }

        public string password { get; set; }

        public DateTime created_date { get; set; }

        public DateTime last_updated { get; set; }
    }
}