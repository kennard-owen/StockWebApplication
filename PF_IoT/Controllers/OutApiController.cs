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
using System.Net.Http;
using System.Threading.Tasks;

namespace PF_IoT.Controllers {
    public class OutApiController : BaseController {

        IHttpClientFactory _httpClientFactory;

        public OutApiController(IHttpClientFactory httpClientFactory) {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        [Route(nameof(SinaApi))]
        public async Task<IActionResult> SinaApi(string code) {
            var client = _httpClientFactory.CreateClient();
            var result = await client.GetStringAsync($"http://hq.sinajs.cn/list={code}");
            return Content(result);
        }
    }
}