using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PF.Core.Entity {

    [SugarTable("stockbaseinfoes")]
    public class StockBaseInfo {
        public int Id { set; get; }

        /// <summary>
        /// 股票代码
        /// </summary>
        public string StockNumber { get; set; }
        /// <summary>
        /// 股票名称
        /// </summary>
        public string StockName { get; set; }

        public string TradePlace { get; set; }


        public DateTime? Middle_RecentMiddleTrendBuyDate { get; set; }
        public DateTime? Middle_RecentMiddleTrendSellDate { get; set; }

        public DateTime? Middle_AccelerateDate { get; set; }
        public DateTime? Middle_AccelerateSlowDownDate { get; set; }
        public DateTime? Middle_DropSlowdownDate { get; set; }
        public DateTime? Middle_ReboundDate { get; set; }

        public DateTime? Short_BullishHintDate { get; set; }
        public DateTime? Short_StrongHintDate { get; set; }
        public DateTime? Short_BullishInARowDate { get; set; }
        public DateTime? Short_UnilateralDate { get; set; }
        /// <summary>
        /// 排名比例
        /// </summary>
        public string BigOrSmall { get; set; }

        public string Conception { get; set; }
        public string Conception2 { get; set; }

        //是否爬过
        [DefaultValue(false)]
        public bool Done { get; set; }

        public DateTime? ModifiedTime { get; set; }



        public decimal? WholeCapital { get; set; }
        public decimal? MarketValue { get; set; }
        public decimal? MarketValueOfCirculation { get; set; }
        public decimal? CirculationRatio { get; set; }
        public decimal? PERatio { get; set; }
        public decimal? StaticPERatio { get; set; }
        public decimal? PriceEarningRatio { get; set; }

        public string Q1_ReportTime { get; set; }
        public decimal? Q1_NetProfit { get; set; }
        public decimal? Q1_NetProfitYOY { get; set; }
        public decimal? Q1_BusinessIncome { get; set; }
        public decimal? Q1_BusinessIncomeYOY { get; set; }
        public decimal? Q1_EarningPerShare { get; set; }
        public decimal? Q1_NetAssetValuePerShare { get; set; }
        public decimal? Q1_CashFlowPerShare { get; set; }
        public decimal? Q1_ReturnOnEquity { get; set; }
        public decimal? Q1_GrossProfitMargin { get; set; }

        public string Q2_ReportTime { get; set; }
        public decimal? Q2_NetProfit { get; set; }
        public decimal? Q2_NetProfitYOY { get; set; }
        public decimal? Q2_BusinessIncome { get; set; }
        public decimal? Q2_BusinessIncomeYOY { get; set; }
        public decimal? Q2_EarningPerShare { get; set; }
        public decimal? Q2_NetAssetValuePerShare { get; set; }
        public decimal? Q2_CashFlowPerShare { get; set; }
        public decimal? Q2_ReturnOnEquity { get; set; }
        public decimal? Q2_GrossProfitMargin { get; set; }

        public string Q3_ReportTime { get; set; }
        public decimal? Q3_NetProfit { get; set; }
        public decimal? Q3_NetProfitYOY { get; set; }
        public decimal? Q3_BusinessIncome { get; set; }
        public decimal? Q3_BusinessIncomeYOY { get; set; }
        public decimal? Q3_EarningPerShare { get; set; }
        public decimal? Q3_NetAssetValuePerShare { get; set; }
        public decimal? Q3_CashFlowPerShare { get; set; }
        public decimal? Q3_ReturnOnEquity { get; set; }
        public decimal? Q3_GrossProfitMargin { get; set; }

        public string Q4_ReportTime { get; set; }
        public decimal? Q4_NetProfit { get; set; }
        public decimal? Q4_NetProfitYOY { get; set; }
        public decimal? Q4_BusinessIncome { get; set; }
        public decimal? Q4_BusinessIncomeYOY { get; set; }
        public decimal? Q4_EarningPerShare { get; set; }
        public decimal? Q4_NetAssetValuePerShare { get; set; }
        public decimal? Q4_CashFlowPerShare { get; set; }
        public decimal? Q4_ReturnOnEquity { get; set; }
        public decimal? Q4_GrossProfitMargin { get; set; }
    }
}
