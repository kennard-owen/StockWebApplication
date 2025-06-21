using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace PF.Core.Entity {
    [SugarTable("realtimestockinfoes")]
    public class RealtimeStockInfo {
        public int Id { set; get; }
        public string StockNumber { set; get; }
        public string TradePlace { set; get; }
        public string StockName { set; get; }
        public decimal OpenPrice { set; get; }
        public decimal LastDayClosePrice { set; get; }
        public decimal CurrentPrice { set; get; }
        public decimal GainPercent { set; get; }
        public decimal Lenth { set; get; }
        public decimal LowGainPercent { set; get; }
        public decimal HighGainPercent { set; get; }


        public string LatestDayParas { set; get; }
        public string LatestDayParas2Days { set; get; }
        //public string LatestDayParas7Days { set; get; }
        public decimal HighestPrice { set; get; }
        public decimal LowestPrice { set; get; }
        public decimal BiddingBuyNo1Price { set; get; }
        public decimal BiddingSellNo1Price { set; get; }
        public decimal DealAmount { set; get; }
        public decimal DealMoney { set; get; }
        public DateTime ModifiedDate { set; get; }

        public string TecScoreList { set; get; }
        public decimal? MaxTecScore { get; set; }
        public string WholeScoreList { set; get; }
        public decimal? MaxWholeScore { get; set; }

        public decimal? MaxAmplitude { get; set; }
        //public decimal? MaxHighAmplitude { get; set; }

        public decimal? Day1Amplitude { get; set; }
        public decimal? Day2Amplitude { get; set; }
        public decimal? Day3Amplitude { get; set; }
        public decimal? Day4Amplitude { get; set; }
        public decimal? Day5Amplitude { get; set; }
        public decimal? Day6Amplitude { get; set; }
        public decimal? Day7Amplitude { get; set; }

        //public decimal? Day1ChangeHand { get; set; }
        //public decimal? Day2ChangeHand { get; set; }
        //public decimal? Day3ChangeHand { get; set; }
        //public decimal? Day4ChangeHand { get; set; }
        //public decimal? Day5ChangeHand { get; set; }
        //public decimal? Day6ChangeHand { get; set; }
        //public decimal? Day7ChangeHand { get; set; }



        public decimal? Day1Lenth { get; set; }
        public decimal? Day2Lenth { get; set; }
        public decimal? Day3Lenth { get; set; }
        public decimal? Day4Lenth { get; set; }
        public decimal? Day5Lenth { get; set; }
        public decimal? Day6Lenth { get; set; }
        public decimal? Day7Lenth { get; set; }

        public decimal? Day1OpenPrice { get; set; }
        public decimal? Day2OpenPrice { get; set; }
        public decimal? Day3OpenPrice { get; set; }
        public decimal? Day4OpenPrice { get; set; }
        public decimal? Day5OpenPrice { get; set; }
        public decimal? Day6OpenPrice { get; set; }
        public decimal? Day7OpenPrice { get; set; }


        public decimal? MaxClosePrice { get; set; }
        public decimal? MinClosePrice { get; set; }
        public decimal? Day1ClosePrice { get; set; }
        public decimal? Day2ClosePrice { get; set; }
        public decimal? Day3ClosePrice { get; set; }
        public decimal? Day4ClosePrice { get; set; }
        public decimal? Day5ClosePrice { get; set; }
        public decimal? Day6ClosePrice { get; set; }
        public decimal? Day7ClosePrice { get; set; }


        public decimal? MaxHighPrice { get; set; }
        public decimal? Day1HighPrice { get; set; }
        public decimal? Day2HighPrice { get; set; }
        public decimal? Day3HighPrice { get; set; }
        public decimal? Day4HighPrice { get; set; }
        public decimal? Day5HighPrice { get; set; }
        public decimal? Day6HighPrice { get; set; }
        public decimal? Day7HighPrice { get; set; }


        public decimal? MinLowPrice { get; set; }
        public decimal? Day1LowPrice { get; set; }
        public decimal? Day2LowPrice { get; set; }
        public decimal? Day3LowPrice { get; set; }
        public decimal? Day4LowPrice { get; set; }
        public decimal? Day5LowPrice { get; set; }
        public decimal? Day6LowPrice { get; set; }
        public decimal? Day7LowPrice { get; set; }
    }
}
