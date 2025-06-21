using System.Collections.Generic;
using PF.Core.Dto;
using PF.Core.Entity;
using PF.Utils.Table;

namespace IServices
{
    public interface IFundServices : IBaseServices<Conceptionfund>
    {
        string PageList(Bootstrap.BootstrapParams bootstrap);

        string SectionPageList(Bootstrap.BootstrapParams bootstrap);

        string PageListHis(Bootstrap.BootstrapParams bootstrap);

        string PageListGroup(Bootstrap.BootstrapParams bootstrap);

        string PageListSectionGroup(Bootstrap.BootstrapParams bootstrap);

        string QualifiedStockList(Bootstrap.BootstrapParams bootstrap);

        string GetProcedure(Bootstrap.BootstrapParams bootstrap);

    }
}