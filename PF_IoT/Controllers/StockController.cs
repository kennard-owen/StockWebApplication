using IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Diagnostics;
using System.Text;
using PF.Core.Dto;
using PF.Core.Entity;
using PF.Models;
using PF.NetCore.Attributes;
using PF.NetCore.NetCoreApp;
using PF.Utils.Pub;
using PF.Utils.Extensions;
using MediatR;
using System.Collections.Generic;
using PF.Utils.Json;
using PF_IoT.Models;
using PF.Utils.Table;

namespace PF_IoT.Controllers
{
    public class StockController : BaseController {
      
        private readonly IMediator _mediator;
        private readonly IstockServices _stockServices;
        private readonly ISys_dictServices _dictServices;

        public StockController( IMediator mediator,IstockServices stockServices, ISys_dictServices dictServices) {
            _mediator = mediator;
            _stockServices = stockServices;
            _dictServices = dictServices;
        }

        [CheckMenu]
        public IActionResult Index() {
            //var list =  _dictServices.Queryable().Where(c => c.IsDel == 1)
            // .ToListAsync();
            //ViewData["dic"] = list;
            return View();
        }


        [CheckMenu]
        public IActionResult FundIndex(string stocks)
        {
            //var list =  _dictServices.Queryable().Where(c => c.IsDel == 1)
            // .ToListAsync();
           
            return View();
        }

        [CheckMenu]
        public IActionResult FundStockOwned(string stocks,string info)
        {
            ViewData["stocks"] = stocks;
            ViewData["info"] = info;
            return View();
        }

        [HttpPost]
        public ContentResult GetStocksInFund([FromForm]string stocksInFund)
        {
            var para = stocksInFund.Split(",").CastToList<string>();
            var res = _stockServices.GetStocksInFund(para);
            return Content(res);
        }


        [HttpPost]
        public ContentResult GetData(PubParams.StrategyBootstrapParams bootstrap) {
            var list = _stockServices.HistoryChartALlStrategy(bootstrap);
            return Content(list);
        }



        [HttpPost]
        public ContentResult GetDataBySql(PubParams.StrategyBootstrapParams bootstrap) {
            var list = _stockServices.HistoryChartBySql(bootstrap);
            return Content(list);
        }


        [HttpPost]
        public ContentResult GetETFDataBySql(PubParams.StrategyBootstrapParams bootstrap)
        {
            var list = _stockServices.FundBySql(bootstrap);
            return Content(list);
        }


        [HttpPost]
        public ContentResult GetHotETFDataBySql(PubParams.StrategyBootstrapParams bootstrap) {
            var list = _stockServices.HotFundBySql(bootstrap);
            return Content(list);
        }


        [HttpPost]
        public ContentResult UpdateStocksTemp([FromForm]PubParams.ProcedureBootstrapParams bootstrap) {
            var res = _stockServices.UpdateStocksTemp(bootstrap);
            return Content(res.ToString());
        }

        [CheckMenu]
        public IActionResult SelfSelectView() {
            return View();
        }


        [CheckMenu]
        public IActionResult FundSelfSelectView()
        {
            return View();
        }

        [CheckMenu]
        public IActionResult SelfSelectStatistic()
        {
            return View();
        }

        [HttpPost]
        public ContentResult GetSelfSelectView(PubParams.FavorTypeBootstrapParams bootstrap) {
            var list = _stockServices.PageList(bootstrap);
            return Content(list);
        }

        [HttpPost]
        public ContentResult GetSelfSelectViewSimple(PubParams.FavorTypeBootstrapParams bootstrap) {
            var list = _stockServices.PageListSimple(bootstrap);
            return Content(list);
        }

        [HttpPost]
        public ContentResult GetFundSelfSelectView(PubParams.FavorTypeBootstrapParams bootstrap)
        {
            var list = _stockServices.FundPageList(bootstrap);
            return Content(list);
        }




      

        [CheckMenu]
        public IActionResult LatestInfoView(string StockNumber,string Date) {
            ViewData["StockNumber"] = StockNumber;
            ViewData["Date"] = Date;
            return View();
        }


        [HttpPost]
        public ContentResult LatestInfoViewData(PubParams.FavorTypeBootstrapParams bootstrap) {
            var list = _stockServices.GetList(bootstrap);
            return Content(list);
        }


        [HttpPost]
        public ContentResult DateList() {
            var list = _stockServices.GetDateList();
            return Content(list);
        }


        [CheckMenu]
        public IActionResult Whole() {
            var list = _dictServices.Queryable().Where(c => c.IsDel == 1)
             .ToListAsync();
            ViewData["dic"] = list;
            return View();
        }


        [HttpPost]
        public ContentResult GetAllStrategyData(PubParams.StrategyBootstrapParams bootstrap) {
            var list = _stockServices.HistoryChartALlStrategy(bootstrap);
            return Content(list);
        }



        [CheckMenu]
        public IActionResult QueryInfo(string StockNumber) {
            ViewData["StockNumber"] = StockNumber;
            return View();
        }




        [HttpPost]
        public ContentResult QueryInfoList(PubParams.FavorTypeBootstrapParams bootstrap) {
            var list = _stockServices.StockPageList(bootstrap);
            return Content(list);
        }

        [CheckMenu]
        public IActionResult QueryInfoTecPara(string StockNumber) {
            ViewData["StockNumber"] = StockNumber;
            return View();
        }


        [HttpPost]
        public ContentResult QueryInfoTecParaList(PubParams.FavorTypeBootstrapParams bootstrap) {
            var list = _stockServices.StockTecParaPageList(bootstrap);
            return Content(list);
        }


        [CheckMenu]
        public IActionResult AllIndex(string StockNumber)
        {
            return View();
        }

    }
}