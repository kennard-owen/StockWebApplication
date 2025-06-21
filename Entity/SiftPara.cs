using SqlSugar;
using System;

namespace PF.Core.Entity
{
    ///<summary>
    ///
    ///</summary>
    public partial class SiftPara
    {
        public SiftPara()
        {

            
        }
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public long Id { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>
        public string SiftParaData { get; set; }

        public string StockOrFund { get; set; }
        public long StrategyId { get; set; }

        

        public DateTime? CreateDateTime { get; set; } = DateTime.Now;


    }
}