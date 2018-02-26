using FundCenter.ViewModels;
using FundCenter.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using FundCenter.Models;
using System.Threading;

namespace FundCenter.Logics
{
    public delegate FundViewModel GetData(string url);
    public delegate FundViewModel FuncHandle(string url);

    public class FundsLogic : IFundsLogic
    {
        public async Task<FundViewModel> SearchFundByFundNumber(string searchFundNumber)
        {
            GetData getData = GetDataInTask;
            //Task<FundViewModel> t = Task.Factory.StartNew(delegate { getData(searchFundNumber.ToString()); });
            return await Task<FundViewModel>.Factory.StartNew(delegate { return getData(searchFundNumber.ToString()); });
        }

        public FundViewModel GetDataInTask(string searchFundNumber)
        {
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            string url = "http://fund.eastmoney.com/{0}.html?spm=aladin";
            string html = wc.DownloadString(String.Format(url, searchFundNumber));


            FundViewModel fundViewModel = new FundViewModel()
            {
                FundNumber = searchFundNumber,
                FundName = GetFundName(html),
                CurrentChangeValue = GetCurrentChangeValue(html),
                LastRefreshTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            return fundViewModel;
        }

        private string GetCurrentChangeValue(string html)
        {
            MatchCollection matches = Regex.Matches(html, "<span class=\"ui-font-middle ui-color-red ui-num\" id=\"gz_gszzl\">(.*)</span>");
            if (matches.Count == 0)
                matches = Regex.Matches(html, "<span class=\"ui-font-middle ui-color-green ui-num\" id=\"gz_gszzl\">(.*)</span>");

            string currentChangeValue = null;
            foreach (Match itemt in matches)
            {
                var temp = itemt.Groups[1].Value;
                currentChangeValue = temp.Substring(0, 5);
                
            }
            return currentChangeValue;
        }

        private string GetFundName(string html)
        {
            MatchCollection matches = Regex.Matches(html, "<div style=\"float: left\">(.*)<span>");

            string fundName = null;
            foreach (Match itemt in matches)
            {
                var temp = itemt.Groups[1].Value;
                fundName = temp.Substring(0, temp.IndexOf("<"));

            }
            return fundName;
        }
    

        public bool AddFundInList(FundViewModel fundViewModel)
        {
            return DBManager<bool>.AddData(string.Format(SQLS.ADD_FUND, fundViewModel.FundNumber, fundViewModel.FundName, fundViewModel.OwnedValue, string.IsNullOrEmpty(fundViewModel.CurrentChangeValue) ? "0" : fundViewModel.CurrentChangeValue.Replace("%", ""), fundViewModel.LastRefreshTime));
        }

        public List<FundViewModel> GetFunds()
        {
            List<FundInfo> fundInfos = DBManager<FundInfo>.GetDatas(SQLS.GET_FUNDS);
            List<FundViewModel> funds = new List<FundViewModel>();
            GetData getData = GetDataInTask;
            //Task<FundViewModel> t = Task.Factory.StartNew(delegate { getData(searchFundNumber.ToString()); });
            //return await Task<FundViewModel>.Factory.StartNew(delegate { return getData(searchFundNumber.ToString()); });

            foreach(var item in fundInfos)
            {
                var result = GetDataInTask(item.fund_number);
                AddFundInList(result);
                funds.Add(result);
                //getData.BeginInvoke(item.fund_number, (e) => 
                //{
                //    FuncHandle dl = e.AsyncState as FuncHandle;
                //    funds.Add(dl.EndInvoke(e));
                //}, null);
                //ThreadPool.
                //funds.Add(GetDataInTask(item.fund_number));
            }

            return funds;
        }


        public List<FundViewModel> GetFundHistory(string fundNumber)
        {
            List<FundInfo> fundInfos = DBManager<FundInfo>.GetDatas(string.Format(SQLS.GET_FUNDS_BY_FUND_NUMBER, fundNumber));
            List<FundViewModel> funds = new List<FundViewModel>();
            foreach (var item in fundInfos)
            {
                FundViewModel fvm = new FundViewModel();
                fvm.FundNumber = item.fund_number;
                fvm.FundName = item.fund_name;
                fvm.OwnedValue = item.owned_value;
                fvm.LastRefreshTime = item.last_refresh_time.ToString();
                fvm.CurrentChangeValue = item.current_change_value;
                funds.Add(fvm);
            }

            return funds;
        }
    }

    public interface IFundsLogic
    {
        List<FundViewModel> GetFunds();

        Task<FundViewModel> SearchFundByFundNumber(string searchFundNumber);

        bool AddFundInList(FundViewModel fundViewModel);

        List<FundViewModel> GetFundHistory(string fundNumber);
    }
}