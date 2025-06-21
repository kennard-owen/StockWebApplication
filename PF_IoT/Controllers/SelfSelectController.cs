using IServices;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System.Linq;
using PF.Core.Dto;
using PF.Core.Entity;
using PF.Core.Entity.Fluent.Validation;
using PF.NetCore.Attributes;
using PF.NetCore.NetCoreApp;
using PF.Utils.Extensions;
using PF.Utils.Pub;
using PF.Utils.Json;
using System;
using PF.Utils.Security;
using PF.Utils.Files;
using System.Collections.Generic;
using PF_IoT.Models;

namespace PF_IoT.Controllers {
    public class SelfSelectController : BaseController {
        private readonly ISelfSelectServices _selfselectServices;
        private readonly IstockServices _stockServices;
        private readonly SqlSugarClient _sqlSugarClient;

        public SelfSelectController(ISelfSelectServices selfselectServices, IstockServices stockServices, SqlSugarClient sqlSugarClient) {
            _selfselectServices = selfselectServices;
            _stockServices = stockServices;
            this._sqlSugarClient = sqlSugarClient;
        }

        [HttpGet]
        [CheckMenu]
        public IActionResult Index() {
            return View();
        }

        [HttpPost]
        [OperationLog(LogType.select)]
        public ContentResult List([FromForm]PubParams.FavorTypeBootstrapParams bootstrap) {
            var sd = _selfselectServices.PageList(bootstrap);
            return Content(sd);
        }


        [HttpGet]
        public IActionResult AskIfAddedToday(string StockNumber) {
            var entity = _selfselectServices.QueryableToEntity(c => c.StockNumber == StockNumber && c.IsDel==1);
           var flag = false;
            if (entity != null) {
                flag = true;
            }
            return BootJsonH(flag ? (flag,$"已经添加至【{entity.Operation}】") : (flag, PubConst.Add2));
        }


        //StockNumber, StockName, TradePlace, Date
        [HttpGet]
        public IActionResult Add(int IsStock,string StockNumber, string StockName, string TradePlace, string Date, string Whole_BeatHowMany, string Tec_BeatHowMany, string Fund_BeatHowMany, string Message_BeatHowMany, string Section_BeatHowMany, string Basic_BeatHowMany) {
            var model = new SelfSelect();
            model.StockNumber = StockNumber;
            model.StockName = StockName;
            model.TradePlace = TradePlace;
            model.IsStock = IsStock;



            //model.RecommendDate = string.Format("{0:yyyy-MM-dd}", Date).ToDateTime();
            model.RecommendDate = DateTime.Now;
            return View(model);
        }


        [HttpPost]
        public ContentResult GetSelfSelectId([FromForm]string stockNumber) {
            var sd = _selfselectServices.QueryableToEntity(s => s.StockNumber == stockNumber).SelfSelectId.ToString();
            return Content('A'+sd);
        }



        [HttpPost]
        public ContentResult ChangeName(string OperationName) {
            var entitys = _selfselectServices.QueryableToList(c => c.Operation == "自选" && c.IsDel == 1);
            foreach (var item in entitys) {
                item.Operation = DateTime.Now.ToString("yyyy-MM-dd_ms")+ OperationName;
            }
            var flag = _selfselectServices.Update(entitys);
            return BootJsonH(flag ? (flag, PubConst.Update1) : (flag, PubConst.Update2));
        }

        [HttpPost]
        [FilterXss]
        [OperationLog(LogType.addOrUpdate)]
        public IActionResult AddOrUpdate([FromForm]SelfSelect selfSelect, [FromForm]string Operation) {

         
            selfSelect.SelfSelectId = PubId.SnowflakeId;
            selfSelect.CreateBy = UserDtoCache.UserId;

            //var stock = _stockServices.Queryable().Where(s => s.StockNumber == selfSelect.StockNumber).OrderBy(s => s.Date, OrderByType.Desc).First();
            //selfSelect.Section = stock.Section;
            //selfSelect.Whole_BeatHowMany = stock.Whole_BeatHowMany;
            //selfSelect.Tec_BeatHowMany = stock.Tec_BeatHowMany;
            //selfSelect.Fund_BeatHowMany = stock.Fund_BeatHowMany;
            //selfSelect.Message_BeatHowMany = stock.Message_BeatHowMany;
            //selfSelect.Section_BeatHowMany = stock.Section_BeatHowMany;
            //selfSelect.Basic_BeatHowMany = stock.Basic_BeatHowMany;
            var entity = _selfselectServices.QueryableToEntity(c => c.StockNumber == selfSelect.StockNumber && c.Operation == selfSelect.Operation && c.IsDel == 1);
            if (entity != null) {
                return BootJsonH((false, $"已经添加至【{entity.Operation}】"));
            }

            bool flag = _selfselectServices.Insert(selfSelect);
            return BootJsonH(flag ? (flag, PubConst.Add1) : (flag, PubConst.Add2));
          
            //else {
            //    selfSelect.Id = Id.ToInt64();
            //    selfSelect.ModifiedBy = UserDtoCache.UserId;
            //    selfSelect.ModifiedDate = DateTimeExt.DateTime;
            //    var flag = _selfselectServices.Update(dict);
            //    return BootJsonH(flag ? (flag, PubConst.Update1) : (flag, PubConst.Update2));
            //}
             //return BootJsonH((PubEnum.Failed.ToInt32(), "未选择正确的操作"));
        }




    



        [HttpPost]
        public IActionResult AddSiftParaData([FromForm]string siftParaData, [FromForm] string StrategyId,[FromForm] string StockOrFund)
        {
            if (StrategyId==null)
            {
                StrategyId = "9999";
            }

           bool flag = false;
            if (siftParaData!=null)
            {
                var _record = _sqlSugarClient.Queryable<SiftPara>().Where(s => s.SiftParaData == siftParaData && s.StrategyId == SqlFunc.ToInt64(StrategyId)).First();
                if (_record == null)
                {
                    SiftPara data = new SiftPara();
                    data.Id = PubId.SnowflakeId;
                    data.SiftParaData = siftParaData;
                    data.StockOrFund = StockOrFund;
                    data.StrategyId = SqlFunc.ToInt64(StrategyId);
                    flag = _sqlSugarClient.Insertable(data).ExecuteCommand() > 0;
                }
            }
            return BootJsonH(flag ? (flag, PubConst.Add1) : (flag, PubConst.Add2));
        }

        [HttpPost]
        public IActionResult DeleteSiftParaData([FromForm]string siftParaData, [FromForm] string StrategyId)
        {

            bool flag = false;
            if (StrategyId==null)
            {
                var _record = _sqlSugarClient.Queryable<SiftPara>().Where(s => s.SiftParaData == siftParaData).First();
                if (_record != null)
                {
                    flag = _sqlSugarClient.Deleteable<SiftPara>().Where(s => s.SiftParaData == siftParaData).ExecuteCommand() > 0;
                }
            }
            else
            {
                var _record = _sqlSugarClient.Queryable<SiftPara>().Where(s => s.SiftParaData == siftParaData && s.StrategyId == SqlFunc.ToInt64(StrategyId)).First();
                if (_record != null)
                {
                    flag = _sqlSugarClient.Deleteable<SiftPara>().Where(s => s.SiftParaData == siftParaData && s.StrategyId == SqlFunc.ToInt64(StrategyId)).ExecuteCommand() > 0;
                }
            }

            return BootJsonH(flag ? (flag, PubConst.Delete1) : (flag, PubConst.Delete2));
        }

        [HttpPost]
        public ContentResult GetSiftParaData([FromForm] string StrategyId, [FromForm] string StockOrFund)
        {
            
            var hotConceptionList = _sqlSugarClient.Queryable<Conceptionfund>().Where(s => s.FundIn >=5).OrderBy(s => s.FundIn, OrderByType.Desc).Take(30).ToList();
            string hot = "@";
            foreach (var item in hotConceptionList)
            {
                hot = hot + item.platename+"|";
            }
            hot = hot.Substring(0, hot.Length - 1);
            SiftPara hotItme = new SiftPara() {
                Id = 1222,
                SiftParaData = hot
            };
            var query = _sqlSugarClient.Queryable<SiftPara>().Where(s => s.StockOrFund == StockOrFund);
            if (StrategyId!=null)
            {
                query = query.Where(s =>s.StrategyId == SqlFunc.ToInt64(StrategyId) || s.StrategyId == 9999);
            }
            var list = query.ToList();
            list.Add(hotItme);

            string res= list.JilToJson();
            return Content(res);
        }



        [HttpGet]
        public IActionResult AddManual(string id) {
            var model = new SelfSelect();
            if (id.IsEmpty()) {
                model.RecommendDate = DateTime.Now;
                return View(model);
            }
            else {
                model = _selfselectServices.QueryableToEntity(c => c.SelfSelectId == SqlFunc.ToInt64(id));
                return View(model);
            }
        }

        [HttpPost]
        [OperationLog(LogType = LogType.addOrUpdate)]
        public IActionResult AddOrUpdateManul([FromForm]SelfSelect selfselect, [FromForm]string id) {

         

            if (id.IsEmptyZero()) {
                //if (_selfselectServices.IsAny(c => c.StockNumber == selfselect.StockNumber)) {
                //    return BootJsonH((false, PubConst.Dept2));
                //}

                var entity = _selfselectServices.QueryableToEntity(c => c.StockNumber == selfselect.StockNumber&& c.Operation== selfselect.Operation && c.IsDel == 1);
                if (entity != null) {
                    return BootJsonH((false, $"已经添加至【{entity.Operation}】"));
                }
                //var stock = _stockServices.Queryable().Where(s => s.StockNumber == selfselect.StockNumber).OrderBy(s => s.Date, OrderByType.Desc).First();
                //selfselect.Section = stock.Section;
                //selfselect.Whole_BeatHowMany = stock.Whole_BeatHowMany;
                //selfselect.Tec_BeatHowMany = stock.Tec_BeatHowMany;
                //selfselect.Fund_BeatHowMany = stock.Fund_BeatHowMany;
                //selfselect.Message_BeatHowMany = stock.Message_BeatHowMany;
                //selfselect.Section_BeatHowMany = stock.Section_BeatHowMany;
                //selfselect.Basic_BeatHowMany = stock.Basic_BeatHowMany;
                selfselect.ModifiedDate = DateTimeExt.DateTime;
                selfselect.SelfSelectId = PubId.SnowflakeId;
                selfselect.CreateBy = UserDtoCache.UserId;
                bool flag = _selfselectServices.Insert(selfselect);
                return BootJsonH(flag ? (flag, PubConst.Add1) : (flag, PubConst.Add2));
            }
            else {
                var entity = _selfselectServices.QueryableToEntity(c => c.StockNumber == selfselect.StockNumber && c.Operation == selfselect.Operation && c.IsDel == 1);
                //if (entity != null) {
                //    return BootJsonH((false, $"已经添加至【{entity.Operation}】"));
                //}
                selfselect.SelfSelectId = id.ToInt64();
                selfselect.ModifiedBy = UserDtoCache.UserId;
                selfselect.ModifiedDate = DateTimeExt.DateTime;
                selfselect.RecommendDate = selfselect.RecommendDate;

                var flag = _selfselectServices.Update(selfselect);
                return BootJsonH(flag ? (flag, PubConst.Update1) : (flag, PubConst.Update2));
            }
        }


        [HttpGet]
        [OperationLog(LogType.delete)]
        public IActionResult Delete(string id) {
            //var flag = _selfselectServices.Update(new SelfSelect { SelfSelectId = SqlFunc.ToInt64(id), IsDel = 0, ModifiedBy = UserDtoCache.UserId, ModifiedDate = DateTimeExt.DateTime }, c => new { c.IsDel, c.ModifiedBy, c.ModifiedDate });
            var flag = _selfselectServices.Delete(s=>s.SelfSelectId == SqlFunc.ToInt64(id));
            return BootJsonH(flag ? (flag, PubConst.Delete1) : (flag, PubConst.Delete2));
        }


        [HttpPost]
        public IActionResult DeleteSelectOperation([FromForm]PubParams.FavorTypeBootstrapParams bootstrap) {
            //var flag = _selfselectServices.Update(new SelfSelect { SelfSelectId = SqlFunc.ToInt64(id), IsDel = 0, ModifiedBy = UserDtoCache.UserId, ModifiedDate = DateTimeExt.DateTime }, c => new { c.IsDel, c.ModifiedBy, c.ModifiedDate });
            var flag = _selfselectServices.Delete(s => bootstrap.CheckOperationList.Contains(s.Operation));
            return BootJsonH(flag ? (flag, PubConst.Delete1) : (flag, PubConst.Delete2));
        }


        //[HttpPost]
        //public IActionResult AddSelfList([FromForm] List<string> CodeList, [FromForm] int IsStock, [FromForm] string Type) {

        //    var flag2 = _selfselectServices.Delete(s => s.Operation=="自选");
        //    List<SelfSelect> myList = new List<SelfSelect>();
        //    foreach (var item in CodeList) {
        //        SelfSelect newItem = new SelfSelect();
        //        newItem.ModifiedDate = DateTimeExt.DateTime;
        //        newItem.SelfSelectId = PubId.SnowflakeId;
        //        newItem.CreateBy = UserDtoCache.UserId;
        //        newItem.StockNumber = item;
        //        newItem.Operation = "自选";
        //        newItem.IsStock = IsStock;
        //        myList.Add(newItem);
        //    }
        //    bool flag = _selfselectServices.Insert(myList);
        //    return BootJsonH(flag ? (flag, PubConst.Delete1) : (flag, PubConst.Delete2));
        //}



        [HttpPost]
        public IActionResult AddSelfList([FromForm] List<CodeList> CodeLists)
        {
            // 删除之前的"自选"操作
            var flag2 = _selfselectServices.Delete(s => s.Operation == "自选");

            // 构造新列表
            List<SelfSelect> myList = new List<SelfSelect>();
            foreach (var item in CodeLists)
            {
                SelfSelect newItem = new SelfSelect
                {
                    ModifiedDate = DateTimeExt.DateTime,
                    SelfSelectId = PubId.SnowflakeId,
                    CreateBy = UserDtoCache.UserId,
                    StockNumber = item.Code,
                    Operation = "自选",
                    IsStock = item.IsStock,
                    Type = item.Type,
                };
                myList.Add(newItem);
            }

            // 批量插入
            bool flag = _selfselectServices.Insert(myList);

            // 返回结果
            return BootJsonH(flag ? (flag, PubConst.Delete1) : (flag, PubConst.Delete2));
        }



        [HttpPost]
        public IActionResult AddSelfFundList([FromForm] List<CodeList> CodeLists)
        {
            // 删除之前的"自选"操作
            var flag2 = _selfselectServices.Delete(s => s.Operation == "自选");

            // 构造新列表
            List<SelfSelect> myList = new List<SelfSelect>();
            foreach (var item in CodeLists)
            {
                SelfSelect newItem = new SelfSelect
                {
                    ModifiedDate = DateTimeExt.DateTime,
                    SelfSelectId = PubId.SnowflakeId,
                    CreateBy = UserDtoCache.UserId,
                    StockNumber = item.Code,
                    Operation = "自选",
                    IsStock = 0,
                    Type = item.Type,
                };
                myList.Add(newItem);
            }

            // 批量插入
            bool flag = _selfselectServices.Insert(myList);

            // 返回结果
            return BootJsonH(flag ? (flag, PubConst.Delete1) : (flag, PubConst.Delete2));
        }


        public class CodeList
        {
            public string Code { get; set; }
            public int IsStock { get; set; }
            public string Type { get; set; }
        }



        [HttpPost]
        public ContentResult OperationList([FromForm] int IsStock) {
            var list = _selfselectServices.OperationList(IsStock);
            return Content(list);
        }


        [HttpPost]
        public ContentResult OperationListDates([FromForm] int IsStock)
        {
            var list = _selfselectServices.OperationListDates(IsStock);
            return Content(list);
        }

        [HttpPost]
        public ContentResult OperationListStrategys([FromForm] List<string> CreateDates, int IsStock)
        {
            var list = _selfselectServices.OperationListStrategys(CreateDates, IsStock);
            return Content(list);
        }


        [HttpPost]
        public ContentResult OperationListOperations([FromForm] List<string> CreateDates, List<string> Strategys,int IsStock)
        {
            var list = _selfselectServices.OperationListOperations(CreateDates, Strategys, IsStock);
            return Content(list);
        }

        [HttpGet]
        [OperationLog(LogType.export)]
        public IActionResult Export([FromQuery]PubParams.FavorTypeBootstrapParams bootstrap) {
            var buffer = _selfselectServices.ExportList(bootstrap);
            if (buffer.IsNull()) {
                return File(JsonL((false, PubConst.File8)).ToBytes(), ContentType.ContentTypeJson);
            }
            return File(buffer, ContentType.ContentTypeFile, DateTimeExt.GetDateTimeS(DateTimeExt.DateTimeFormatString) + "-" + EncoderUtil.UrlHttpUtilityEncoder("推荐列表") + ".xlsx");
        }
    }
}