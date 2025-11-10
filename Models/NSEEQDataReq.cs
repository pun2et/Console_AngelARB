using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngelArbApp.Models
{
    public class NSEEQDataReq
    {
        public string mode { get; set; }
        public Exchangetokens exchangeTokens { get; set; }
    }

    public class Exchangetokens
    {
        public string[] NSE { get; set; }
    }

}
