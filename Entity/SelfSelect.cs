using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PF.Core.Entity {

    [SugarTable("selfselects")]
    public class SelfSelect {


        public SelfSelect() {
            this.IsDel = Convert.ToByte("1");
        }


        [SugarColumn(IsPrimaryKey = true)]
        public long SelfSelectId { set; get; }

        public int IsStock { get; set; }

        public string Type { get; set; }
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
        /// 确诊时间
        /// </summary>
        public DateTime RecommendDate { get; set; }
        /// <summary>
        /// 交易地
        /// </summary>
        public string TradePlace { get; set; }
      
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        #endregion
        /// <summary>
        /// 行业
        /// </summary>
        public string Section { get; set; }

        public decimal? Whole_BeatHowMany { get; set; }
        public decimal? Tec_BeatHowMany { get; set; }
        public decimal? Fund_BeatHowMany { get; set; }
        public decimal? Message_BeatHowMany { get; set; }
        public decimal? Section_BeatHowMany { get; set; }
        public decimal? Basic_BeatHowMany { get; set; }
        #region 操作
        /// <summary>
        /// 股票代码
        /// </summary>
        public string Operation { get; set; }


        /// <summary>
        /// 股票代码
        /// </summary>
        public string Strategy { get; set; }
        /// <summary>
        /// 股票名称
        /// </summary>
        public decimal Cost { get; set; }
        /// <summary>
        /// 行业
        /// </summary>
        public decimal CurrentPrice { get; set; }
        /// <summary>
        /// 交易地
        /// </summary>
        public decimal Margin { get; set; }

        /// <summary>
        /// 买了多少钱
        /// </summary>
        public decimal HowMuch { get; set; }
        /// <summary>
        /// Desc:1未删除   0删除
        /// Default:1
        /// Nullable:False
        /// </summary>
        public byte IsDel { get; set; }
        #endregion
        public DateTime? ModifiedDate { get; set; }
        public DateTime? CreateDate { get; set; } = DateTime.Now;
        public long? ModifiedBy { get; set; }


        public decimal Day1Profit { get; set; } = -9999;
        public decimal Day2Profit { get; set; } = -9999;
        public decimal Day3Profit { get; set; } = -9999;
        public decimal Day4Profit { get; set; } = -9999;
        public decimal Day5Profit { get; set; } = -9999;
        public decimal Day6Profit { get; set; } = -9999;
        public decimal Day7Profit { get; set; } = -9999;
        public long? CreateBy { get; set; }
    }
}
