using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PF.Core.Entity {
    public class Result {

        public int Id { set; get; }
        #region 整体
        /// <summary>
        /// 股票代码
        /// </summary>
        public string StockNumber { get; set; }
        /// <summary>
        /// 股票名称
        /// </summary>
        public string StockName { get; set; }
        /// <summary>
        /// 行业
        /// </summary>
        public string Section { get; set; }
        /// <summary>
        /// 交易地
        /// </summary>
        public string TradePlace { get; set; }
        /// <summary>
        /// 盘中盘后
        /// </summary>
        public string InOrAfterTrade { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 确诊时间
        /// </summary>
        public DateTime RecommendDate { get; set; }
        #endregion

        #region 排名趋势
        /// <summary>
        /// 排名比例
        /// </summary>
        public decimal Whole_BeatHowMany { get; set; }
        /// <summary>
        /// 买入等级
        /// </summary>
        public string Whole_BuyLevel { get; set; }
        /// <summary>
        /// 短期趋势
        /// /html[1]/body[1]/div[1]/div[6]/div[1]/div[1]/div[3]/ul[1]/li[1]/p[1]
        /// </summary>
        public string Whole_ShortTrend { get; set; }
        /// <summary>
        /// 中期趋势
        /// /html[1]/body[1]/div[1]/div[6]/div[1]/div[1]/div[3]/ul[1]/li[2]/p[1]
        /// </summary>
        public string Whole_MiddleTrend { get; set; }
        /// <summary>
        /// 长期趋势
        /// /html[1]/body[1]/div[1]/div[6]/div[1]/div[1]/div[3]/ul[1]/li[3]/p[1]
        /// </summary>
        public string Whole_LongTrend { get; set; }
        /// <summary>
        /// //div[@class='chart_base']/div[1]/div[3]
        ///  
        /// </summary>
        public decimal Tec_BeatHowMany { get; set; }
        /// <summary>
        /// //div[@class='chart_base']/div[2]/div[3]
        ///  
        /// </summary>
        public decimal Fund_BeatHowMany { get; set; }
        /// <summary>
        /// //div[@class='chart_base']/div[3]/div[3]
        ///  
        /// </summary>
        public decimal Message_BeatHowMany { get; set; }
        /// <summary>
        /// //div[@class='chart_base']/div[4]/div[3]
        ///  
        /// </summary>
        public decimal Section_BeatHowMany { get; set; }
        /// <summary>
        /// //div[@class='chart_base']/div[5]/div[3]
        ///  
        /// </summary>
        public decimal Basic_BeatHowMany { get; set; }
        #endregion

        #region 涨幅
        public decimal? Day1 { get; set; }
        public decimal? Day2 { get; set; }
        public decimal? Day3 { get; set; }
        public decimal? Day4 { get; set; }
        public decimal? Day5 { get; set; }
        public decimal? Day6 { get; set; }
        public decimal? Day7 { get; set; }
        public decimal? Day8 { get; set; }
        public decimal? Day9 { get; set; }
        public decimal? Day10 { get; set; }
        public decimal? Day11 { get; set; }
        public decimal? Day12 { get; set; }
        public decimal? Day13 { get; set; }
        public decimal? Day14 { get; set; }
        public decimal? Day15 { get; set; }
        public decimal? Day16 { get; set; } 
        #endregion

        public DateTime? ModifiedTime { get; set; }
    }
}
