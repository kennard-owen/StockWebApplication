using SqlSugar;
using System;

namespace PF.Core.Entity
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("procedurelist")]
    public partial class Procedurelist
    {
        public Procedurelist()
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
        public string ProcedureName { get; set; }


        public string Remark { get; set; }

        public int Order { get; set; }

    }
}