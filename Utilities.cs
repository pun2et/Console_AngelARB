using AngelArbApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AngelArbApp
{
    public static class Utilities
    {
        public static List<STKFutList> GetNSEFutList()
        {
            var data = System.IO.File.ReadAllText("Data\\Angel_Master_OCT.json").ToString();
            var masterData = JsonConvert.DeserializeObject<List<MMData>>(data);
            var sortedlist = masterData.OrderBy(o => o.name).ToList();
            var Futlist = sortedlist.ToList().FindAll(a => a.exch_seg == "NFO" && a.instrumenttype == "FUTSTK");
            List<STKFutList> stkfutList = new List<STKFutList>();

            foreach (var item in Futlist)
            {
                if (stkfutList.Exists(a => a.StockSymbol == item.name))
                {
                    stkfutList.FirstOrDefault(b => b.StockSymbol == item.name).Cntrs.Add(item);

                }
                else
                {
                    var eqToken = sortedlist.ToList().Find(a => a.exch_seg == "NSE" && a.instrumenttype == "" && a.symbol.Contains("-EQ") && a.name == item.name);

                    var miniContract = new STKFutList();
                    miniContract.StockSymbol = item.name;
                    miniContract.StockToken = eqToken.token;
                    miniContract.Cntrs = new List<MMData>();
                    miniContract.Cntrs.Add(item);
                    stkfutList.Add(miniContract);
                }

            }

            List<STKFutList> ValidContractsList = new List<STKFutList>();

            foreach (var item in stkfutList)
            {
                if (item.Cntrs.Count > 2)
                {
                    ValidContractsList.Add(item);
                }
            }

            return ValidContractsList;
        }
    }
}
