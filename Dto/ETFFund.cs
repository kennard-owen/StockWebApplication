using System;
using System.Collections.Generic;
using System.Text;

namespace PF.Core.Dto
{
    public class ETFFund
    {
        public int Id { set; get; }

        public string SelfSelectId { set; get; }
        public string Name { set; get; }
        public string Code { set; get; }
        public decimal Price { set; get; }

        public decimal Amplitude { set; get; }

        public decimal PremiumRate { set; get; }
        public decimal DealMoney { set; get; }

        public decimal AmplitudeOneMonth { set; get; }

        public decimal AmplitudeThisYear { set; get; }
        public decimal AmplitudeLatestOneYear { set; get; }

        public decimal AmplitudeLatestThreeYear { set; get; }

        public decimal AmplitudeLatestFiveYear { set; get; }

        public decimal FundScale { set; get; }

        public decimal OperationRate { set; get; }

        public string TrackIndex { set; get; }

        public DateTime GoPublicDate { set; get; }

        public string FundCompany { set; get; }

        public string FundType { set; get; }
        public string Category { set; get; }
        public string Hot { set; get; }
        public string StocksOwned { set; get; }
        public string StocksOwnedNames { set; get; }

        public DateTime UpdateTime { set; get; } = DateTime.Now;

        public DateTime Date { set; get; } = DateTime.Now;

        public DateTime CreateDate { set; get; } = DateTime.Now;
        public DateTime ModifiedDate { set; get; } = DateTime.Now;
        public DateTime RecommendDate { set; get; } = DateTime.Now;

        public decimal? Day7Amplitude { set; get; }
        public decimal? Day6Amplitude { set; get; }
        public decimal? Day5Amplitude { set; get; }
        public decimal? Day4Amplitude { set; get; }
        public decimal? Day3Amplitude { set; get; }
        public decimal? Day2Amplitude { set; get; }
        public decimal? Day1Amplitude { set; get; }

        public decimal? Day7ClosePrice { set; get; }
        public decimal? Day6ClosePrice { set; get; }
        public decimal? Day5ClosePrice { set; get; }
        public decimal? Day4ClosePrice { set; get; }
        public decimal? Day3ClosePrice { set; get; }
        public decimal? Day2ClosePrice { set; get; }
        public decimal? Day1ClosePrice { set; get; }

        public decimal? MaxClosePrice { set; get; }
        public decimal? MinClosePrice { set; get; }
        public decimal? MaxAmplitude { set; get; }



        public decimal NDayAmplitude { get; set; }
        public decimal GapPercent { get; set; }
        public decimal GapPercentWithLow { get; set; }
    }
}