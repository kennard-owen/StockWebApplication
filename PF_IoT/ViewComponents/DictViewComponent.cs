using IServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PF.Utils.Extensions;
using PF.Utils.Pub;

namespace KopSoftWms.ViewComponents
{
    public class DictViewComponent : ViewComponent
    {
        private readonly ISys_dictServices _dictServices;

        public DictViewComponent(ISys_dictServices dictServices)
        {
            _dictServices = dictServices;
        }

        public async Task<IViewComponentResult> InvokeAsync() {
            var list = await _dictServices.Queryable().Where(c => c.IsDel == 1)
               .ToListAsync();
            return View(list);
        }
    }
}