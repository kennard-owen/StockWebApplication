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
using Microsoft.AspNetCore.Http;
using System.IO;
using PF.Utils.Delegate;
using System.Collections.Generic;
using PF_IoT.Models;

namespace PF_IoT.Controllers
{
    public class DictController : BaseController
    {
        private readonly ISys_dictServices _dictServices;
        private readonly SqlSugarClient _sqlSugarClient;

        public DictController(ISys_dictServices dictServices, SqlSugarClient sqlSugarClient)
        {
            _dictServices = dictServices;
            this._sqlSugarClient = sqlSugarClient;
        }
        [HttpPost]
        public IActionResult GetDicList([FromForm] string dictType) {
            var list = _dictServices.Queryable().Where(c => c.IsDel == 1 && c.DictType== dictType)
                .Select(s=>new {
                DictId= s.DictId.ToString(),
                s.DictNo,
                s.DictName,
                s.SQL ,
                s.DictType,
                s.IsDel,
                s.Remark,
                s.StrategyImg
                })
           .ToList().JilToJson();


            return Content(list);
        }


        [HttpGet]
        [CheckMenu]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [CheckMenu]
        public IActionResult Tool()
        {
            return View();
        }

        [HttpPost]
        //[FilterXss]
        public IActionResult UpdateTransactionInfo([FromForm] TransactionDto tran) {
            tran.template = tran.template.Replace("'", "\\'");
            tran.content = tran.content.Replace("'", "\\'");
            string sql = $"update transactioninfo set Content='{tran.content}',Template='{tran.template}'";
            

            var flag = _sqlSugarClient.Ado.ExecuteCommand(sql) >= 0;
            return BootJsonH(flag ? (flag, PubConst.Add1) : (flag, PubConst.Add2));
        }

        [HttpPost]
        public IActionResult CreateTransaction([FromForm] string[] pros)
        {
            string sql = "";
            int flagCount = 0;
            bool flag = false;
            foreach (var sqlItem in pros)
            {
                try
                {
                    if (_sqlSugarClient.Ado.ExecuteCommand(sqlItem) >= 0)
                    {
                        flagCount = flagCount + 1;
                    }
                }
                catch (System.Exception ex)
                {
                    return BootJsonH(flag ? (flag, PubConst.Add1) : (flag, PubConst.Add2));
                }
                //sql = item.Replace("'", "\\'");
             
            }
            flag = flagCount == pros.Count();
            return BootJsonH(flag  ? (flag, PubConst.Add1) : (flag, PubConst.Add2));
        }
        [HttpPost]
        public ContentResult GetTransactionInfo() {
            string sql = $"select Content,Template from transactioninfo";
            var sd = _sqlSugarClient.Ado.SqlQuerySingle<TransactionDto>(sql).JilToJson();
            return Content(sd);
        }


        [HttpPost]
        [OperationLog(LogType.select)]
        public ContentResult List([FromForm]PubParams.DictBootstrapParams bootstrap)
        {
            var sd = _dictServices.PageList(bootstrap);
            return Content(sd);
        }

        [HttpGet]
        public IActionResult Add(string id)
        {
            var model = new Sys_dict();
            if (id.IsEmpty())
            {
                return View(model);
            }
            else
            {
                model = _dictServices.QueryableToEntity(c => c.DictId == SqlFunc.ToInt64(id));
                return View(model);
            }
        }

        [HttpPost]
        //[FilterXss]
        [OperationLog(LogType.addOrUpdate)]
        public IActionResult AddOrUpdate([FromForm]Sys_dict dict, [FromForm]string id,[FromForm] string PreId)
        {
            var validator = new SysDictFluent();
            var results = validator.Validate(dict);
            var success = results.IsValid;
            if (!success)
            {
                //string msg = results.Errors.Aggregate("", (current, item) => current + (item.ErrorMessage + "</br>"));
                string msg = results.Errors.Aggregate("", (current, item) => (current + item.ErrorMessage + "</br>"));
                return BootJsonH((PubEnum.Failed.ToInt32(), msg));
            }
            if (id.IsEmptyZero())
            {
                if (_dictServices.IsAny(c => c.DictName == dict.DictName))
                {
                    return BootJsonH((false, PubConst.Dict1));

                }
                
                dict.SQL = dict.SQL.Replace("&gt;", ">").Replace("&lt;", "<").Replace("&#x2B;", "+");
                dict.Remark = dict.Remark?.Replace("&gt;", ">").Replace("&lt;", "<").Replace("&#x2B;", "+");
                dict.DictId = PubId.SnowflakeId;
                dict.CreateBy = UserDtoCache.UserId;
                //dict.DictType=1


                var siftParaList = _sqlSugarClient.Queryable<SiftPara>().Where(s => s.StrategyId == SqlFunc.ToInt64(PreId)).ToList();
                List<SiftPara> list = new List<SiftPara>();
                foreach (var item in siftParaList)
                {
                    list.Add(new SiftPara()
                    {
                        Id = PubId.SnowflakeId,
                        SiftParaData = item.SiftParaData,
                        StrategyId= dict.DictId,
                        StockOrFund= item.StockOrFund
                    });
                }
                if (list.Count>0)
                {
                    var res = _sqlSugarClient.Insertable<SiftPara>(list).ExecuteCommand();
                }
                bool flag = _dictServices.Insert(dict);
                return BootJsonH(flag ? (flag, PubConst.Add1) : (flag, PubConst.Add2));
            }
            else
            {
                dict.SQL = dict.SQL?.Replace("&gt;", ">").Replace("&lt;", "<").Replace("&#x2B;", "+");
                dict.Remark = dict.Remark?.Replace("&gt;", ">").Replace("&lt;", "<").Replace("&#x2B;", "+");
                dict.DictId = id.ToInt64();
                dict.ModifiedBy = UserDtoCache.UserId;
                dict.ModifiedDate = DateTimeExt.DateTime;
                var flag = _dictServices.Update(dict);
                return BootJsonH(flag ? (flag, PubConst.Update1) : (flag, PubConst.Update2));
            }
        }

        [HttpGet]
        [OperationLog(LogType.delete)]
        public IActionResult Delete(string id)
        {
            var flag = _dictServices.Update(new Sys_dict { DictId = SqlFunc.ToInt64(id), IsDel = 0, ModifiedBy = UserDtoCache.UserId, ModifiedDate = DateTimeExt.DateTime }, c => new { c.IsDel, c.ModifiedBy, c.ModifiedDate });
            return BootJsonH(flag ? (flag, PubConst.Delete1) : (flag, PubConst.Delete2));
        }


        [HttpPost]
        public IActionResult DeleteSelectStrategy([FromForm]PubParams.StrategyBootstrapParams bootstrap) {
            var flag1 = _dictServices.Delete(s => bootstrap.StrategyIdList.Contains(s.DictId.ToString()));
            var flag2 = _sqlSugarClient.Deleteable<SiftPara>().Where(s => bootstrap.StrategyIdList.Contains(s.StrategyId.ToString())).ExecuteCommand() >= 0;
            var flag = flag1 & flag2;
            return BootJsonH(flag ? (flag, PubConst.Delete1) : (flag, PubConst.Delete2));
        }



        public IActionResult UploadStrategyImg(IFormFile file,string id)
        {
            if (file == null || file.Length <= 0)
            {
                return BootJsonH((false, PubConst.File1));
            }
            string fileExt = Path.GetExtension(file.FileName).ToLower();
            var img = Path.Combine("upload", "strategy", file.FileName+PubId.GetUuid()) + fileExt;
            var filepath = Path.Combine(WebRoot, img);
            return DelegateUtil.TryExecute<IActionResult>(() =>
            {
                using (var stream = new FileStream(filepath, FileMode.CreateNew))
                {
                    file.CopyTo(stream);
                }
                var flag = _dictServices.Update(new Sys_dict { DictId = SqlFunc.ToInt64(id), StrategyImg = img}, c => new { c.StrategyImg });
                return BootJsonH(flag ? (flag, PubConst.File6) : (flag, PubConst.File7));
            }, BootJsonH((false, PubConst.File5))
             );
        }
    }
}