using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngelArbApp.Models
{   

    public class SettingsModel
    {
        public string ClientCode { get; set; }
        public string ClientPIN { get; set; }
        public string TOTPCode { get; set; }
        public string APIKey { get; set; }
        public string TimeDelayInMiliSec { get; set; }
        public string CurrentMonth { get; set; }
        public string NextMonth { get; set; }
        public string FarMonth { get; set; }
        public string CurrentYear { get; set; }
        public string Spread_LessThan { get; set; }
        public string CurrentEXPDate { get; set; }
        public string NextEXPDate { get; set; }
        public string FarEXPDate { get; set; }
        public string DisplaySortBy { get; set; }
    }

}
