using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PF_IoT.Models {
    public class ParaInterval {
        public string imei { get; set; }
        public int interval { get; set; }
        public string timeBegin { get; set; }
        public string timeEnd { get; set;}


        public int empty { get; set; }
        public int full { get; set; }
    }
}
