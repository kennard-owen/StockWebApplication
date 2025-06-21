using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PF.Core.Entity {

    [SugarTable("tradedayrecords")]
    public class TradeDayRecord
    {

        public int Id { set; get; }

        public string YesOrNot { set; get; }

        public DateTime Date { set; get; }

        public int LastDayOfWeek { set; get; }

    }
}
