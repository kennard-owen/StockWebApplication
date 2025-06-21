using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PF.Core.Entity;
using PF.Utils.Env;
using IServices;
using PF.NetCore.DI;
using Microsoft.Extensions.DependencyInjection;
using PF.Utils.Log;

namespace KopSoftWms
{
    public class SysLogNotification : INotificationHandler<Sys_log>
    {
        public Task Handle(Sys_log notification, CancellationToken cancellationToken)
        {
            var _log = GlobalCore.GetRequiredService<ISys_logServices>();
            //var _log = ServiceResolve.Resolve<ISys_logServices>();
            _log.Insert(notification);
            return Task.CompletedTask;
        }
    }
}