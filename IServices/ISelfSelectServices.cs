using PF.Core.Entity;
using PF.Core.Dto;
using System.Collections.Generic;

namespace IServices
{
    public interface ISelfSelectServices : IBaseServices<SelfSelect>
    {
        string PageList(PubParams.FavorTypeBootstrapParams bootstrap);

        string OperationList(int isStock);

        string OperationListDates(int isStock);

        string OperationListStrategys(List<string> CreateDates, int isStock);

        string OperationListOperations(List<string> CreateDates,List<string> Strategys, int IsStock);
        

        string DeleteSelectedStrategy(PubParams.FavorTypeBootstrapParams bootstrap);

        byte[] ExportList(PubParams.FavorTypeBootstrapParams bootstrap);
    }
}