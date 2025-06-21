using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PF.Core.Entity {

    [SugarTable("Sectionfundhis")]
    public class Sectionfundhis
    {

        public Sectionfundhis() {
         
        }

        [SugarColumn(IsPrimaryKey = true)]
        public int Id { set; get; }

        public string platename { set; get; }

        public decimal ConAmplitude { set; get; }

        public decimal FundIn { set; get; }

        public decimal UpRate { set; get; }

        public DateTime UpdateDate { set; get; }

        public DateTime? CreatTime { get; set; } = DateTime.Now;
    }
}
