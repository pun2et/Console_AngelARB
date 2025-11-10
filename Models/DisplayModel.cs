using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clientupstoxone.Models
{
    
    public class DisplayModel
    {
        public string Symbol { get; set; }
        public string Price { get; set; }
        public string NearFUT_Price { get; set; }
        public string NextFUT_Price { get; set; }
        public string FUT_Difference_per { get; set; }
        public string Lot_Size { get; set; }
        public string NearFUT_Sprade { get; set; }
        public string NextFUT_Sprade { get; set; }
        public string Total_Sprade { get; set; }
        public string Updated_time { get; set; }
    }

    public class OptionSpreadModel
    {
        public string Symbol { get; set; }
        public string Avg_cost_CM { get; set; }
        public string Avg_cost_NM { get; set; }
        public string Avg_cost_FM { get; set; }
        public string Updated_time { get; set; }
    }

    public class  OIBidAsk
    {

        public float OI { get; set; }
        public float Bid{ get; set; }
        public float Ask { get; set; }
    }
}
