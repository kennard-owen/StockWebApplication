using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PF.Core.Entity {

    [SugarTable("tecpararecordhis")]
    public class TecParaRecordHis {
        public int Id { set; get; }
        /// <summary>
        /// 股票代码
        /// </summary>
        public string StockNumber { get; set; }

        public DateTime? Date { get; set; } = DateTime.Now.Date;


        public string ParaSouce { set; get; }
        public string ProcessedPara { set; get; }

        public DateTime? CreatTime { get; set; } = DateTime.Now;


        //public string ParaBuy { set; get; }
        //public string ParaSell { set; get; }
        //public string ParaEvent { set; get; }
        //public string ParaZxst { set; get; }
    }
}
