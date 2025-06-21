using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PF.Core.Entity {

    [SugarTable("dayrecords")]
    public class DayRecord {


        public int Id { set; get; }

        public string UpdateTimeFlag { set; get; }

        public DateTime Date { set; get; }

        public string InOrAfterTrade { set; get; }

        public string Remark { set; get; }

        public DateTime? ModifiedTime { get; set; }
    }
}
