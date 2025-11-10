using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngelArbApp
{
   
    public class InstrumentResp
    {
        public bool status { get; set; }
        public string message { get; set; }
        public string errorcode { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public Fetched[] fetched { get; set; }
        public object[] unfetched { get; set; }
    }

    public class Fetched
    {
        public string exchange { get; set; }
        public string tradingSymbol { get; set; }
        public string symbolToken { get; set; }
        public float ltp { get; set; }
        public float open { get; set; }
        public float high { get; set; }
        public float low { get; set; }
        public float close { get; set; }
        public double lastTradeQty { get; set; }
        public string exchFeedTime { get; set; }
        public string exchTradeTime { get; set; }
        public float netChange { get; set; }
        public float percentChange { get; set; }
        public float avgPrice { get; set; }
        public double tradeVolume { get; set; }
        public double opnInterest { get; set; }
        public float lowerCircuit { get; set; }
        public float upperCircuit { get; set; }
        public int totBuyQuan { get; set; }
        public int totSellQuan { get; set; }
        public float _52WeekLow { get; set; }
        public float _52WeekHigh { get; set; }
        public Depth depth { get; set; }
    }

    public class Depth
    {
        public Buy[] buy { get; set; }
        public Sell[] sell { get; set; }
    }

    public class Buy
    {
        public float price { get; set; }
        public int quantity { get; set; }
        public int orders { get; set; }
    }

    public class Sell
    {
        public float price { get; set; }
        public int quantity { get; set; }
        public int orders { get; set; }
    }

}
