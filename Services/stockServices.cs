using IRepository;
using IServices;
using PF.Core.Dto;
using PF.Core.Entity;
using PF.Utils.Extensions;
using PF.Utils.Json;
using PF.Utils.Table;
using SqlSugar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;

namespace Services
{
    public class stockServices : BaseServices<Stock>, IstockServices
    {
        private readonly SqlSugarClient _sqlSugarClient;
        private readonly IstockRepository _repository;

        public stockServices(SqlSugarClient sqlSugarClient,IstockRepository repository) : base(repository)
        {
            this._sqlSugarClient = sqlSugarClient;
            _repository = repository;
        }

        public string PageListTest(PubParams.FavorTypeBootstrapParams bootstrap) {
            if (bootstrap.offset != 0) {
                bootstrap.offset = bootstrap.offset / bootstrap.limit + 1;
            }
            //var order = ExpressionExt.InitO<Sys_user>(bootstrap.sort);
            //var date = _sqlSugarClient.Queryable<DayRecord>().OrderBy(s => s.Date, OrderByType.Desc).First().Date;
          

            var query = _sqlSugarClient.Queryable<SelfSelect, StockBaseInfo>
                ((s, c) => new object[] {
                   JoinType.Left,s.StockNumber==c.StockNumber,
                 }).Where((s, c) => s.IsDel == 1)
                 .Select((s, c) => new {
                     SelfSelectId = s.SelfSelectId.ToString(),
                     s.StockNumber,
                     s.Section,
                     s.Operation,
                     c.StockName,
                     c.TradePlace,
                     s.Whole_BeatHowMany,
                     s.Tec_BeatHowMany,
                     s.Fund_BeatHowMany,
                     s.Message_BeatHowMany,
                     s.Section_BeatHowMany,
                     s.Basic_BeatHowMany,
                     s.Remark,
                     s.Cost,
                     s.Margin,
                     s.CurrentPrice,
                     s.HowMuch,
                     s.CreateDate,
                     s.ModifiedDate,
                     s.RecommendDate
                 }).MergeTable().OrderBy(s=>s.CreateDate, OrderByType.Desc);
            if (!bootstrap.datemin.IsEmpty() && !bootstrap.datemax.IsEmpty()) {
                query.Where(s => s.CreateDate > bootstrap.datemin.ToDateTimeB() && s.CreateDate <= bootstrap.datemax.ToDateTimeE());
            }
            if (bootstrap.FavorType != null) {
                query.Where(s => s.Operation == bootstrap.FavorType);
            }
            var list = query.ToList();
            return list.JilToJson();
        }

        
        public bool UpdateStocksTemp(PubParams.ProcedureBootstrapParams bootstrap) {

            string command = $"call `{bootstrap.Procedure}`('{bootstrap.datemin}', '{bootstrap.datemax}')";
            int res =_sqlSugarClient.Ado.ExecuteCommand(command);
            return res >=0;
        }

        public string FundPageList(PubParams.FavorTypeBootstrapParams bootstrap)
        {
            var query = _sqlSugarClient.Queryable<FundRealTimeInfo, SelfSelect>
               ((s, c) => new object[] {
                   JoinType.Left,s.Code==c.StockNumber,
                }).Where((s, c) => bootstrap.CheckOperationList.Contains(c.Operation) && c.IsStock==0).Select<ETFFund>();
            var temp = query.ToList();
            var list = temp.Where(s => bootstrap.SelectedDates.Any(d => d.Date == s.UpdateTime.Date)).ToList();

            return list.JilToJson();
        }
        public string PageList(PubParams.FavorTypeBootstrapParams bootstrap) {
            if (bootstrap.offset != 0) {
                bootstrap.offset = bootstrap.offset / bootstrap.limit + 1;
            }
            //var order = ExpressionExt.InitO<Sys_user>(bootstrap.sort);
            //var date = _sqlSugarClient.Queryable<DayRecord>().OrderBy(s => s.Date, OrderByType.Desc).First().Date;

            DayRecord dayRecord = null;
            var a = DateTime.Now.Hour;
            dayRecord = _sqlSugarClient.Queryable<DayRecord>().Where(s => s.InOrAfterTrade == "盘后").OrderBy(s => s.Id, OrderByType.Desc).First();
         
          

            var query = _sqlSugarClient.Queryable<SelfSelect, StockBaseInfo, Stock, TecParaRecord, RealtimeStockInfo>
                ((s, c, k,t,r) => new object[] {
                   JoinType.Left,s.StockNumber==c.StockNumber,
                   JoinType.Left,s.StockNumber==k.StockNumber,
                   JoinType.Left,s.StockNumber==t.StockNumber,
                   JoinType.Left,s.StockNumber==r.StockNumber,
                 }).Where((s, c, k, t, r) => s.IsDel == 1 && k.Date == dayRecord.Date && k.InOrAfterTrade == dayRecord.InOrAfterTrade)
                 .Select((s, c, k, t, r) => new {
                     SelfSelectId = s.SelfSelectId.ToString(),
                     s.StockNumber,
                     s.Section,
                     s.Operation,
                     s.Day1Profit,
                     s.Day2Profit,
                     s.Day3Profit,
                     s.Day4Profit,
                     s.Day5Profit,
                     s.Day6Profit,
                     s.Day7Profit,





                     c.StockName,
                     c.TradePlace,
                     c.Conception,
                     k.Date,
                     t.ProcessedPara,
                     ParaTime= t.CreatTime,

                     r.Day1Amplitude,
                      r.Day2Amplitude,
                      r.Day3Amplitude,
                      r.Day4Amplitude,
                      r.Day5Amplitude,
                      r.Day6Amplitude,
                      r.Day7Amplitude,
                     r.MaxAmplitude,
                     r.GainPercent,
                     NDayAmplitude=(r.CurrentPrice - r.Day5ClosePrice) * 100 / r.Day5ClosePrice,
                     GapPercent=(r.CurrentPrice - r.MaxClosePrice) * 100 / r.MaxClosePrice,

                     c. WholeCapital ,
                c.MarketValue ,
                c.MarketValueOfCirculation ,
                c.CirculationRatio ,
                c.PERatio ,
                c.StaticPERatio ,
                c.PriceEarningRatio ,


                    c.Middle_RecentMiddleTrendBuyDate,
                    c.Middle_RecentMiddleTrendSellDate,

                     c.Q1_ReportTime ,
                c.Q1_NetProfit ,
                c.Q1_NetProfitYOY ,
                c.Q1_BusinessIncome ,
                c.Q1_BusinessIncomeYOY ,
                c.Q1_EarningPerShare ,
                c.Q1_NetAssetValuePerShare ,
                c.Q1_CashFlowPerShare ,
                c.Q1_ReturnOnEquity ,
                c.Q1_GrossProfitMargin ,

                c.Q2_ReportTime ,
                c.Q2_NetProfit ,
                c.Q2_NetProfitYOY ,
                c.Q2_BusinessIncome ,
                c.Q2_BusinessIncomeYOY ,
                c.Q2_EarningPerShare ,
                c.Q2_NetAssetValuePerShare ,
                c.Q2_CashFlowPerShare ,
                c.Q2_ReturnOnEquity ,
                c.Q2_GrossProfitMargin ,

                c.Q3_ReportTime ,
                c.Q3_NetProfit ,
                c.Q3_NetProfitYOY ,
                c.Q3_BusinessIncome ,
                c.Q3_BusinessIncomeYOY ,
                c.Q3_EarningPerShare ,
                c.Q3_NetAssetValuePerShare ,
                c.Q3_CashFlowPerShare ,
                c.Q3_ReturnOnEquity ,
                c.Q3_GrossProfitMargin ,

                c.Q4_ReportTime ,
                c.Q4_NetProfit ,
                c.Q4_NetProfitYOY ,
                c.Q4_BusinessIncome ,
                c.Q4_BusinessIncomeYOY ,
                c.Q4_EarningPerShare ,
                c.Q4_NetAssetValuePerShare ,
                c.Q4_CashFlowPerShare ,
                c.Q4_ReturnOnEquity ,
                c.Q4_GrossProfitMargin ,
                     k.Whole_BeatHowMany,
                     k.Tec_BeatHowMany,
                     k.Fund_BeatHowMany,
                     k.Message_BeatHowMany,
                     k.Section_BeatHowMany,
                     k.Basic_BeatHowMany,

                     k.Section_Section_Rank,
                     k.Section_ThisStock_5DayUp,
                     k.Section_ThisStock_1DayUp,
                     k.UpdateTimeFlag,
                     k.Whole_ShortTrend,
                     k.Whole_MiddleTrend,
                     k.Whole_LongTrend,
                     k.Whole_Level2Suggest,
                     k.Fund_Level2_Description,
                     k.Whole_DescriptionLevel2,
                     k.Fund_Section_Overview,
                     k.Fund_Level2_GoWhere,
                     k.Fund_Institute_Position_Description,

                     s.Remark,
                     s.Cost,
                     s.Margin,
                     s.CurrentPrice,
                     s.HowMuch,
                     s.CreateDate,
                     s.ModifiedDate,
                     s.RecommendDate
                 }).MergeTable();
            if (!bootstrap.datemin.IsEmpty() && !bootstrap.datemax.IsEmpty()) {
                query.Where(s => s.CreateDate > bootstrap.datemin.ToDateTimeB() && s.CreateDate <= bootstrap.datemax.ToDateTimeE());
            }
            if (bootstrap.CheckOperationList != null) {
                query.Where(s => bootstrap.CheckOperationList.Contains(s.Operation));
            }

            if (bootstrap.Stock != null) {
                query.Where(s => s.StockName.Contains(bootstrap.Stock)|| s.StockNumber.Contains(bootstrap.Stock));
            }
            query = query.OrderBy(s => s.GainPercent,OrderByType.Desc).PartitionBy(s => s.StockNumber).Take(1000000000);
            var list = query.ToList();
            return list.JilToJson();
        }

        public string PageListSimple(PubParams.FavorTypeBootstrapParams bootstrap) {
            if (bootstrap.offset != 0) {
                bootstrap.offset = bootstrap.offset / bootstrap.limit + 1;
            }
         

            var query = _sqlSugarClient.Queryable<SelfSelect, StockBaseInfo, RealtimeStockInfo>
                ((s, c, r) => new object[] {
                   JoinType.Left,s.StockNumber==c.StockNumber,
                   JoinType.Left,s.StockNumber==r.StockNumber,
                 }).Where((s, c, r) => s.IsDel == 1 && s.IsStock== bootstrap.IsStock )
                 .Select((s, c, r) => new {
                     SelfSelectId = s.SelfSelectId.ToString(),
                     s.StockNumber,
                     s.Section,
                     s.Operation,
                     s.Day1Profit,
                     s.Day2Profit,
                     s.Day3Profit,
                     s.Day4Profit,
                     s.Day5Profit,
                     s.Day6Profit,
                     s.Day7Profit,
                     s.Type,




                     c.StockName,
                     c.TradePlace,
                     c.Conception,
                     Date = DateTime.Now,
                     ProcessedPara = "",
                     ParaTime = DateTime.Now,

                     r.Day1Amplitude,
                     r.Day2Amplitude,
                     r.Day3Amplitude,
                     r.Day4Amplitude,
                     r.Day5Amplitude,
                     r.Day6Amplitude,
                     r.Day7Amplitude,
                     r.MaxAmplitude,
                     GainPercent= r.GainPercent==null?0 : r.GainPercent,
                     NDayAmplitude = (r.CurrentPrice - r.Day5ClosePrice) * 100 / r.Day5ClosePrice,
                     GapPercent = (r.CurrentPrice - r.MaxClosePrice) * 100 / r.MaxClosePrice,

                     c.WholeCapital,
                     c.MarketValue,
                     c.MarketValueOfCirculation,
                     c.CirculationRatio,
                     c.PERatio,
                     c.StaticPERatio,
                     c.PriceEarningRatio,


                     c.Middle_RecentMiddleTrendBuyDate,
                     c.Middle_RecentMiddleTrendSellDate,

                     c.Q1_ReportTime,
                     c.Q1_NetProfit,
                     c.Q1_NetProfitYOY,
                     c.Q1_BusinessIncome,
                     c.Q1_BusinessIncomeYOY,
                     c.Q1_EarningPerShare,
                     c.Q1_NetAssetValuePerShare,
                     c.Q1_CashFlowPerShare,
                     c.Q1_ReturnOnEquity,
                     c.Q1_GrossProfitMargin,

                     c.Q2_ReportTime,
                     c.Q2_NetProfit,
                     c.Q2_NetProfitYOY,
                     c.Q2_BusinessIncome,
                     c.Q2_BusinessIncomeYOY,
                     c.Q2_EarningPerShare,
                     c.Q2_NetAssetValuePerShare,
                     c.Q2_CashFlowPerShare,
                     c.Q2_ReturnOnEquity,
                     c.Q2_GrossProfitMargin,

                     c.Q3_ReportTime,
                     c.Q3_NetProfit,
                     c.Q3_NetProfitYOY,
                     c.Q3_BusinessIncome,
                     c.Q3_BusinessIncomeYOY,
                     c.Q3_EarningPerShare,
                     c.Q3_NetAssetValuePerShare,
                     c.Q3_CashFlowPerShare,
                     c.Q3_ReturnOnEquity,
                     c.Q3_GrossProfitMargin,

                     c.Q4_ReportTime,
                     c.Q4_NetProfit,
                     c.Q4_NetProfitYOY,
                     c.Q4_BusinessIncome,
                     c.Q4_BusinessIncomeYOY,
                     c.Q4_EarningPerShare,
                     c.Q4_NetAssetValuePerShare,
                     c.Q4_CashFlowPerShare,
                     c.Q4_ReturnOnEquity,
                     c.Q4_GrossProfitMargin,
                     Whole_BeatHowMany = 100,
                     Tec_BeatHowMany = 100,
                     Fund_BeatHowMany = 100,
                     Message_BeatHowMany = 100,
                     Section_BeatHowMany = 100,
                     Basic_BeatHowMany = 100,

                     Section_Section_Rank = 100,
                     Section_ThisStock_5DayUp = 0,
                     Section_ThisStock_1DayUp = 0,
                     UpdateTimeFlag = "",
                    Whole_ShortTrend = "",
                     Whole_MiddleTrend = "",
                     Whole_LongTrend = "",
                     Whole_Level2Suggest = "",
                     Fund_Level2_Description = "",
                     Whole_DescriptionLevel2 = "",
                     Fund_Section_Overview = "",
                     Fund_Level2_GoWhere = "",
                     Fund_Institute_Position_Description = "",

                     s.Remark,
                     s.Cost,
                     s.Margin,
                     s.CurrentPrice,
                     s.HowMuch,
                     s.CreateDate,
                     s.ModifiedDate,
                     s.RecommendDate
                 }).MergeTable();
            if (!bootstrap.datemin.IsEmpty() && !bootstrap.datemax.IsEmpty()) {
                query.Where(s => s.CreateDate > bootstrap.datemin.ToDateTimeB() && s.CreateDate <= bootstrap.datemax.ToDateTimeE());
            }
            if (bootstrap.CheckOperationList != null) {
                query.Where(s => bootstrap.CheckOperationList.Contains(s.Operation));
            }

            //query.Where(s => bootstrap.SelectedDates.Any(d => d.Date == s.CreateDate));

            if (bootstrap.Stock != null) {
                query.Where(s => s.StockName.Contains(bootstrap.Stock) || s.StockNumber.Contains(bootstrap.Stock));
            }
            query = query.OrderBy(s => s.GapPercent, OrderByType.Desc).PartitionBy(s => s.StockNumber).Take(1000000000);
            var list = query.ToList().Where(s => bootstrap.SelectedDates.Any(d => d.Date == s.CreateDate.Value.Date)).ToList();
            return list.JilToJson();
        }

        public string GetList(PubParams.FavorTypeBootstrapParams bootstrap) {
            var totalNumber = 0;
            if (bootstrap.offset != 0) {
                bootstrap.offset = bootstrap.offset / bootstrap.limit + 1;
            }
            var query = _sqlSugarClient.Queryable<Stock>().Where(s => s.StockNumber == bootstrap.StockNumber).OrderBy(s => s.Date,OrderByType.Desc).Take(30);

            if (!bootstrap.search.IsEmpty()) {
                query.Where((s) => s.Whole_DescriptionLevel2 == bootstrap.search 
                || s.Whole_Level2Suggest == bootstrap.search
                    || s.Tec_Cost_Description == bootstrap.search
                );
            }
            var list = query.ToPageList(bootstrap.offset, bootstrap.limit, ref totalNumber);
            return list.JilToJson();
        }


        public string HistoryChart3(PubParams.StrategyBootstrapParams bootstrap) {
            if (bootstrap.StrategyId == null) {
                return null;
            }
            var command = _sqlSugarClient.Queryable<Sys_dict>().Where((s) => s.IsDel == 1 && s.DictId== SqlFunc.ToInt64(bootstrap.StrategyId));
            string sql = command.First().SQL;
            sql = sql.Replace("#begin", $"and date>='{bootstrap.datemin}'").Replace("#end", $"and date<='{bootstrap.datemax}'");
            switch (bootstrap.InOrAfterTrade) {
                case "盘中":
                    sql= sql.Replace("#盘", "and InOrAfterTrade='盘中' ");
                    break;
                case "盘后":
                    sql = sql.Replace("#盘", "and InOrAfterTrade='盘后' ");
                    break;
                case "盘中+盘后":
                    sql = sql.Replace("#盘", "");
                    break;
            }
            if (bootstrap.Section!=null) {
                sql = sql.Replace("#行业", $"and Section REGEXP  '.*{bootstrap.Section}.*' ");
            }
            else {
                sql = sql.Replace("#行业", "");
            }
            List<Stock> list = new List<Stock>();
            try {
                list = _sqlSugarClient.Ado.SqlQuery<Stock>(sql);
            }
            catch (Exception) {
                sql = @"SQL错误
                    " + sql;
            }
            return new{sql,list}.JilToJson();
        }


        public string HistoryChartALlStrategy(PubParams.StrategyBootstrapParams bootstrap) {
          if (bootstrap.StrategyIdList == null) {
                return null;
            }
            List<Stock> list = new List<Stock>();
            string sql = "";
            foreach (var item in bootstrap.StrategyIdList) {
                var command = _sqlSugarClient.Queryable<Sys_dict>().Where((s) => s.IsDel == 1 && s.DictId == SqlFunc.ToInt64(item));
                sql = command.First().SQL;
                sql = sql.Replace("#begin", $"and date>='{bootstrap.datemin}'").Replace("#end", $"and date<='{bootstrap.datemax}'");
                switch (bootstrap.InOrAfterTrade) {
                    case "盘中":
                        sql = sql.Replace("#盘", "and InOrAfterTrade='盘中' ");
                        break;
                    case "盘后":
                        sql = sql.Replace("#盘", "and InOrAfterTrade='盘后' ");
                        break;
                    case "盘中+盘后":
                        sql = sql.Replace("#盘", "");
                        break;
                }
                if (bootstrap.CheckSectionList != null) {
                    string sectionList = "";
                    foreach (var section in bootstrap.CheckSectionList) {
                        var temp = section.Split('-')[0];
                        sectionList += $".*{temp}.*|";
                    }
                    sectionList = sectionList.Substring(0, sectionList.Length - 1);
                    sql = sql.Replace("#行业", $"and Section REGEXP  '{sectionList}' ");
                }
                else {
                    sql = sql.Replace("#行业", "");
                }
                try {
                   var listTemp = _sqlSugarClient.Ado.SqlQuery<Stock>(sql);
                   list.AddRange(listTemp);
                }
                catch (Exception) {
                    sql = @"SQL错误
                    " + sql;
                }
            }
            return new { sql, list }.JilToJson();
        }


        public string HistoryChartBySql(PubParams.StrategyBootstrapParams bootstrap) {
            if (bootstrap.StrategyIdList == null) {
                return null;
            }
            List<StockAndBaseinfo> list = new List<StockAndBaseinfo>();
            string sql = "";
            sql = bootstrap.sql;
                sql = sql.Replace("#begin", $"'{bootstrap.datemin}'").Replace("#end", $"'{bootstrap.datemax}'");
                switch (bootstrap.InOrAfterTrade) {
                    case "盘中":
                        sql = sql.Replace("#盘", "and InOrAfterTrade='盘中' ");
                        break;
                    case "盘后":
                        sql = sql.Replace("#盘", "and InOrAfterTrade='盘后' ");
                        break;
                    case "盘中+盘后":
                        sql = sql.Replace("#盘", "");
                        break;
                }
            if (sql.Contains("时间标定")) {
                sql = sql.Replace("#时间标定", $"'{bootstrap.DateWantSee}'");
            }
            if (bootstrap.CheckSectionList != null) {
                    string sectionList = "";
                    foreach (var section in bootstrap.CheckSectionList) {
                        var temp = section.Split('-')[0];
                        sectionList += $".*{temp}.*|";
                    }
                    sectionList = sectionList.Substring(0, sectionList.Length - 1);
                    sql = sql.Replace("#行业", $"and Section REGEXP  '{sectionList}' ");
                }
                else {
                    sql = sql.Replace("#行业", "");
                }
                try {
                    list = _sqlSugarClient.Ado.SqlQuery<StockAndBaseinfo>(sql);
                }
                catch (Exception) {
                    sql = @"SQL错误
                    " + sql;
                }
            
            return new { sql, list }.JilToJson();
        }


        public string GetStocksInFund(List<string> stocksInFund)
        {
            //List<StockAndBaseinfo> list = new List<StockAndBaseinfo>();
            //foreach (var stockCode in stocksInFund)
            //{
            //   var entity= _sqlSugarClient.Queryable<StockBaseInfo>().Where(s => s.StockNumber == stockCode).First();

            //    var query = _sqlSugarClient.Queryable<StockBaseInfo, RealtimeStockInfo>
            //  ((s, c) => new object[] {
            //       JoinType.Left,s.StockNumber==c.StockNumber,
            //   }).Where(s => stocksInFund.Contains(s.StockNumber)).Select<StockBaseInfo>();
            //    if (entity!=null)
            //    {
            //        StockAndBaseinfo temp = new StockAndBaseinfo();
            //        temp.StockNumber = entity.StockNumber;
            //        temp.StockName = entity.StockName;
            //        temp.TradePlace = entity.TradePlace;
            //        temp.Conception = entity.Conception;
            //        temp.ModifiedTime = DateTime.Now;
            //        list.Add(temp);
            //    }
            //}

            var query = _sqlSugarClient.Queryable<StockBaseInfo, RealtimeStockInfo>
             ((s, c) => new object[] {
                   JoinType.Left,s.StockNumber==c.StockNumber,
              }).Where(s => stocksInFund.Contains(s.StockNumber)).Select<StockAndBaseinfo>().ToList(); ;
            //.OrderBy(s => s.GainPercent, OrderByType.Desc)
            string sql = "";
            var list = query.OrderByDescending(s => s.GainPercent).ToList();
            return new { sql, list }.JilToJson();
        }
        public string FundBySql(PubParams.StrategyBootstrapParams bootstrap)
        {
            if (bootstrap.StrategyIdList == null)
            {
                return null;
            }
            List<ETFFund> list = new List<ETFFund>();
            string sql = "";
            sql = bootstrap.sql;
            sql = sql.Replace("#begin", $"'{bootstrap.datemin}'").Replace("#end", $"'{bootstrap.datemax}'");
            switch (bootstrap.InOrAfterTrade)
            {
                case "盘中":
                    sql = sql.Replace("#盘", "and InOrAfterTrade='盘中' ");
                    break;
                case "盘后":
                    sql = sql.Replace("#盘", "and InOrAfterTrade='盘后' ");
                    break;
                case "盘中+盘后":
                    sql = sql.Replace("#盘", "");
                    break;
            }
            if (sql.Contains("时间标定"))
            {
                sql = sql.Replace("#时间标定", $"'{bootstrap.DateWantSee}'");
            }
            if (bootstrap.CheckSectionList != null)
            {
                string sectionList = "";
                foreach (var section in bootstrap.CheckSectionList)
                {
                    var temp = section.Split('-')[0];
                    sectionList += $".*{temp}.*|";
                }
                sectionList = sectionList.Substring(0, sectionList.Length - 1);
                sql = sql.Replace("#行业", $"and Section REGEXP  '{sectionList}' ");
            }
            else
            {
                sql = sql.Replace("#行业", "");
            }
            try
            {
                list = _sqlSugarClient.Ado.SqlQuery<ETFFund>(sql);
            }
            catch (Exception)
            {
                sql = @"SQL错误
                    " + sql;
            }

            return new { sql, list }.JilToJson();
        }


        public string HotFundBySql(PubParams.StrategyBootstrapParams bootstrap) {
            List<ETFFund> list = new List<ETFFund>();
            string sql = $@"select *
                    from
                    fundhistory t1
                    left
                    join fundrealtimeinfoes t2 on t1.Code = t2.Code
                    WHERE
                    t1.date >= '{bootstrap.datemin}' and t1.date <='{bootstrap.datemin}'
                    And t1.hot = '热门'
                    ORDER BY t1.Amplitude desc";
            try {
                list = _sqlSugarClient.Ado.SqlQuery<ETFFund>(sql);
            }
            catch (Exception) {
                sql = @"SQL错误
                    " + sql;
            }

            return new { sql, list }.JilToJson();
        }

        public class ChartdData {
            public int Zcount { get; set; }
            public int? Ydistance { get; set; }
            public string Xtime { get; set; }
        }


        public string IntervalData(string imei ,int interval , string timeBegin,string timeEnd) {
           string sql = "";
           sql = $@"SELECT time, Distance,COUNT( * ) as Count,sum(Distance)/count(Distance) AS AvgDistance 
                FROM
	                (
	                SELECT Distance,
		                DATE_FORMAT(
			                concat( date( CreateDate ), ' ', HOUR ( CreateDate ), ':', floor( MINUTE ( CreateDate ) / {interval} ) * {interval}),
			                '%Y-%m-%d %H:%i:00' 
		                ) AS time 
	                FROM dev_payload where IMEI='{imei}' AND CreateDate BETWEEN '{timeBegin}' AND '{timeEnd}'
	                ) a 
                GROUP BY DATE_FORMAT( time, '%Y-%m-%d %H:%i:00' ) 
                ORDER BY time;
                ";
            var list = _sqlSugarClient.Ado.SqlQuery<GroupQueryResult>(sql);
            return list.JilToJson();
        }

        public string PieChart(string imei,int empty, int full,string timeBegin, string timeEnd) {
            string sql = "";
            sql= $@"SELECT count(*)  FROM dev_payload where Distance>={empty} AND IMEI='{imei}' AND CreateDate BETWEEN '{timeBegin}' AND '{timeEnd}' ";
            var emptyData = _sqlSugarClient.Ado.SqlQuery<int>(sql);
            sql = $@"SELECT count(*)  FROM dev_payload where IMEI='{imei}' AND Distance BETWEEN {full} AND {empty}  AND CreateDate BETWEEN '{timeBegin}' AND '{timeEnd}' ";
            var okData = _sqlSugarClient.Ado.SqlQuery<int>(sql);
            sql = $@"SELECT count(*)  FROM dev_payload where Distance<={full} AND IMEI='{imei}' AND CreateDate BETWEEN '{timeBegin}' AND '{timeEnd}' ";
            var fullData = _sqlSugarClient.Ado.SqlQuery<int>(sql);
            var res = new Count {
                empty = emptyData[0],
                ok = okData[0],
                full = fullData[0]
            };
            return  res.JilToJson();
        }

        public string GetDateList() {
            var list = _sqlSugarClient.Queryable<DayRecord>().OrderBy(s => s.Date,OrderByType.Desc).GroupBy(s => s.Date).Select(s => new { s.Date }).Take(30).ToList();
            return list.JilToJson();
        }


        //
        public class Count  {
            public int empty { get; set; }
            public int ok { get; set; }
            public int full { get; set; }
        }



        public class  GroupQueryResult{
            public string time { get; set; }
            public int Distance { get; set; }
            public int Count { get; set; }
            public int AvgDistance { get; set; }
        }




        public string StockPageList(PubParams.FavorTypeBootstrapParams bootstrap) {
            var totalNumber = 0;
            if (bootstrap.offset != 0) {
                bootstrap.offset = bootstrap.offset / bootstrap.limit + 1;
            }
            //var order = ExpressionExt.InitO<Sys_user>(bootstrap.sort);

            var query = _sqlSugarClient.Queryable<Stock>().Where(s => s.StockName == bootstrap.StockNumber || s.StockNumber == bootstrap.StockNumber)
              .OrderBy(s=>s.Date,OrderByType.Desc);
            //if (!bootstrap.search.IsEmpty()) {
            //    query.Where((s) => s.UserName == bootstrap.search || s.UserNickname == bootstrap.search);
            //}
            if (!bootstrap.datemin.IsEmpty() && !bootstrap.datemax.IsEmpty()) {
                query.Where(s => s.CreateDate > bootstrap.datemin.ToDateTimeB() && s.CreateDate <= bootstrap.datemax.ToDateTimeE());
            }
            //if (bootstrap.order.Equals("desc", StringComparison.OrdinalIgnoreCase)) {
            //    query.OrderBy($"MergeTable.{bootstrap.sort} desc");
            //}
            //else {
            //    query.OrderBy($"MergeTable.{bootstrap.sort} asc");
            //}
            var list = query.ToPageList(bootstrap.offset, bootstrap.limit, ref totalNumber);

            return Bootstrap.GridData(list, totalNumber).JilToJson();
        }




        public string StockTecParaPageList(PubParams.FavorTypeBootstrapParams bootstrap) {

            if (bootstrap.StockNumber=="") {
                return null;
            }
            var totalNumber = 0;
            if (bootstrap.offset != 0) {
                bootstrap.offset = bootstrap.offset / bootstrap.limit + 1;
            }

            var query = _sqlSugarClient.Queryable<TecParaRecordHis, TradeDayRecord>
               ((s, c) => new object[] {
                   JoinType.Left,s.Date==c.Date
                }).Where(s => s.StockNumber == bootstrap.StockNumber );


            if (bootstrap.WeekInfo== "WeekInfo")
            {
                query = query.Where((s, c) => c.LastDayOfWeek == 1);
            }
            query= query.OrderBy(s => s.Date, OrderByType.Desc);
            //if (!bootstrap.search.IsEmpty()) {

            //    query.Where((s) => s.UserName == bootstrap.search || s.UserNickname == bootstrap.search);
            //}
            if (!bootstrap.datemin.IsEmpty() && !bootstrap.datemax.IsEmpty()) {
                query.Where(s => s.CreatTime > bootstrap.datemin.ToDateTimeB() && s.CreatTime <= bootstrap.datemax.ToDateTimeE());
            }
            //if (bootstrap.order.Equals("desc", StringComparison.OrdinalIgnoreCase)) {
            //    query.OrderBy($"MergeTable.{bootstrap.sort} desc");
            //}
            //else {
            //    query.OrderBy($"MergeTable.{bootstrap.sort} asc");
            //}

            query = query.OrderBy(s=>s.CreatTime,OrderByType.Desc);
            var list = query.ToPageList(bootstrap.offset, bootstrap.limit, ref totalNumber);
            return Bootstrap.GridData(list, totalNumber).JilToJson();
        }

    }
}