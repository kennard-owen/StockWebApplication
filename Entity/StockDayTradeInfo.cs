using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PF.Core.Entity {
    [SugarTable("stockdaytradeinfoes")]
    public class StockDayTradeInfo {
        public int Id { set; get; }
        public DateTime Date { set; get; }
        public string StockNumber { set; get; }
        public decimal ? OpenPrice { set; get; }
        public decimal ? ClosePrice { set; get; }
        public decimal ? Margin { set; get; }
        public decimal ? Amplitude  { set; get; }
        public decimal ? Low { set; get; }
        public decimal ? High  { set; get; }
        public decimal ? Volume  { set; get; }
        public decimal ? Money  { set; get; }
        public decimal ? ChangeHand { set; get; }
    }
}
