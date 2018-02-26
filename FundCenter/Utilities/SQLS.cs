using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FundCenter.Utilities
{
    public static class SQLS
    {
        public static readonly string GET_USER_BY_USER_NAME = "SELECT * FROM center_user WHERE username = '{0}'";

        public static readonly string ADD_FUND = "INSERT INTO fund_info VALUES('{0}', '{1}', '{2}', '{3}', '{4}')";

        public static readonly string GET_FUNDS = "SELECT * FROM fund_info WHERE fund_id in ( select max(fund_id) from  fund_info  group by fund_number)";

        public static readonly string GET_FUNDS_BY_FUND_NUMBER = "SELECT * FROM fund_info WHERE fund_number = {0} order by last_refresh_time asc";
    }
}