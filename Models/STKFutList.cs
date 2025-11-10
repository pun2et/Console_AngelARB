using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngelArbApp.Models
{
    public class STKFutList
    {
        public string StockSymbol { get; set; }
        public string StockToken { get; set; }
        public List<MMData> Cntrs { get; set; }
    }
}
