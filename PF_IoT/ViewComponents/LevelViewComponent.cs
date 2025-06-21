using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PF.Utils.Extensions;
using PF.Utils.Pub;

namespace KopSoftWms.ViewComponents
{
    public class LevelViewComponent : ViewComponent
    {
        public LevelViewComponent()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var list = await EnumExt.ToKVListLinq<PubLevel>().ToAsync();
            return View(list);
        }
    }
}