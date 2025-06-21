using PF.Core.Dto;
using PF.Core.Entity;
using PF.Utils.Table;
using System.Collections.Generic;

namespace IServices
{
    public interface IstockServices : IBaseServices<Stock>
    {
        string PageList(PubParams.FavorTypeBootstrapParams bootstrap);

        string PageListSimple(PubParams.FavorTypeBootstrapParams bootstrap);
        string FundPageList(PubParams.FavorTypeBootstrapParams bootstrap);
        
        string HistoryChartALlStrategy(PubParams.StrategyBootstrapParams bootstrap);

        string HistoryChartBySql(PubParams.StrategyBootstrapParams bootstrap);


        string IntervalData(string imei, int interval, string timeBegin, string timeEnd);

        string PieChart(string imei, int empty, int full, string timeBegin, string timeEnd);

        string GetList(PubParams.FavorTypeBootstrapParams bootstrap);

        string GetDateList();

        string StockPageList(PubParams.FavorTypeBootstrapParams bootstrap);


        string StockTecParaPageList(PubParams.FavorTypeBootstrapParams bootstrap);

        bool UpdateStocksTemp(PubParams.ProcedureBootstrapParams bootstrap);

        string FundBySql(PubParams.StrategyBootstrapParams bootstrap);

        string HotFundBySql(PubParams.StrategyBootstrapParams bootstrap);

        string GetStocksInFund(List<string> stocksInFund);



    }


}