using IRepository;
using IServices;
using SqlSugar;
using System;
using PF.Core.Dto;
using PF.Core.Entity;
using PF.Utils.Extensions;
using PF.Utils.Json;
using PF.Utils.Table;
using Microsoft.AspNetCore.Mvc;
using PF.Utils.Excel;
using PF.Utils.Pub;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class SelfSelectServices : BaseServices<SelfSelect>, ISelfSelectServices {
        private readonly ISelfSelectRepository _repository;
        private readonly SqlSugarClient _client;

        public SelfSelectServices(
            SqlSugarClient client,
            ISelfSelectRepository repository) : base(repository) {
            _repository = repository;
            _client = client;
        }

        public string PageList(PubParams.FavorTypeBootstrapParams bootstrap) {
            int totalNumber = 0;
            if (bootstrap.offset != 0) {
                bootstrap.offset = bootstrap.offset / bootstrap.limit + 1;
            }
            //var query = _client.Queryable<SelfSelect>().Where(s => s.Operation == bootstrap.FavorType).Where(s=>s.IsDel==1);

            var query = _client.Queryable<SelfSelect, StockBaseInfo>((s, c) => new object[] {
                   JoinType.Left,s.StockNumber==c.StockNumber
                 }).Where((s, c) => s.Operation == bootstrap.FavorType).Where((s, c) => s.IsDel == 1).Select((s, c) => new {
                     SelfSelectId = s.SelfSelectId.ToString(),
                     s.StockNumber,
                     c.StockName,
                     s.RecommendDate,
                     s.Remark,
                     s.Section,
                     s.Operation,
                     s.Cost,
                     s.CurrentPrice,
                     s.Margin,
                     s.CreateDate,
                     s.ModifiedDate,
                 }).MergeTable();

            //if (bootstrap.FavorType == "买入") {
            //    var list1 = query.ToPageList(bootstrap.offset, bootstrap.limit, ref totalNumber);
            //    return Bootstrap.GridData(list1, totalNumber).JilToJson();
            //}

            if (!bootstrap.search.IsEmpty()) {
                query.Where((s) => s.StockNumber.Contains(bootstrap.search) || s.StockName.Contains(bootstrap.search) || s.Remark.Contains(bootstrap.search));
            }
            if (!bootstrap.datemin.IsEmpty() && !bootstrap.datemax.IsEmpty()) {
                query.Where(s => s.CreateDate >= bootstrap.datemin.ToDateTimeB() && s.CreateDate <= bootstrap.datemax.ToDateTimeE());
            }
            if (bootstrap.order.Equals("desc", StringComparison.OrdinalIgnoreCase)) {
                query.OrderBy($"MergeTable.{bootstrap.sort} desc");
            }
            else {
                query.OrderBy($"MergeTable.{bootstrap.sort} asc");
            }
            var list = query.ToPageList(bootstrap.offset, bootstrap.limit, ref totalNumber);
            return Bootstrap.GridData(list, totalNumber).JilToJson();
        }

        public string OperationList(int isStock)
        {
            var list = _client.Queryable<SelfSelect>()
                .Where(s => s.IsDel == 1 && s.IsStock == isStock)
                .OrderBy(s => s.CreateDate, OrderByType.Desc)
                .GroupBy(s => s.Operation)
                .Select(
                s => new
                {
                    s.Operation,
                    s.Strategy,
                    COUNT = SqlFunc.AggregateCount(s.SelfSelectId)
                }).ToList();
            return list.JilToJson();
        }


        public string OperationListNew(int isStock)
        {
            var list = _client.Queryable<SelfSelect>()
                .Where(s => s.IsDel == 1 && s.IsStock == isStock)
                .GroupBy(s => s.Operation) // 先分组
                .Select(s => new
                {
                    s.Operation,
                    Strategy = SqlFunc.AggregateMax(s.Strategy),
                    COUNT = SqlFunc.AggregateCount(s.SelfSelectId),
                    LatestDate = SqlFunc.AggregateMax(s.CreateDate) // 新增聚合字段
                })
                .OrderBy(s => s.LatestDate, OrderByType.Desc) // 对聚合后的结果排序
                .ToList();
            return list.JilToJson();
        }



        public string OperationListDates(int isStock)
        {
            var list = _client.Queryable<SelfSelect>()
                .Where(s => s.IsDel == 1 && s.IsStock == isStock)
                .GroupBy(s => s.CreateDate.Value.Date.ToString("yyyy-MM-dd")) // 按日期部分分组
                .Select(s => new
                {
                    CreateDate = s.CreateDate.Value.Date.ToString("yyyy-MM-dd"), // 转换为年月日字符串
                    COUNT = SqlFunc.AggregateCount(1) // 统计数量
                })
                .OrderBy(s => s.CreateDate, OrderByType.Desc) // 按分组后的日期降序排序
                .ToList();
            return list.JilToJson();
        }



        //public string OperationListStrategys(string CreateDate)
        //{
        //    var date = Convert.ToDateTime(CreateDate);
        //    var list = _client.Queryable<SelfSelect>()
        //        .Where(s => s.IsDel == 1 && s.IsStock == 1 && s.CreateDate.Value.Date== date.Date)
        //        .GroupBy(s => s.Strategy) // 按日期部分分组
        //        .Select(s => new {
        //            Strategy = s.Strategy, // 转换为年月日字符串
        //            COUNT = SqlFunc.AggregateCount(1) // 统计数量
        //        })
        //        .OrderBy(s => s.Strategy, OrderByType.Desc) // 按分组后的日期降序排序
        //        .ToList();
        //    return list.JilToJson();
        //}


        //public string OperationListStrategys(List<string> CreateDates)
        //{

        //    var date = Convert.ToDateTime(CreateDate);
        //    var list = _client.Queryable<SelfSelect>()
        //        .Where(s => s.IsDel == 1 && s.IsStock == 1 && s.CreateDate.Value.Date == date.Date)
        //        .GroupBy(s => string.IsNullOrEmpty(s.Strategy) ? s.Operation : s.Strategy) // 如果 Strategy 为空，使用 Operation 替代
        //        .Select(s => new {
        //            Strategy = string.IsNullOrEmpty(s.Strategy) ? s.Operation : s.Strategy, // 分组后的 Strategy 或 Operation
        //            COUNT = SqlFunc.AggregateCount(1) // 统计数量
        //        })
        //        .OrderBy(s => s.Strategy, OrderByType.Desc) // 按 Strategy 降序排序
        //        .ToList();
        //    return list.JilToJson();
        //}
        public string OperationListStrategys(List<string> CreateDates,int isStock)
        {
            // 将字符串日期列表转换为 DateTime 列表（仅日期部分）
            var dateList = CreateDates.Select(d => Convert.ToDateTime(d).Date).ToList();

            var list = _client.Queryable<SelfSelect>()
                .Where(s => s.IsDel == 1 && s.IsStock == isStock && dateList.Contains(s.CreateDate.Value.Date)) // 筛选 CreateDate 在列表中的记录
                .GroupBy(s => string.IsNullOrEmpty(s.Strategy) ? s.Operation : s.Strategy) // 如果 Strategy 为空，使用 Operation 替代
                .Select(s => new {
                    Strategy = string.IsNullOrEmpty(s.Strategy) ? s.Operation : s.Strategy, // 分组后的 Strategy 或 Operation
                    COUNT = SqlFunc.AggregateCount(1) // 统计数量
                })
                .OrderBy(s => s.Strategy, OrderByType.Desc) // 按 Strategy 降序排序
                .ToList();
            return list.JilToJson();
        }

        public string OperationListOperations(List<string> CreateDates, List<string> Strategys, int IsStock)
        {
            // 将字符串日期列表转换为 DateTime 列表（仅日期部分）
            var dateList = CreateDates.Select(d => Convert.ToDateTime(d).Date).ToList();

            var list = _client.Queryable<SelfSelect>()
                .Where(s => s.IsDel == 1 && s.IsStock == IsStock &&
                            dateList.Contains(s.CreateDate.Value.Date) && // 包含 CreateDates 中的任意日期
                            (Strategys.Contains(s.Strategy)|| Strategys.Contains(s.Operation))) // 包含 Strategys 中的任意策略
                .OrderBy(s => s.CreateDate, OrderByType.Desc)
                .GroupBy(s => s.Operation) // 按 Operation 分组
                .Select(s => new {
                    Operation = s.Operation, // 分组后的 Operation
                    COUNT = SqlFunc.AggregateCount(s.SelfSelectId), // 统计数量
                })
                .ToList();


            return list.JilToJson();
        }

        public string OperationListOperationsNew(List<string> CreateDates, List<string> Strategys, int IsStock)
        {
            // 将字符串日期列表转换为 DateTime 列表（仅日期部分）
            var dateList = CreateDates.Select(d => Convert.ToDateTime(d).Date).ToList();

            var list = _client.Queryable<SelfSelect>()
                .Where(s => s.IsDel == 1 && s.IsStock == IsStock &&
                            dateList.Contains(s.CreateDate.Value.Date) && // 包含 CreateDates 中的任意日期
                            (Strategys.Contains(s.Strategy) || Strategys.Contains(s.Operation))) // 包含 Strategys 中的任意策略
                .GroupBy(s => s.Operation) // 按 Operation 分组
                .Select(s => new {
                    Operation = s.Operation, // 分组后的 Operation
                    Strategy = SqlFunc.AggregateMax(s.Strategy),
                    COUNT = SqlFunc.AggregateCount(s.SelfSelectId), // 统计数量
                    LatestDate = SqlFunc.AggregateMax(s.CreateDate) // 新增聚合字段
                })
                  .OrderBy(s => s.LatestDate, OrderByType.Desc) // 对聚合后的结果排序
                .ToList();




            //LatestDate = SqlFunc.AggregateMax(s.CreateDate) // 新增聚合字段
            //    })
            //    .OrderBy(s => s.LatestDate, OrderByType.Desc) // 对聚合后的结果排序
            //    .ToList();


            return list.JilToJson();
        }

        //public string OperationList(int isStock)
        //{
        //    var list = _client.Queryable<SelfSelect>()
        //        .Where(s => s.IsDel == 1 && s.IsStock == isStock)
        //        .GroupBy(s => s.Operation)
        //        .OrderBy(s => SqlFunc.AggregateMax(s.CreateDate), OrderByType.Desc)  // assuming you want the latest date
        //        .Select(s => new {
        //            s.Operation,
        //            COUNT = SqlFunc.AggregateCount(s.SelfSelectId)
        //        }).ToList();
        //    return list.JilToJson();
        //}


        public string DeleteSelectedStrategy(PubParams.FavorTypeBootstrapParams bootstrap) {
            throw new NotImplementedException();
        }

        public byte[] ExportList(PubParams.FavorTypeBootstrapParams bootstrap) {
            var query = _client.Queryable<SelfSelect, StockBaseInfo>((s, c) => new object[] {
                   JoinType.Left,s.StockNumber==c.StockNumber
                 }).Where((s, c) => s.Operation == bootstrap.FavorType).Where((s, c) => s.IsDel == 1).Select((s, c) => new ExcelReport {
                     股票代码 = s.StockNumber,
                     代码名称 = c.StockName,
                     推荐日期 = s.RecommendDate,
                     备注 = s.Remark,
                 
                     分类 = s.Operation,
                     成本 = s.Cost.ToString(),
                     当前价格 = s.CurrentPrice.ToString(),
                     持股1天利润 = s.Day1Profit,
                     持股2天利润 = s.Day2Profit,
                     持股3天利润 = s.Day3Profit,
                     持股4天利润 = s.Day4Profit,
                     持股5天利润 = s.Day5Profit,
                     持股6天利润 = s.Day6Profit,
                     持股7天利润 = s.Day7Profit,
                     创建时间 = s.CreateDate,
                     修改时间 = s.ModifiedDate,
                     概念 = c.Conception,

                 }).MergeTable();

            if (!bootstrap.search.IsEmpty()) {
                query.Where((s) => s.股票代码.Contains(bootstrap.search) || s.代码名称.Contains(bootstrap.search) || s.备注.Contains(bootstrap.search));
            }
            if (!bootstrap.datemin.IsEmpty() && !bootstrap.datemax.IsEmpty()) {
                query.Where(s => s.创建时间 >= bootstrap.datemin.ToDateTimeB() && s.创建时间 <= bootstrap.datemax.ToDateTimeE());
            }

            query.OrderBy($"MergeTable.持股2天利润 desc");

            var list = query.ToList();
            var rerort1 = GetReport(list, 0, 100);
            var rerort2 = GetReport(list, 3, 100);
            var rerort3 =GetReportAverageProfit(list, 3, 100);

            list.Add(rerort1);
            list.Add(rerort2);
            list.Add(rerort3);

            return NpoiUtil.Export(list, ExcelVersion.V2007);
        }



        private ExcelReport GetReport(List<ExcelReport> list,decimal low,decimal high) {
            int total = list.Count;
            ExcelReport resObject = new ExcelReport();
            decimal[] res = new decimal[7];
            int[] count = new int[7];
       
            foreach (var item in list) {
                if (item.持股1天利润 >= low && item.持股1天利润 < high) {
                    count[0] = count[0] + 1;
                }
                if (item.持股2天利润 >= low && item.持股2天利润 < high) {
                    count[1] = count[1] + 1;
                }
                if (item.持股3天利润 >=low && item.持股3天利润 < high) {
                    count[2] = count[2] + 1;
                }
                if (item.持股4天利润 >= low && item.持股4天利润 < high) {
                    count[3] = count[3] + 1;
                }
                if (item.持股5天利润 >= low && item.持股5天利润 < high) {
                    count[4] = count[4] + 1;
                }
                if (item.持股6天利润 >= low && item.持股6天利润 < high) {
                    count[5] = count[5] + 1;
                }
                if (item.持股7天利润 >=low && item.持股6天利润 < high) {
                    count[6] = count[6] + 1;
                }

            }

            for (int i = 0; i < 7; i++) {
                res[i] = Math.Round((decimal)count[i] / total, 2);
            }
            resObject.成本 = $"汇总";
            resObject.当前价格 = $">={low}%,<{high}%占比";
            resObject.持股1天利润 = res[0];
            resObject.持股2天利润 = res[1];
            resObject.持股3天利润 = res[2];
            resObject.持股4天利润 = res[3];
            resObject.持股5天利润 = res[4];
            resObject.持股6天利润 = res[5];
            resObject.持股7天利润 = res[6];
            return resObject;
        }

        private ExcelReport GetReportAverageProfit(List<ExcelReport> list, decimal low, decimal high) {
            int total = list.Count;
            ExcelReport resObject = new ExcelReport();
            decimal[] res = new decimal[7];
           
            decimal[] averageProfit = new decimal[7];
            foreach (var item in list) {
               
                averageProfit[0] = averageProfit[0] + item.持股1天利润!=-9999? item.持股1天利润:0;
                averageProfit[1] = averageProfit[1] + item.持股2天利润!=-9999? item.持股2天利润:0;
                averageProfit[2] = averageProfit[2] + item.持股3天利润!=-9999? item.持股3天利润:0;
                averageProfit[3] = averageProfit[3] + item.持股4天利润!=-9999? item.持股4天利润:0;
                averageProfit[4] = averageProfit[4] + item.持股5天利润!=-9999? item.持股5天利润:0;
                averageProfit[5] = averageProfit[5] + item.持股6天利润!=-9999? item.持股6天利润:0;
                averageProfit[6] = averageProfit[6] + item.持股7天利润!=-9999? item.持股7天利润:0;
                 

            }

            for (int i = 0; i < 7; i++) {
                res[i] = Math.Round((decimal)averageProfit[i] / total, 2);
            }
            resObject.成本 = $"汇总";
            resObject.当前价格 = $"组合平均盈利N个点";
            resObject.持股1天利润 = res[0];
            resObject.持股2天利润 = res[1];
            resObject.持股3天利润 = res[2];
            resObject.持股4天利润 = res[3];
            resObject.持股5天利润 = res[4];
            resObject.持股6天利润 = res[5];
            resObject.持股7天利润 = res[6];
            return resObject;
        }

        public class ExcelReport{

            public string 股票代码 { get; set; }
            public string 代码名称 { get; set; }
            public DateTime? 推荐日期 { get; set; }
            public string 备注 { get; set; }
            public string 分类 { get; set; }
            public string 成本 { get; set; }
            public string 当前价格 { get; set; }

            public decimal 持股1天利润 { get; set; }
            public decimal 持股2天利润 { get; set; }
            public decimal 持股3天利润  { get; set; }
            public decimal 持股4天利润  { get; set; }
            public decimal 持股5天利润  { get; set; }
            public decimal 持股6天利润  { get; set; }
            public decimal 持股7天利润  { get; set; }
            public DateTime ? 创建时间 { get; set; }
            public DateTime ? 修改时间 { get; set; }
            public string 概念 { get; set; }

        }

    }
}