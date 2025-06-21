using System;
using System.Collections.Generic;
using static PF.Utils.Table.Bootstrap;

namespace PF.Core.Dto
{
    public class PubParams
    {
        /// <summary>
        /// 设备查询参数
        /// </summary>
        public class DeviceBootstrapParams : BootstrapParams
        {
            public string DeptId { get; set; }
            public string DeviceType { get; set; }
            public string PropertyType { get; set; }
        }

        /// <summary>
        /// 字典查询参数
        /// </summary>
        public class DictBootstrapParams : BootstrapParams
        {
            public string DictType { get; set; }
        }


        public class ProcedureBootstrapParams : BootstrapParams
        {
            public string Procedure { get; set; }
        }

        /// <summary>
        /// 字典查询参数
        /// </summary>
        public class FavorTypeBootstrapParams : BootstrapParams {
            public string FavorType { get; set; }
            public string StockNumber { get; set; }
            public string Stock { get; set; }

            public string WeekInfo { get; set; }

            public int IsStock { get; set; }
            public string Date { get; set; }

            public List<DateTime> SelectedDates { get; set; }
            string Number { get; set; }
            public List<string> CheckOperationList { get; set; }
            
        } 
        /// <summary>
        /// 入库单查询参数
        /// </summary>
        public class StockInBootstrapParams : BootstrapParams
        {
            public string StockInType { get; set; }
            public string StockInStatus { get; set; }
        }

        /// <summary>
        /// 策略ID
        /// </summary>
        public class StrategyBootstrapParams : BootstrapParams {
            public string StrategyId { get; set; }
            public string InOrAfterTrade { get; set; }
            public string DateWantSee { get; set; }
            public string Section { get; set; }
            public string sql { get; set; }
            public List<string> StrategyIdList { get; set; }
            public List<string> CheckSectionList { get; set; }
        }

        /// <summary>
        /// 库存查询参数
        /// </summary>
        public class InventoryBootstrapParams : BootstrapParams
        {
            public string StorageRackId { get; set; }
            public string MaterialId { get; set; }
        }

        /// <summary>
        /// 出库单查询参数
        /// </summary>
        public class StockOutBootstrapParams : BootstrapParams
        {
            public string StockOutType { get; set; }
            public string StockOutStatus { get; set; }
        }

        public class StatusBootstrapParams : BootstrapParams
        {
            public string Status { get; set; }
        }
    }
}