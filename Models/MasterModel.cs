using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngelArbApp.Models
{
   

    public class MasterModel
    {
        public MMData[] ObjectInfo { get; set; }
    }

    public class MMData
    {
        public string token { get; set; }
        public string symbol { get; set; }
        public string name { get; set; }
        public string expiry { get; set; }
        public string strike { get; set; }
        public string lotsize { get; set; }
        public string instrumenttype { get; set; }
        public string exch_seg { get; set; }
        public string tick_size { get; set; }
    }

}
