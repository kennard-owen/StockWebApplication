using IRepository;
using IServices;
using SqlSugar;
using System;
using PF.Core.Dto;
using PF.Core.Entity;
using PF.Utils.Extensions;
using PF.Utils.Json;
using PF.Utils.Pub;
using PF.Utils.Security;
using PF.Utils.Table;
using PF.Core.Orm.SqlSugar;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class FundServices : BaseServices<Conceptionfund>, IFundServices {
        private readonly SqlSugarClient _client;
        private readonly IfundRepository _repository;

        public FundServices(SqlSugarClient client, IfundRepository repository) : base(repository) {
            _client = client;
            _repository = repository;
        }

        public string PageList(Bootstrap.BootstrapParams bootstrap)
        {

            if (bootstrap.sort == "CreateDate") {
                bootstrap.sort = bootstrap.sort.Replace("CreateDate", "CreatTime");
            }
            var totalNumber = 0;
            if (bootstrap.offset != 0)
            {
                bootstrap.offset = bootstrap.offset / bootstrap.limit + 1;
            }
            //var order = ExpressionExt.InitO<Sys_user>(bootstrap.sort);

            var query = _client.Queryable<Conceptionfund>();
            if (!bootstrap.search.IsEmpty())
            {
                query.Where((s) => s.platename.Contains(bootstrap.search));
            }
            if (!bootstrap.datemin.IsEmpty() && !bootstrap.datemax.IsEmpty())
            {
                query.Where(s => s.UpdateDate > bootstrap.datemin.ToDateTimeB() && s.UpdateDate <= bootstrap.datemax.ToDateTimeE());
            }
            if (bootstrap.order.Equals("desc", StringComparison.OrdinalIgnoreCase))
            {
                query.OrderBy($"{bootstrap.sort} desc");
            }
            else
            {
                query.OrderBy($"{bootstrap.sort} asc");
            }
            var list = query.ToList();
            return Bootstrap.GridData(list, totalNumber).JilToJson();
        }

     

        public string PageListHis(Bootstrap.BootstrapParams bootstrap) {

            if (bootstrap.sort == "CreateDate") {
                bootstrap.sort = bootstrap.sort.Replace("CreateDate", "CreatTime");
            }
            var totalNumber = 0;
            if (bootstrap.offset != 0) {
                bootstrap.offset = bootstrap.offset / bootstrap.limit + 1;
            }
            //var order = ExpressionExt.InitO<Sys_user>(bootstrap.sort);

            var query = _client.Queryable<Conceptionfundhis>();
            if (!bootstrap.search.IsEmpty()) {
                query.Where((s) => s.platename.Contains(bootstrap.search));
            }
            if (!bootstrap.datemin.IsEmpty() && !bootstrap.datemax.IsEmpty()) {
                query.Where(s => s.UpdateDate > bootstrap.datemin.ToDateTimeB() && s.UpdateDate <= bootstrap.datemax.ToDateTimeE());
            }
            if (bootstrap.order.Equals("desc", StringComparison.OrdinalIgnoreCase)) {
                query.OrderBy($"{bootstrap.sort} desc");
            }
            else {
                query.OrderBy($"{bootstrap.sort} asc");
            }
            var list = query.ToPageList(bootstrap.offset, bootstrap.limit, ref totalNumber);
            return Bootstrap.GridData(list, totalNumber).JilToJson();
        }

        public string PageListGroup(Bootstrap.BootstrapParams bootstrap) {

            decimal fundInMin = 0;
            decimal fundInMax = 0;
            decimal TimesInMin = 0;
            decimal TimesInMax = 0;
            decimal CurrentFundMin = 0;
            decimal CurrentFundMax = 0;
            List<ConceptionfundGroup> res = new List<ConceptionfundGroup>();


            try
            {
                if (bootstrap.search != null && (bootstrap.search.Contains(",")|| bootstrap.search.Contains("£¬")))
                {
                    var temp = bootstrap.search.Split(new string[] { "," , "£¬" }, StringSplitOptions.RemoveEmptyEntries);
                    fundInMin = Convert.ToDecimal(temp[0]);
                    fundInMax = Convert.ToDecimal(temp[1]);
                    CurrentFundMin = Convert.ToDecimal(temp[2]);
                    CurrentFundMax = Convert.ToDecimal(temp[3]);
                    TimesInMin = Convert.ToDecimal(temp[4]);
                    TimesInMax = Convert.ToDecimal(temp[5]);
                   
                }
            }
            catch (Exception)
            {
                return null;
            }
           
            if (bootstrap.sort == "CreateDate") {
                bootstrap.sort = bootstrap.sort.Replace("CreateDate", "CreatTime");
            }
            var totalNumber = 0;
            if (bootstrap.offset != 0) {
                bootstrap.offset = bootstrap.offset / bootstrap.limit + 1;
            }
          
           var query= _client.Queryable<Conceptionfundhis>()
            .Where(s => s.UpdateDate >= bootstrap.datemin.ToDateTimeB() && s.UpdateDate <= bootstrap.datemax.ToDateTimeE())
            .GroupBy(s =>
               s.platename
                  )
            .Select(s => new ConceptionfundGroup() {
                platename = s.platename,
                FundInTotal = SqlFunc.AggregateSum(s.FundIn),
                Times=0,
                CurrentFund=0,
                TimeSpan = SqlFunc.AggregateCount(s.Id),
            });

            if (!bootstrap.search.IsEmpty()) {
                if (!(bootstrap.search.Contains(",")||bootstrap.search.Contains("£¬")))
                {
                    query.Where((s) => s.platename.Contains(bootstrap.search));
                }
              
            }
            if (bootstrap.order.Equals("desc", StringComparison.OrdinalIgnoreCase)) {
                query.OrderBy($"{bootstrap.sort} desc");
            }
            else {
                query.OrderBy($"{bootstrap.sort} asc");
            }

            var list1 = query.ToList();


            var query2 = _client.Queryable<Conceptionfundhis>()
            .Where(s => s.UpdateDate >= bootstrap.datemin.ToDateTimeB() && s.UpdateDate <= bootstrap.datemax.ToDateTimeE() && s.FundIn >= 0)
            .GroupBy(s =>
               s.platename
                  )
            //.Having(s => SqlFunc.AggregateCount(s.FundIn) >= 0)
            .Select(s => new ConceptionfundGroup() {
                platename = s.platename,
                Times = SqlFunc.AggregateCount(s.Id),
                FundInTotal = 0,
                CurrentFund = 0,
                TimeSpan = SqlFunc.AggregateCount(s.Id),
            });

            if (!bootstrap.search.IsEmpty()) {
                if (!(bootstrap.search.Contains(",") || bootstrap.search.Contains("£¬")))
                {
                    query2.Where((s) => s.platename.Contains(bootstrap.search));
                }
            }
            if (bootstrap.order.Equals("desc", StringComparison.OrdinalIgnoreCase)) {
                query2.OrderBy($"{bootstrap.sort} desc");
            }
            else {
                query2.OrderBy($"{bootstrap.sort} asc");
            }
            var list2 = query2.ToList();

            res = list1;

            int count = 0;
        

            var currentFundList = _client.Queryable<Conceptionfundhis>().Where(s => s.UpdateDate == bootstrap.datemax.ToDateTimeB()).OrderBy(s=>s.FundIn,OrderByType.Desc).ToList();

            if (bootstrap.sort== "FundInTotal") {
                foreach (var item in list1) {
                    item.id = count++;
                    var temp = list2.Find(s => s.platename == item.platename);
                    item.Times = temp == null ? -999 : temp.Times;

                    var currentOne = currentFundList.Find(s => s.platename == item.platename);
                    item.CurrentFund = currentOne == null ? -999 : currentOne.FundIn;
                }

                res = list1;
            }
            count = 0;
            if (bootstrap.sort == "Times") {
                foreach (var item in list2) {
                    item.id = count++;
                    var temp = list1.Find(s => s.platename == item.platename);
                    item.FundInTotal = temp == null ? -999 : temp.FundInTotal;

                    var currentOne = currentFundList.Find(s => s.platename == item.platename);
                    item.CurrentFund = currentOne == null ? -999 : currentOne.FundIn;
                }
                res = list2;
            }

            count = 0;
            if (bootstrap.sort == "CurrentFund")
            {
                List<ConceptionfundGroup> tempList = new List<ConceptionfundGroup>();
                foreach (var item in currentFundList)
                {
                    ConceptionfundGroup fundEntity = new ConceptionfundGroup();
                    var temp = list1.Find(s => s.platename == item.platename);
                    if (temp!=null)
                    {
                        fundEntity.CurrentFund = item.FundIn;

                        fundEntity.id = count++;
                        fundEntity.platename = temp.platename;
                        fundEntity.FundInTotal =  temp.FundInTotal;
                        fundEntity.TimeSpan = temp.TimeSpan;
                    }
                    var temp2 = list2.Find(s => s.platename == item.platename);
                    if (temp2 != null)
                    {
                        fundEntity.Times = temp2.Times;
                    }
                    tempList.Add(fundEntity);
                }

                res = tempList;
            }


            if (bootstrap.search!=null)
            {
                if (bootstrap.search.Contains(",")|| bootstrap.search.Contains("£¬"))
                {
                    List<ConceptionfundGroup> finalRes = new List<ConceptionfundGroup>();
                    finalRes = res.Where(s => s.FundInTotal >= fundInMin && s.FundInTotal <= fundInMax 
                    && s.Times >= TimesInMin && s.Times <= TimesInMax 
                    && s.CurrentFund >= CurrentFundMin && s.CurrentFund <= CurrentFundMax
                    ).ToList();
                    return Bootstrap.GridData(finalRes, totalNumber).JilToJson();
                }
            }
           
            return Bootstrap.GridData(res, totalNumber).JilToJson();
            //command = $"delete from conceptionfundhis where id> {id}";
            //res = _client.Ado.ExecuteCommand(command);

        }

        public string SectionPageList(Bootstrap.BootstrapParams bootstrap)
        {
            if (bootstrap.sort == "CreateDate")
            {
                bootstrap.sort = bootstrap.sort.Replace("CreateDate", "CreatTime");
            }
            var totalNumber = 0;
            if (bootstrap.offset != 0)
            {
                bootstrap.offset = bootstrap.offset / bootstrap.limit + 1;
            }
            //var order = ExpressionExt.InitO<Sys_user>(bootstrap.sort);

            var query = _client.Queryable<Sectionfund>();
            if (!bootstrap.search.IsEmpty())
            {
                query.Where((s) => s.platename.Contains(bootstrap.search));
            }
            if (!bootstrap.datemin.IsEmpty() && !bootstrap.datemax.IsEmpty())
            {
                query.Where(s => s.UpdateDate >= bootstrap.datemin.ToDateTimeB() && s.UpdateDate <= bootstrap.datemax.ToDateTimeE());
            }
            if (bootstrap.order.Equals("desc", StringComparison.OrdinalIgnoreCase))
            {
                query.OrderBy($"{bootstrap.sort} desc");
            }
            else
            {
                query.OrderBy($"{bootstrap.sort} asc");
            }
            var list = query.ToList();
            return Bootstrap.GridData(list, totalNumber).JilToJson();
        }

        public string PageListSectionGroup(Bootstrap.BootstrapParams bootstrap)
        {
            decimal fundInMin = 0;
            decimal fundInMax = 0;
            decimal TimesInMin = 0;
            decimal TimesInMax = 0;
            decimal CurrentFundMin = 0;
            decimal CurrentFundMax = 0;
            List<ConceptionfundGroup> res = new List<ConceptionfundGroup>();


            try
            {
                if (bootstrap.search != null && (bootstrap.search.Contains(",") || bootstrap.search.Contains("£¬")))
                {
                    var temp = bootstrap.search.Split(new string[] { ",", "£¬" }, StringSplitOptions.RemoveEmptyEntries);
                    fundInMin = Convert.ToDecimal(temp[0]);
                    fundInMax = Convert.ToDecimal(temp[1]);
                    CurrentFundMin = Convert.ToDecimal(temp[2]);
                    CurrentFundMax = Convert.ToDecimal(temp[3]);
                    TimesInMin = Convert.ToDecimal(temp[4]);
                    TimesInMax = Convert.ToDecimal(temp[5]);

                }
            }
            catch (Exception)
            {
                return null;
            }

            if (bootstrap.sort == "CreateDate")
            {
                bootstrap.sort = bootstrap.sort.Replace("CreateDate", "CreatTime");
            }
            var totalNumber = 0;
            if (bootstrap.offset != 0)
            {
                bootstrap.offset = bootstrap.offset / bootstrap.limit + 1;
            }

            var query = _client.Queryable<Sectionfundhis>()
             .Where(s => s.UpdateDate >= bootstrap.datemin.ToDateTimeB() && s.UpdateDate <= bootstrap.datemax.ToDateTimeE())
             .GroupBy(s =>
                s.platename
                   )
             .Select(s => new ConceptionfundGroup()
             {
                 platename = s.platename,
                 FundInTotal = SqlFunc.AggregateSum(s.FundIn),
                 Times = 0,
                 CurrentFund = 0,
                 TimeSpan = SqlFunc.AggregateCount(s.Id),
             });

            if (!bootstrap.search.IsEmpty())
            {
                if (!(bootstrap.search.Contains(",") || bootstrap.search.Contains("£¬")))
                {
                    query.Where((s) => s.platename.Contains(bootstrap.search));
                }

            }
            if (bootstrap.order.Equals("desc", StringComparison.OrdinalIgnoreCase))
            {
                query.OrderBy($"{bootstrap.sort} desc");
            }
            else
            {
                query.OrderBy($"{bootstrap.sort} asc");
            }

            var list1 = query.ToList();


            var query2 = _client.Queryable<Sectionfundhis>()
            .Where(s => s.UpdateDate >= bootstrap.datemin.ToDateTimeB() && s.UpdateDate <= bootstrap.datemax.ToDateTimeE() && s.FundIn >= 0)
            .GroupBy(s =>
               s.platename
                  )
            //.Having(s => SqlFunc.AggregateCount(s.FundIn) >= 0)
            .Select(s => new ConceptionfundGroup()
            {
                platename = s.platename,
                Times = SqlFunc.AggregateCount(s.Id),
                FundInTotal = 0,
                CurrentFund = 0,
                TimeSpan = SqlFunc.AggregateCount(s.Id),
            });

            if (!bootstrap.search.IsEmpty())
            {
                if (!(bootstrap.search.Contains(",") || bootstrap.search.Contains("£¬")))
                {
                    query2.Where((s) => s.platename.Contains(bootstrap.search));
                }
            }
            if (bootstrap.order.Equals("desc", StringComparison.OrdinalIgnoreCase))
            {
                query2.OrderBy($"{bootstrap.sort} desc");
            }
            else
            {
                query2.OrderBy($"{bootstrap.sort} asc");
            }
            var list2 = query2.ToList();

            res = list1;

            int count = 0;

            var currentFundList = _client.Queryable<Sectionfundhis>().Where(s => s.UpdateDate == bootstrap.datemax.ToDateTimeB()).OrderBy(s => s.FundIn, OrderByType.Desc).ToList();

            if (bootstrap.sort == "FundInTotal")
            {
                foreach (var item in list1)
                {
                    item.id = count++;
                    var temp = list2.Find(s => s.platename == item.platename);
                    item.Times = temp == null ? -999 : temp.Times;

                    var currentOne = currentFundList.Find(s => s.platename == item.platename);
                    item.CurrentFund = currentOne == null ? -999 : currentOne.FundIn;
                }

                res = list1;
            }
            count = 0;
            if (bootstrap.sort == "Times")
            {
                foreach (var item in list2)
                {
                    item.id = count++;
                    var temp = list1.Find(s => s.platename == item.platename);
                    item.FundInTotal = temp == null ? -999 : temp.FundInTotal;

                    var currentOne = currentFundList.Find(s => s.platename == item.platename);
                    item.CurrentFund = currentOne == null ? -999 : currentOne.FundIn;
                }
                res = list2;
            }

            count = 0;
            if (bootstrap.sort == "CurrentFund")
            {
                List<ConceptionfundGroup> tempList = new List<ConceptionfundGroup>();
                foreach (var item in currentFundList)
                {
                    ConceptionfundGroup fundEntity = new ConceptionfundGroup();
                    var temp = list1.Find(s => s.platename == item.platename);
                    if (temp != null)
                    {
                        fundEntity.CurrentFund = item.FundIn;

                        fundEntity.id = count++;
                        fundEntity.platename = temp.platename;
                        fundEntity.FundInTotal = temp.FundInTotal;
                        fundEntity.TimeSpan = temp.TimeSpan;
                    }
                    var temp2 = list2.Find(s => s.platename == item.platename);
                    if (temp2 != null)
                    {
                        fundEntity.Times = temp2.Times;
                    }
                    tempList.Add(fundEntity);
                }

                res = tempList;
            }


            if (bootstrap.search != null)
            {
                if (bootstrap.search.Contains(",") || bootstrap.search.Contains("£¬"))
                {
                    List<ConceptionfundGroup> finalRes = new List<ConceptionfundGroup>();
                    finalRes = res.Where(s => s.FundInTotal >= fundInMin && s.FundInTotal <= fundInMax
                    && s.Times >= TimesInMin && s.Times <= TimesInMax
                    && s.CurrentFund >= CurrentFundMin && s.CurrentFund <= CurrentFundMax
                    ).ToList();
                    return Bootstrap.GridData(finalRes, totalNumber).JilToJson();
                }
            }

            return Bootstrap.GridData(res, totalNumber).JilToJson();
            //command = $"delete from conceptionfundhis where id> {id}";
            //res = _client.Ado.ExecuteCommand(command);
        }

        public string QualifiedStockList(Bootstrap.BootstrapParams bootstrap) {

            var query = _client.Queryable<stockqualified>()
             .GroupBy(s =>
                s.SiftType
                   )
             .Select(s => new {
                 s.SiftType,
                 Count = SqlFunc.AggregateCount(s.StockNumber),
             }).OrderBy(s=>s.Count,OrderByType.Desc);
            var list = query.ToList();
            if (!bootstrap.search.IsEmpty())
            {
                list = list.Where((s) => s.SiftType.Contains(bootstrap.search)).ToList();
            }


            return Bootstrap.GridData(list, list.Count).JilToJson();
        }

        public string GetProcedure(Bootstrap.BootstrapParams bootstrap)
        {
            var query = _client.Queryable<Procedurelist>().OrderBy(s=>s.Order);
            var list = query.ToList();
            return list.JilToJson();
        }

        public class ConceptionfundGroup {
            
            public int id { set; get; }

            public string platename { set; get; }

            public decimal FundInTotal { set; get; }

            public decimal CurrentFund { set; get; }

            public decimal Times { set; get; }

            public decimal TimeSpan { set; get; }

        }

    }
}