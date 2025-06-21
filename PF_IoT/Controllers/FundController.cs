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
using PF.Utils.Table;

namespace PF_IoT.Controllers {
    public class FundController : BaseController {
        private readonly SqlSugarClient _sqlSugarClient;
        private readonly IFundServices _fundServices;
        public FundController(SqlSugarClient sqlSugarClient, IFundServices fundServices) {
            this._sqlSugarClient = sqlSugarClient;
            _fundServices = fundServices;
        }


        [CheckMenu]
        public IActionResult Fund()
        {
            return View();
        }

        [CheckMenu]
        public IActionResult SectionFund()
        {
            return View();
        }


        [CheckMenu]
        public IActionResult SectionFundGroup()
        {
            return View();
        }

        [CheckMenu]
        public IActionResult FundHis() {
            return View();
        }


        [HttpPost]
        public ContentResult List([FromForm]Bootstrap.BootstrapParams bootstrap) {
            var sd = _fundServices.PageList(bootstrap);
            return Content(sd);
        }


        [HttpGet]
        [OperationLog(LogType.delete)]
        public IActionResult Delete(string selfSelectId)
        {
            string command = $"delete from selfselects where selfSelectId={selfSelectId}; ";
            bool flag = _sqlSugarClient.Ado.ExecuteCommand(command) >= 0;
            return BootJsonH(flag ? (flag, PubConst.Delete1) : (flag, PubConst.Delete2));
        }



        [HttpPost]
        public ContentResult SectionList([FromForm] Bootstrap.BootstrapParams bootstrap)
        {
            var sd = _fundServices.SectionPageList(bootstrap);
            return Content(sd);
        }

        [HttpPost]
        public ContentResult ListSectionGroup([FromForm] Bootstrap.BootstrapParams bootstrap)
        {
            var sd = _fundServices.PageListSectionGroup(bootstrap);
            return Content(sd);
        }


        [HttpPost]
        public ContentResult ListHis([FromForm]Bootstrap.BootstrapParams bootstrap) {
            var sd = _fundServices.PageListHis(bootstrap);
            return Content(sd);
        }

        [CheckMenu]
        public IActionResult FundGroup() {
            return View();
        }

        [HttpPost]
        public ContentResult ListGroup([FromForm]Bootstrap.BootstrapParams bootstrap) {
            var sd = _fundServices.PageListGroup(bootstrap);
            return Content(sd);
        }


        [HttpPost]
        public ContentResult AddToHistory([FromForm]Bootstrap.BootstrapParams bootstrap)
        {
            string command = @"INSERT INTO conceptionfundhis(platecode,platename,cid,ConAmplitude,FundIn,UpRate,UpdateDate,CreatTime,Flag)
                    SELECT platecode, platename, cid, ConAmplitude, FundIn, UpRate, UpdateDate, CreatTime,1
                    FROM conceptionfunds; ";
            bool res = _sqlSugarClient.Ado.ExecuteCommand(command)>=0;
            return Content(res.ToString());
        }

        [HttpPost]
        public ContentResult RemoveToHistory([FromForm]Bootstrap.BootstrapParams bootstrap)
        {
            string command = @"delete from conceptionfundhis where Flag=1";
            bool res = _sqlSugarClient.Ado.ExecuteCommand(command) >= 0;
            return Content(res.ToString());
        }

        [HttpPost]
        public ContentResult AddHotConception([FromForm]string conception)
        {
            string command = @"INSERT INTO stockqualified(stockNumber,SiftType)
                            SELECT StockNumber,'热点'
                            FROM stockbaseinfoes
                            WHERE
                            Conception REGEXP '" + conception + "' and stockNumber not in (select stockNumber from stockqualified where SiftType='热点');";
            int number = _sqlSugarClient.Ado.ExecuteCommand(command);
            bool res = number >=0;
            return Content(new { res=res.ToString(),number= number }.JilToJson());
        }

        [HttpPost]
        public ContentResult DeleteHotConception([FromForm]string conception)
        {
            string command = @"delete from stockqualified where SiftType='热点'";
            int number = _sqlSugarClient.Ado.ExecuteCommand(command);
            bool res = number >= 0;
            return Content(new { res = res.ToString(), number = number }.JilToJson());
        }





        [HttpPost]
        public ContentResult SectionAddToHistory([FromForm] Bootstrap.BootstrapParams bootstrap)
        {
            string command = @"INSERT INTO Sectionfundhis(platename,ConAmplitude,FundIn,UpRate,UpdateDate,CreatTime,Flag)
                    SELECT platename, ConAmplitude, FundIn, UpRate, UpdateDate, CreatTime,1
                    FROM Sectionfunds; ";
            bool res = _sqlSugarClient.Ado.ExecuteCommand(command) >= 0;
            return Content(res.ToString());
        }

        [HttpPost]
        public ContentResult SectionRemoveToHistory([FromForm] Bootstrap.BootstrapParams bootstrap)
        {
            string command = @"delete from Sectionfundhis where Flag=1";
            bool res = _sqlSugarClient.Ado.ExecuteCommand(command) >= 0;
            return Content(res.ToString());
        }

        [HttpPost]
        public ContentResult SectionAddHotConception([FromForm] string conception)
        {
            string command = @"INSERT INTO stockqualified(stockNumber,SiftType)
                            SELECT StockNumber,'热点'
                            FROM stockbaseinfoes
                            WHERE
                            Conception REGEXP '" + conception + "' and stockNumber not in (select stockNumber from stockqualified where SiftType='热点');";
            int number = _sqlSugarClient.Ado.ExecuteCommand(command);
            bool res = number >= 0;
            return Content(new { res = res.ToString(), number = number }.JilToJson());
        }

        [HttpPost]
        public ContentResult SectionDeleteHotConception([FromForm] string conception)
        {
            string command = @"delete from stockqualified where SiftType='热点'";
            int number = _sqlSugarClient.Ado.ExecuteCommand(command);
            bool res = number >= 0;
            return Content(new { res = res.ToString(), number = number }.JilToJson());
        }

        [CheckMenu]
        public IActionResult QualifiedStock() {
            return View();
        }

        [HttpPost]
        public ContentResult QualifiedStockList([FromForm]Bootstrap.BootstrapParams bootstrap) {
            var sd = _fundServices.QualifiedStockList(bootstrap);
            return Content(sd);
        }



        [HttpPost]
        public ContentResult GetProcedure([FromForm]Bootstrap.BootstrapParams bootstrap)
        {
            var sd = _fundServices.GetProcedure(bootstrap);
            return Content(sd);
        }



    }
}