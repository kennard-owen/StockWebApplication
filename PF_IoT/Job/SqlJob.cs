using PF.NetCore.DI;
using PF.Utils.Log;
using SqlSugar;
using System;

namespace KopSoftWms
{
    public class SqlJob
    {
        private readonly SqlSugarClient _client;

        public SqlJob(SqlSugarClient client)
        {
            _client = client;
        }

        public void Run()
        {
            var _nlog = ServiceResolve.Resolve<ILogUtil>();
            GC.Collect();
        }
    }
}