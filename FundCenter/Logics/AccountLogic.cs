using FundCenter.Models;
using FundCenter.Utilities;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FundCenter.Logics
{
    public class AccountLogic : IAccountLogic
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private ICache _cache;
        public AccountLogic(ICache cache)
        {
            _cache = cache;
        }

        public long Login(string userName, string password)
        {
            CenterUser centerUser = DBManager<CenterUser>.GetData(string.Format(SQLS.GET_USER_BY_USER_NAME, userName));
            if (centerUser != null)
            {
                if (MD5Helper.MD5Encrypt64(password).Equals(centerUser.password))
                {
                    _cache.Add<string>(SessionConstant.USER, centerUser.id.ToString());
                    return centerUser.id;
                }
                else
                {
                    logger.Log(LogLevel.Info, "Invalid password", userName);
                    throw new Exception("Invalid password");
                }
            }
            else
            {
                logger.Log(LogLevel.Info, "Invalid username", userName);
                throw new Exception("Invalid username");
            }
        }

        public bool LogOut(long userId)
        {
            var user = _cache.Get<long>(SessionConstant.USER);
            if (user == userId)
            {
                _cache.Remove(SessionConstant.USER);
            }
            return true;
        }
    }

    public interface IAccountLogic
    {
        long Login(string userName, string password);

        bool LogOut(long userId);
    }
}