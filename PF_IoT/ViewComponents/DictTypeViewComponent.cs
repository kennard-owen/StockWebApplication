using IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using PF.Utils.Extensions;
using PF.Utils.Pub;

namespace KopSoftWms.ViewComponents
{
    public class DictTypeViewComponent : ViewComponent
    {
        public DictTypeViewComponent()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var list = await EnumExt.ToKVListLinq<PubDictType>().ToAsync();
            return View(list);
        }
    }
}