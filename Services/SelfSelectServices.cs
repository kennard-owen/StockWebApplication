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

            //if (bootstrap.FavorType == "����") {
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
                .GroupBy(s => s.Operation) // �ȷ���
                .Select(s => new
                {
                    s.Operation,
                    Strategy = SqlFunc.AggregateMax(s.Strategy),
                    COUNT = SqlFunc.AggregateCount(s.SelfSelectId),
                    LatestDate = SqlFunc.AggregateMax(s.CreateDate) // �����ۺ��ֶ�
                })
                .OrderBy(s => s.LatestDate, OrderByType.Desc) // �ԾۺϺ�Ľ������
                .ToList();
            return list.JilToJson();
        }



        public string OperationListDates(int isStock)
        {
            var list = _client.Queryable<SelfSelect>()
                .Where(s => s.IsDel == 1 && s.IsStock == isStock)
                .GroupBy(s => s.CreateDate.Value.Date.ToString("yyyy-MM-dd")) // �����ڲ��ַ���
                .Select(s => new
                {
                    CreateDate = s.CreateDate.Value.Date.ToString("yyyy-MM-dd"), // ת��Ϊ�������ַ���
                    COUNT = SqlFunc.AggregateCount(1) // ͳ������
                })
                .OrderBy(s => s.CreateDate, OrderByType.Desc) // �����������ڽ�������
                .ToList();
            return list.JilToJson();
        }



        //public string OperationListStrategys(string CreateDate)
        //{
        //    var date = Convert.ToDateTime(CreateDate);
        //    var list = _client.Queryable<SelfSelect>()
        //        .Where(s => s.IsDel == 1 && s.IsStock == 1 && s.CreateDate.Value.Date== date.Date)
        //        .GroupBy(s => s.Strategy) // �����ڲ��ַ���
        //        .Select(s => new {
        //            Strategy = s.Strategy, // ת��Ϊ�������ַ���
        //            COUNT = SqlFunc.AggregateCount(1) // ͳ������
        //        })
        //        .OrderBy(s => s.Strategy, OrderByType.Desc) // �����������ڽ�������
        //        .ToList();
        //    return list.JilToJson();
        //}


        //public string OperationListStrategys(List<string> CreateDates)
        //{

        //    var date = Convert.ToDateTime(CreateDate);
        //    var list = _client.Queryable<SelfSelect>()
        //        .Where(s => s.IsDel == 1 && s.IsStock == 1 && s.CreateDate.Value.Date == date.Date)
        //        .GroupBy(s => string.IsNullOrEmpty(s.Strategy) ? s.Operation : s.Strategy) // ��� Strategy Ϊ�գ�ʹ�� Operation ���
        //        .Select(s => new {
        //            Strategy = string.IsNullOrEmpty(s.Strategy) ? s.Operation : s.Strategy, // ������ Strategy �� Operation
        //            COUNT = SqlFunc.AggregateCount(1) // ͳ������
        //        })
        //        .OrderBy(s => s.Strategy, OrderByType.Desc) // �� Strategy ��������
        //        .ToList();
        //    return list.JilToJson();
        //}
        public string OperationListStrategys(List<string> CreateDates,int isStock)
        {
            // ���ַ��������б�ת��Ϊ DateTime �б������ڲ��֣�
            var dateList = CreateDates.Select(d => Convert.ToDateTime(d).Date).ToList();

            var list = _client.Queryable<SelfSelect>()
                .Where(s => s.IsDel == 1 && s.IsStock == isStock && dateList.Contains(s.CreateDate.Value.Date)) // ɸѡ CreateDate ���б��еļ�¼
                .GroupBy(s => string.IsNullOrEmpty(s.Strategy) ? s.Operation : s.Strategy) // ��� Strategy Ϊ�գ�ʹ�� Operation ���
                .Select(s => new {
                    Strategy = string.IsNullOrEmpty(s.Strategy) ? s.Operation : s.Strategy, // ������ Strategy �� Operation
                    COUNT = SqlFunc.AggregateCount(1) // ͳ������
                })
                .OrderBy(s => s.Strategy, OrderByType.Desc) // �� Strategy ��������
                .ToList();
            return list.JilToJson();
        }

        public string OperationListOperations(List<string> CreateDates, List<string> Strategys, int IsStock)
        {
            // ���ַ��������б�ת��Ϊ DateTime �б������ڲ��֣�
            var dateList = CreateDates.Select(d => Convert.ToDateTime(d).Date).ToList();

            var list = _client.Queryable<SelfSelect>()
                .Where(s => s.IsDel == 1 && s.IsStock == IsStock &&
                            dateList.Contains(s.CreateDate.Value.Date) && // ���� CreateDates �е���������
                            (Strategys.Contains(s.Strategy)|| Strategys.Contains(s.Operation))) // ���� Strategys �е��������
                .OrderBy(s => s.CreateDate, OrderByType.Desc)
                .GroupBy(s => s.Operation) // �� Operation ����
                .Select(s => new {
                    Operation = s.Operation, // ������ Operation
                    COUNT = SqlFunc.AggregateCount(s.SelfSelectId), // ͳ������
                })
                .ToList();


            return list.JilToJson();
        }

        public string OperationListOperationsNew(List<string> CreateDates, List<string> Strategys, int IsStock)
        {
            // ���ַ��������б�ת��Ϊ DateTime �б������ڲ��֣�
            var dateList = CreateDates.Select(d => Convert.ToDateTime(d).Date).ToList();

            var list = _client.Queryable<SelfSelect>()
                .Where(s => s.IsDel == 1 && s.IsStock == IsStock &&
                            dateList.Contains(s.CreateDate.Value.Date) && // ���� CreateDates �е���������
                            (Strategys.Contains(s.Strategy) || Strategys.Contains(s.Operation))) // ���� Strategys �е��������
                .GroupBy(s => s.Operation) // �� Operation ����
                .Select(s => new {
                    Operation = s.Operation, // ������ Operation
                    Strategy = SqlFunc.AggregateMax(s.Strategy),
                    COUNT = SqlFunc.AggregateCount(s.SelfSelectId), // ͳ������
                    LatestDate = SqlFunc.AggregateMax(s.CreateDate) // �����ۺ��ֶ�
                })
                  .OrderBy(s => s.LatestDate, OrderByType.Desc) // �ԾۺϺ�Ľ������
                .ToList();




            //LatestDate = SqlFunc.AggregateMax(s.CreateDate) // �����ۺ��ֶ�
            //    })
            //    .OrderBy(s => s.LatestDate, OrderByType.Desc) // �ԾۺϺ�Ľ������
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
                     ��Ʊ���� = s.StockNumber,
                     �������� = c.StockName,
                     �Ƽ����� = s.RecommendDate,
                     ��ע = s.Remark,
                 
                     ���� = s.Operation,
                     �ɱ� = s.Cost.ToString(),
                     ��ǰ�۸� = s.CurrentPrice.ToString(),
                     �ֹ�1������ = s.Day1Profit,
                     �ֹ�2������ = s.Day2Profit,
                     �ֹ�3������ = s.Day3Profit,
                     �ֹ�4������ = s.Day4Profit,
                     �ֹ�5������ = s.Day5Profit,
                     �ֹ�6������ = s.Day6Profit,
                     �ֹ�7������ = s.Day7Profit,
                     ����ʱ�� = s.CreateDate,
                     �޸�ʱ�� = s.ModifiedDate,
                     ���� = c.Conception,

                 }).MergeTable();

            if (!bootstrap.search.IsEmpty()) {
                query.Where((s) => s.��Ʊ����.Contains(bootstrap.search) || s.��������.Contains(bootstrap.search) || s.��ע.Contains(bootstrap.search));
            }
            if (!bootstrap.datemin.IsEmpty() && !bootstrap.datemax.IsEmpty()) {
                query.Where(s => s.����ʱ�� >= bootstrap.datemin.ToDateTimeB() && s.����ʱ�� <= bootstrap.datemax.ToDateTimeE());
            }

            query.OrderBy($"MergeTable.�ֹ�2������ desc");

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
                if (item.�ֹ�1������ >= low && item.�ֹ�1������ < high) {
                    count[0] = count[0] + 1;
                }
                if (item.�ֹ�2������ >= low && item.�ֹ�2������ < high) {
                    count[1] = count[1] + 1;
                }
                if (item.�ֹ�3������ >=low && item.�ֹ�3������ < high) {
                    count[2] = count[2] + 1;
                }
                if (item.�ֹ�4������ >= low && item.�ֹ�4������ < high) {
                    count[3] = count[3] + 1;
                }
                if (item.�ֹ�5������ >= low && item.�ֹ�5������ < high) {
                    count[4] = count[4] + 1;
                }
                if (item.�ֹ�6������ >= low && item.�ֹ�6������ < high) {
                    count[5] = count[5] + 1;
                }
                if (item.�ֹ�7������ >=low && item.�ֹ�6������ < high) {
                    count[6] = count[6] + 1;
                }

            }

            for (int i = 0; i < 7; i++) {
                res[i] = Math.Round((decimal)count[i] / total, 2);
            }
            resObject.�ɱ� = $"����";
            resObject.��ǰ�۸� = $">={low}%,<{high}%ռ��";
            resObject.�ֹ�1������ = res[0];
            resObject.�ֹ�2������ = res[1];
            resObject.�ֹ�3������ = res[2];
            resObject.�ֹ�4������ = res[3];
            resObject.�ֹ�5������ = res[4];
            resObject.�ֹ�6������ = res[5];
            resObject.�ֹ�7������ = res[6];
            return resObject;
        }

        private ExcelReport GetReportAverageProfit(List<ExcelReport> list, decimal low, decimal high) {
            int total = list.Count;
            ExcelReport resObject = new ExcelReport();
            decimal[] res = new decimal[7];
           
            decimal[] averageProfit = new decimal[7];
            foreach (var item in list) {
               
                averageProfit[0] = averageProfit[0] + item.�ֹ�1������!=-9999? item.�ֹ�1������:0;
                averageProfit[1] = averageProfit[1] + item.�ֹ�2������!=-9999? item.�ֹ�2������:0;
                averageProfit[2] = averageProfit[2] + item.�ֹ�3������!=-9999? item.�ֹ�3������:0;
                averageProfit[3] = averageProfit[3] + item.�ֹ�4������!=-9999? item.�ֹ�4������:0;
                averageProfit[4] = averageProfit[4] + item.�ֹ�5������!=-9999? item.�ֹ�5������:0;
                averageProfit[5] = averageProfit[5] + item.�ֹ�6������!=-9999? item.�ֹ�6������:0;
                averageProfit[6] = averageProfit[6] + item.�ֹ�7������!=-9999? item.�ֹ�7������:0;
                 

            }

            for (int i = 0; i < 7; i++) {
                res[i] = Math.Round((decimal)averageProfit[i] / total, 2);
            }
            resObject.�ɱ� = $"����";
            resObject.��ǰ�۸� = $"���ƽ��ӯ��N����";
            resObject.�ֹ�1������ = res[0];
            resObject.�ֹ�2������ = res[1];
            resObject.�ֹ�3������ = res[2];
            resObject.�ֹ�4������ = res[3];
            resObject.�ֹ�5������ = res[4];
            resObject.�ֹ�6������ = res[5];
            resObject.�ֹ�7������ = res[6];
            return resObject;
        }

        public class ExcelReport{

            public string ��Ʊ���� { get; set; }
            public string �������� { get; set; }
            public DateTime? �Ƽ����� { get; set; }
            public string ��ע { get; set; }
            public string ���� { get; set; }
            public string �ɱ� { get; set; }
            public string ��ǰ�۸� { get; set; }

            public decimal �ֹ�1������ { get; set; }
            public decimal �ֹ�2������ { get; set; }
            public decimal �ֹ�3������  { get; set; }
            public decimal �ֹ�4������  { get; set; }
            public decimal �ֹ�5������  { get; set; }
            public decimal �ֹ�6������  { get; set; }
            public decimal �ֹ�7������  { get; set; }
            public DateTime ? ����ʱ�� { get; set; }
            public DateTime ? �޸�ʱ�� { get; set; }
            public string ���� { get; set; }

        }

    }
}