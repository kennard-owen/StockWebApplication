using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace PF.Core.Entity
{
    [SugarTable("FundRealTimeInfoes")]
    public class FundRealTimeInfo
    {
        public int Id { set; get; }

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

    }
}
