using System;
using System.Collections.Generic;
using System.Text;

namespace PF.Core.Dto {
    public class StockAndBaseinfo {
        public int Id { set; get; }
        /// <summary>
        /// 股票名称
        /// </summary>
        public string StockName { get; set; }
        /// <summary>
        /// 股票代码
        /// </summary>
        public string StockNumber { get; set; }

        /// <summary>
        /// //div[@id='nav_funds']/div[1]/div[3]/div[4]/div[1]
        ///  燃气水务行业， 近5日资金总体呈流出状态，请投资者谨慎关注该行业的个股
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
        /// 更新标记
        /// </summary>
        public string UpdateTimeFlag { get; set; }

        public string Conception { get; set; }

        /// <summary>
        /// 是否分析过
        /// </summary>
        //public int Handled { get; set; }

        /// <summary>
        /// 诊断时间
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? CreateDate { get; set; } = DateTime.Now;

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModifiedTime { get; set; }

        #region 整体
        /// <summary>
        /// 综合诊断
        /// </summary>
        public decimal Whole_DiagnosisScore { get; set; }
        /// <summary>
        /// 排名比例
        /// </summary>
        public decimal Whole_BeatHowMany { get; set; }
        /// <summary>
        /// 买入等级
        /// </summary>
        //public string Whole_BuyLevel { get; set; }

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


        ///// <summary>
        ///// 主力机构数量
        ///// </summary>
        //public decimal? MainForceCount { get; set; }


        ///// <summary>
        ///// 主力机构数量
        ///// </summary>
        //public decimal? MainForceHowMany { get; set; }
        ///// <summary>
        ///// 持仓量A股占比
        ///// </summary>
        //public decimal? MainForcePercentage { get; set; }

        /// <summary>
        /// 波段
        /// /html[1]/body[1]/div[1]/div[6]/div[1]/div[1]/div[3]/ul[1]/li[4]/p[1]
        /// </summary>
        //public string Whole_BandTrend { get; set; }

        ///// <summary>
        ///// 高级诊股链接/这里要trim还要提取一下
        /////  /html[1]/body[1]/div[1]/div[6]/div[1]/div[2]/div[1]/span[1]
        ///// </summary>
        //public string AdvancedDiagnosisLink { get; set; }


        /// <summary>
        /// Level2Suggest //需要trim
        ///  //div[@class='fl stockintro']/strong/text()
        /// </summary>
        /// owen
        //public string Whole_Level2Suggest { get; set; }


        ///// <summary>
        ///// 
        /////  排名
        ///// </summary>
        //public int FundRank { get; set; }

        ///// <summary>
        ///// 平均成本
        /////  //p[@class='cnt showlevel2 hide']/strong
        /////  去掉元
        ///// </summary>
        //public decimal Whole_AverageCost { get; set; }


        /// <summary>
        /// //p[@class='cnt showlevel2 hide']/text()[2]
        ///  text被strong分割了，这里用text(2)
        /// </summary>
        /// owen
        //public string Whole_DescriptionLevel2 { get; set; }


        /// <summary>
        /// //p[@class='cnt showlevel1 hide']/text()[2]
        ///  
        /// </summary>
        /// owen
        //public string Whole_DescriptionLevel1 { get; set; }


        ///// <summary>
        ///// //div[@class='chart_base']/div[1]/div[3]
        /////  
        ///// </summary>
        //public decimal Whole_TecScore { get; set; }


        ///// <summary>
        ///// //div[@class='chart_base']/div[2]/div[3]
        /////  
        ///// </summary>
        //public decimal Whole_FundScore { get; set; }


        ///// <summary>
        ///// //div[@class='chart_base']/div[3]/div[3]
        /////  
        ///// </summary>
        //public decimal Whole_MessageScore { get; set; }


        ///// <summary>
        ///// //div[@class='chart_base']/div[4]/div[3]
        /////  
        ///// </summary>
        //public decimal Whole_SectionScore { get; set; }


        ///// <summary>
        ///// //div[@class='chart_base']/div[5]/div[3]
        /////  
        ///// </summary>
        //public decimal Whole_BasicScore { get; set; }


        #endregion

        #region 技术面

        /// <summary>
        /// 技术打败多少
        ///  //div[@id='nav_technical']/div[1]/div[1]/p[1]/span[@class='gray']
        /// </summary>
        public decimal Tec_BeatHowMany { get; set; }

        ///// <summary>
        ///// 技术概述
        /////  //div[@id='nav_technical']/div[1]/div[1]/p[2]
        ///// </summary>
        //public string Tec_OverViewDescription { get; set; }

        ///// <summary>
        ///// //div[@id='nav_technical']/div[1]/div[3]/div[1]/div[1]/strong
        /////  去掉元
        ///// </summary>
        //public decimal Tec_AvgCost { get; set; }

        ///// <summary>
        ///// //div[@id='nav_technical']/div[1]/div[3]/div[1]/div[1]
        /////  近期的平均成本为<strong>23.86元</strong>，股价在成本上方运行。
        ///// </summary>
        //public string Tec_Cost_Description { get; set; }

        ///// <summary>
        ///// //div[@id='nav_technical']/div[1]/div[3]/div[2]/div[1]
        /////  过去10个交易日，该股走势明显跑赢大盘，明显跑赢行业平均水平。
        ///// </summary>
        //public string Tec_Market_Performance { get; set; }

        ///// <summary>
        ///// //div[@id='nav_technical']/div[1]/div[3]/div[3]/div[1]
        /////  该股压力位为23.09元，支撑位为21.50元。
        ///// </summary>
        //public decimal Tec_SupportPrice { get; set; }

        ///// <summary>
        ///// //div[@id='nav_technical']/div[1]/div[3]/div[3]/div[1]
        /////  该股压力位为23.09元，支撑位为21.50元。
        ///// </summary>
        //public decimal Tec_PressurePrice { get; set; }

        ///// <summary>
        ///// 多空趋势
        /////  //div[@id='nav_technical']/div[1]/div[3]/div[4]/div[1]
        /////  多头行情中，并且有加速上涨趋势。
        ///// </summary>
        //public string Tec_BullOrBear { get; set; }
        #endregion

        //#region 资金
        /// <summary>
        /// //div[@id='nav_funds']/div[1]/div[1]/p[1]/span[@class='gray']
        ///  资金面打败了86%的股票
        /// </summary>
        public decimal Fund_BeatHowMany { get; set; }

        ///// <summary>
        ///// //div[@id='nav_funds']/div[1]/div[1]/p[2]
        /////  近5日内该股资金流入较多。据统计，近10日内主力筹码较分散，呈低度控盘状态。
        ///// </summary>
        //public string Fund_Level2_Description { get; set; }

        ///// <summary>
        ///// //div[@id='nav_funds']/div[1]/div[1]/p[3]
        /////  近5日内该股资金总体呈流入状态。据统计，近10日内主力筹码较分散，呈低度控盘状态。
        ///// </summary>
        //public string Fund_Level1_Description { get; set; }

        ///// <summary>
        ///// //div[@id='nav_funds']/div[1]/div[3]/div[1]/div[1]
        /////  近5日内该股资金流入较多，远高于行业平均水平，5日共流入2430.09万元。
        ///// </summary>
        //public string Fund_Level2_GoWhere { get; set; }

        ///// <summary>
        ///// //div[@id='nav_funds']/div[1]/div[3]/div[2]/div[1]
        /////  近5日内该股资金总体呈流入状态，高于行业平均水平，5日共流入3366.92万元。
        ///// </summary>
        //public string Fund_Level1_GoWhere { get; set; }

        ///// <summary>
        ///// //div[@id='nav_funds']/div[1]/div[3]/div[3]/div[2]/div[1]/ul/li[contains(@class, 'current')]
        /////  没有控盘
        ///// </summary>
        //public string Fund_MainForceControl { get; set; }

        ///// <summary>
        ///// //div[@id='nav_funds']/div[1]/div[3]/div[4]/div[1]
        /////  燃气水务行业， 近5日资金总体呈流出状态，请投资者谨慎关注该行业的个股
        ///// </summary>
        //public string Fund_Section_Overview { get; set; }

        ///// <summary>
        ///// //div[@id='nav_funds']/div[1]/div[3]/div[4]/div[2]/div[@class='update_time']
        /////  更新时间：09月27日 16:09
        ///// </summary>
        //public string Fund_Section_MoneyFlow_UpdateTime { get; set; }

        ///// <summary>
        ///// //div[@id='nav_funds']/div[1]/div[3]/div[4]/div[2]/table/tr/td[2]
        /////  机构近日流向
        ///// </summary>
        //public decimal Fund_Institute_Today_MoneyFlow { get; set; }

        ///// <summary>
        ///// //div[@id='nav_funds']/div[1]/div[3]/div[4]/div[2]/table/tr/td[4]
        /////  5天流向
        ///// </summary>
        //public decimal Fund_Institute_5day_MoneyFlow { get; set; }

        ///// <summary>
        ///// //div[@id='nav_funds']/div[1]/div[3]/div[4]/div[2]/table/tr[2]/td[2]
        /////  
        ///// </summary>
        //public decimal Fund_Institute_10day_MoneyFlow { get; set; }

        ///// <summary>
        ///// //div[@id='nav_funds']/div[1]/div[3]/div[4]/div[2]/table/tr[2]/td[4]
        /////  
        ///// </summary>
        //public decimal Fund_Institute_20day_MoneyFlow { get; set; }

        ///// <summary>
        ///// //div[@id='nav_funds']/div[1]/div[3]/div[5]/div[1]
        /////  2019-06-30 与 2019-03-31 相比，主力机构持仓量减少11.13万股，其中基金数没有增加，主力净增仓5家，暂未发现中长期投资价值。
        /////  需要trim
        ///// </summary>
        //public string Fund_Institute_Position_Description { get; set; }

        /////// <summary>
        /////// //div[@id='nav_funds']/div[1]/div[3]/div[5]/div[1]
        ///////  2019-06-30 与 2019-03-31 相比，主力机构持仓量减少11.13万股，其中基金数没有增加，主力净增仓5家，暂未发现中长期投资价值。
        ///////  需要trim
        /////// </summary>
        ////public string Fund_Institute_FundCompanyNumber_Description { get; set; }

        /////// <summary>
        /////// //div[@id='nav_funds']/div[1]/div[3]/div[5]/div[1]
        ///////  2019-06-30 与 2019-03-31 相比，主力机构持仓量减少11.13万股，其中基金数没有增加，主力净增仓5家，暂未发现中长期投资价值。
        ///////  需要trim
        /////// </summary>
        ////public string Fund_Institute_MainForceNumber_Description { get; set; }

        /////// <summary>
        /////// //div[@id='nav_funds']/div[1]/div[3]/div[5]/div[1]
        ///////  2019-06-30 与 2019-03-31 相比，主力机构持仓量减少11.13万股，其中基金数没有增加，主力净增仓5家，暂未发现中长期投资价值。
        ///////  需要trim
        /////// </summary>
        ////public string Fund_Institute_Value_Description { get; set; }


        //#endregion

        //#region 消息面
        /// <summary>
        /// //div[@id='nav_message']/div[1]/div[1]/p[1]/span[@class='gray']
        ///  消息面打败了42%的股票
        /// </summary>
        public decimal Message_BeatHowMany { get; set; }

        ///// <summary>
        /////  //div[@id='nav_message']/div[1]/div[1]/p[2]
        /////  近期该股消息面总体多空平衡，没有较强的利好或利空趋势。
        ///// </summary>
        //public string Message_Description { get; set; }

        ///// <summary>
        ///// //div[@id='nav_message']/div/div[@class='nx_items']/div[3]/div[1]/table/tr[2]/td[3]
        /////  0条
        ///// </summary>
        //public decimal Message_BigGoodNews { get; set; }


        ///// <summary>
        /////  //div[@id='nav_message']/div/div[@class='nx_items']/div[3]/div[1]/table/tr[3]/td[3]
        /////  0条
        ///// </summary>
        //public decimal Message_BigBadNews { get; set; }


        //#endregion

        #region 行业

        /// <summary>
        /// //div[@id='nav_trade']/div[1]/div[1]/p[1]/span[@class='gray']
        ///  行业面打败了32%的股票
        /// </summary>
        public decimal Section_BeatHowMany { get; set; }

        /// <summary>
        /// //div[@id='nav_trade']/div[1]/div[1]/p[2]
        ///  近10日来该行业走势不明显，跑赢大盘。
        /// </summary>
        //public string Section_Description { get; set; }

        /// <summary>
        /// //td/a[@href='/600856']/../../td[1]
        ///  排名
        /// </summary>
        public decimal Section_Section_Rank { get; set; }

        /// <summary>
        /// //td/a[@href='/600856']/../../td[3]
        ///  一天涨幅
        /// </summary>
        public decimal Section_ThisStock_1DayUp { get; set; }

        /// <summary>
        /// //td/a[@href='/600856']/../../td[4]
        ///  5天涨幅
        /// </summary>
        public decimal Section_ThisStock_5DayUp { get; set; }
        #endregion


        #region 基本面
        /// <summary>
        /// //div[@id='nav_basic']/div[1]/div[1]/p[1]/span[@class='gray']
        ///  基本面打败了18%的股票
        /// </summary>
        public decimal Basic_BeatHowMany { get; set; }

        /// <summary>
        /// //div[@id='nav_basic']/div[1]/div[1]/p[2]
        ///  该公司运营状况尚可，暂时未获得多数机构的显著认同，后续可继续关注。
        /// </summary>
        //public string Basic_Description { get; set; }

        /// <summary>
        /// //div[@class='bd layout2 pr']/div[@class='situation']/ul
        /// <li class="li1 "></li>
        ////<li class="li2 "></li>
        ////<li class="li3 "></li>
        ////<li class="li4 "></li>
        ////<li class="li5 current"></li>
        /// </summary>
        //public string Basic_Institute_Ratings { get; set; }
        #endregion

        //public DateTime? Middle_RecentMiddleTrendBuyDate { get; set; }
        //public DateTime? Middle_RecentMiddleTrendSellDate { get; set; }
        //public DateTime? Middle_AccelerateDate { get; set; }
        //public DateTime? Middle_AccelerateSlowDownDate { get; set; }
        //public DateTime? Middle_DropSlowdownDate { get; set; }
        //public DateTime? Middle_ReboundDate { get; set; }

        //public DateTime? Short_BullishHintDate { get; set; }
        //public DateTime? Short_StrongHintDate { get; set; }
        //public DateTime? Short_BullishInARowDate { get; set; }
        //public DateTime? Short_UnilateralDate { get; set; }


        //public decimal? WholeCapital { get; set; }
        //public decimal? MarketValue { get; set; }
        //public decimal? MarketValueOfCirculation { get; set; }
        //public decimal? CirculationRatio { get; set; }
        //public decimal? PERatio { get; set; }
        //public decimal? StaticPERatio { get; set; }
        //public decimal? PriceEarningRatio { get; set; }

        //public string Q1_ReportTime { get; set; }
        //public decimal? Q1_NetProfit { get; set; }
        //public decimal? Q1_NetProfitYOY { get; set; }
        //public decimal? Q1_BusinessIncome { get; set; }
        //public decimal? Q1_BusinessIncomeYOY { get; set; }
        //public decimal? Q1_EarningPerShare { get; set; }
        //public decimal? Q1_NetAssetValuePerShare { get; set; }
        //public decimal? Q1_CashFlowPerShare { get; set; }
        //public decimal? Q1_ReturnOnEquity { get; set; }
        //public decimal? Q1_GrossProfitMargin { get; set; }

        //public string Q2_ReportTime { get; set; }
        //public decimal? Q2_NetProfit { get; set; }
        //public decimal? Q2_NetProfitYOY { get; set; }
        //public decimal? Q2_BusinessIncome { get; set; }
        //public decimal? Q2_BusinessIncomeYOY { get; set; }
        //public decimal? Q2_EarningPerShare { get; set; }
        //public decimal? Q2_NetAssetValuePerShare { get; set; }
        //public decimal? Q2_CashFlowPerShare { get; set; }
        //public decimal? Q2_ReturnOnEquity { get; set; }
        //public decimal? Q2_GrossProfitMargin { get; set; }

        //public string Q3_ReportTime { get; set; }
        //public decimal? Q3_NetProfit { get; set; }
        //public decimal? Q3_NetProfitYOY { get; set; }
        //public decimal? Q3_BusinessIncome { get; set; }
        //public decimal? Q3_BusinessIncomeYOY { get; set; }
        //public decimal? Q3_EarningPerShare { get; set; }
        //public decimal? Q3_NetAssetValuePerShare { get; set; }
        //public decimal? Q3_CashFlowPerShare { get; set; }
        //public decimal? Q3_ReturnOnEquity { get; set; }
        //public decimal? Q3_GrossProfitMargin { get; set; }

        //public string Q4_ReportTime { get; set; }
        //public decimal? Q4_NetProfit { get; set; }
        //public decimal? Q4_NetProfitYOY { get; set; }
        //public decimal? Q4_BusinessIncome { get; set; }
        //public decimal? Q4_BusinessIncomeYOY { get; set; }
        //public decimal? Q4_EarningPerShare { get; set; }
        //public decimal? Q4_NetAssetValuePerShare { get; set; }
        //public decimal? Q4_CashFlowPerShare { get; set; }
        //public decimal? Q4_ReturnOnEquity { get; set; }
        //public decimal? Q4_GrossProfitMargin { get; set; }
        public decimal? Day1Amplitude { get; set; }
        public decimal? Day2Amplitude { get; set; }
        public decimal? Day3Amplitude { get; set; }
        public decimal? Day4Amplitude { get; set; }
        public decimal? Day5Amplitude { get; set; }
        public decimal? Day6Amplitude { get; set; }
        public decimal? Day7Amplitude { get; set; }


        public decimal OpenPrice { set; get; }
        public decimal LastDayClosePrice { set; get; }
        public decimal CurrentPrice { set; get; }
        public decimal GainPercent { set; get; }
        public decimal LowGainPercent { set; get; }
        public decimal HighGainPercent { set; get; }
        public decimal HighestPrice { set; get; }
        public decimal LowestPrice { set; get; }
        public decimal BiddingBuyNo1Price { set; get; }
        public decimal BiddingSellNo1Price { set; get; }
        public decimal DealAmount { set; get; }
        public decimal DealMoney { set; get; }
        public DateTime ModifiedDate { set; get; }



        public string ProcessedPara { get; set; }
        public string LatestDayParas { get; set; }
        public DateTime ParaTime { set; get; }
        public decimal MaxAmplitude { get; set; }
        public decimal NDayAmplitude { get; set; }
        public decimal GapPercent { get; set; }
        public decimal GapPercentWithLow { get; set; }

        /// <summary>
        /// 七天之内最高的那天
        /// </summary>
        public int  HighDay { get; set; }


    }


    public class StockAndBaseinfoCompareByStockNumber : IEqualityComparer<StockAndBaseinfo> {
        public bool Equals(StockAndBaseinfo x, StockAndBaseinfo y) {
            if (x == null || y == null)
                return false;
            if (x.StockNumber == y.StockNumber)
                return true;
            else
                return false;
        }

        public int GetHashCode(StockAndBaseinfo obj) {
            if (obj == null)
                return 0;
            else
                return obj.StockNumber.GetHashCode();
        }
    }
}
