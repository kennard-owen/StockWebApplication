using SqlSugar;
using System;

namespace PF.Core.Entity
{
    ///<summary>
    ///
    ///</summary>
    public partial class stockqualified {
        public stockqualified()
        {

            
        }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>
        public string StockNumber { get; set; }


        public string SiftType { get; set; }


    }
}