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

namespace PF_IoT.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ISys_userServices _userServices;
        private readonly ISys_logServices _logServices;
        private readonly ISys_roleServices _roleServices;
        
        private readonly IMemoryCache _cache;
        private readonly IMediator _mediator;
    

        private readonly IstockServices _dev_payloadServices;

        public HomeController(ISys_logServices logServices,
            ISys_userServices sysUserServices,
            ISys_roleServices roleServices, IMemoryCache cache,
            IMediator mediator, 
            IstockServices dev_payloadServices)
        {
            _logServices = logServices;
            _userServices = sysUserServices;
            _roleServices = roleServices;
            _cache = cache;
            _mediator = mediator;
          
            _dev_payloadServices = dev_payloadServices;

        }

        public IActionResult Index()
        {
            //TempData["returnUrl"] = returnUrl;
            _userServices.Login(UserDtoCache.UserId, GetIp());
            _mediator.Publish(new Sys_log
            {
                LogId = PubId.SnowflakeId,
                Browser = GetBrowser(),
                CreateBy = UserDtoCache.UserId,
                Description = $"{UserDtoCache.UserNickname}登录成功",
                LogIp = GetIp(),
                Url = GetUrl(),
                LogType = LogType.login.EnumToString(),
            });
            ViewBag.title = GetDescriptor("title");
            ViewBag.company = GetDescriptor("company");
            ViewBag.customer = GetDescriptor("customer");
            ViewBag.nickname = UserDtoCache.UserNickname;
            ViewBag.headimg = UserDtoCache.HeadImg;
            //菜单
            var menus = _roleServices.GetMenu(UserDtoCache.RoleId.Value);
            GetMemoryCache.Set("menu", menus);
            ViewData["menu"] = menus;

            return View();
        }

        [CheckMenu]
        public IActionResult OriginalSensorIndex() {
            //菜单
            _userServices.Login(UserDtoCache.UserId, GetIp());
            ViewBag.title = GetDescriptor("title");
            ViewBag.company = GetDescriptor("company");
            ViewBag.customer = GetDescriptor("customer");
            ViewBag.nickname = UserDtoCache.UserNickname;
            ViewBag.headimg = UserDtoCache.HeadImg;


            var menus = _roleServices.GetMenu(UserDtoCache.RoleId.Value);
            GetMemoryCache.Set("menu", menus);
            ViewData["menu"] = menus;

            return View();
        }



        //[HttpPost]
        public ContentResult HistoryChart([FromForm]ParaInterval para) {
            var list = _dev_payloadServices.IntervalData(para.imei, para.interval, para.timeBegin, para.timeEnd);
            return Content(list);
        }


        public ContentResult PieChart([FromForm]ParaInterval para) {
            var pie = _dev_payloadServices.PieChart(para.imei, para.empty, para.full, para.timeBegin, para.timeEnd);
            return Content(pie);
        }




        [AddHeader("Content-Type", PF.Utils.Files.ContentType.ContentTypeSSE)]
        [AddHeader("Cache-Control", PF.Utils.Files.ContentType.CacheControl)]
        [AddHeader("Connection", PF.Utils.Files.ContentType.Connection)]
        public IActionResult ServerSendMsg()
        {
            var a = new ServerSentEventsDto
            {
                Data = DateTime.Now.ToString(),
                Event = "message",
                Id = Guid.NewGuid().ToString(),
                Retry = "1000",
            };
            StringBuilder sb = new StringBuilder();
            sb.Append($"id:{a.Id}\n");
            sb.Append($"retry:{a.Retry}\n");
            sb.Append($"event:{a.Event}\n");
            sb.Append($"data:{a.Data}\n\n");
            return Content(sb.ToString());
        }

        public IActionResult Welcome()
        {
            ViewBag.title = GetDescriptor("title");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}