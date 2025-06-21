﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;
using PF.Utils.Log;
using SqlSugar;

namespace PF.Core.Orm.SqlSugar
{
    public static class SqlSugarServiceCollectionExt
    {
        public static IServiceCollection AddSqlSugarDao(this IServiceCollection services, Action<ConnectionConfig> configAction, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            var config = new ConnectionConfig();
            configAction.Invoke(config);
            switch (lifetime)
            {
                case ServiceLifetime.Singleton:
                    services.AddSingleton(new SqlSugarDao(config));
                    break;

                case ServiceLifetime.Scoped:
                    services.AddScoped(serviceProvider =>
                    {
                        return new SqlSugarDao(config);
                    });
                    break;

                case ServiceLifetime.Transient:
                    services.AddTransient(serviceProvider =>
                    {
                        return new SqlSugarDao(config);
                    });
                    break;

                default:
                    services.AddScoped(serviceProvider =>
                    {
                        return new SqlSugarDao(config);
                    });
                    break;
            }

            return services;
        }

        public static IServiceCollection AddSqlSugarClient<T>(this IServiceCollection services, Action<ConnectionConfig> configAction, ServiceLifetime lifetime = ServiceLifetime.Scoped) where T : SqlSugarClient
        {
            switch (lifetime)
            {
                case ServiceLifetime.Singleton:

                    services.AddSingleton(serviceProvider =>
                    {
                        var config = new ConnectionConfig()
                        {
                            ConfigureExternalServices = new ConfigureExternalServices
                            {
                                SqlFuncServices = SqlSugarConfig.GetLambda()
                            }
                        };
                        configAction.Invoke(config);
                        var log = serviceProvider.GetRequiredService<ILogUtil>();
                        var db = new SqlSugarClient(config)
                        {
                            MappingTables = SqlSugarConfig.listTable
                        };
                        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                        string flag = configuration["Log:sqllog"];
                        if (string.IsNullOrWhiteSpace(flag))
                        {
                            flag = "false";
                        }
                        if (flag.Equals("true", StringComparison.OrdinalIgnoreCase))
                        {
                            db.Ado.IsEnableLogEvent = true;
                            //SQL执行完事件
                            db.Aop.OnLogExecuted = (sql, pars) =>
                            {
                                log.Info(sql);
                            };
                            //SQL执行前事件
                            db.Aop.OnLogExecuting = (sql, pars) =>
                            {
                                log.Info(db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                            };
                            db.Aop.OnError = (exp) =>//执行SQL 错误事件
                            {
                                log.Debug(exp.Message);
                            };
                        }
                        else
                        {
                            db.Ado.IsEnableLogEvent = false;
                        }

                        return (T)db;
                    });
                    break;

                case ServiceLifetime.Scoped:

                    services.AddScoped(serviceProvider =>
                    {
                        var configS = new ConnectionConfig()
                        {
                            ConfigureExternalServices = new ConfigureExternalServices
                            {
                                SqlFuncServices = SqlSugarConfig.GetLambda()
                            }
                        };
                        configAction.Invoke(configS);
                        var db = new SqlSugarClient(configS)
                        {
                            MappingTables = SqlSugarConfig.listTable
                        };
                        var log = serviceProvider.GetRequiredService<ILogUtil>();
                        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                        string flag = configuration["Log:sqllog"];
                        if (string.IsNullOrWhiteSpace(flag))
                        {
                            flag = "false";
                        }
                        if (flag.Equals("true", StringComparison.OrdinalIgnoreCase))
                        {
                            db.Ado.IsEnableLogEvent = true;
                            //SQL执行完事件
                            db.Aop.OnLogExecuted = (sql, pars) =>
                            {
                                log.Info(sql);
                            };
                            //SQL执行前事件
                            db.Aop.OnLogExecuting = (sql, pars) =>
                            {
                                log.Info(db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                            };
                            db.Aop.OnError = (exp) =>//执行SQL 错误事件
                            {
                                log.Debug(exp.Message + ";;" + exp.Sql);
                            };
                        }
                        else
                        {
                            db.Ado.IsEnableLogEvent = false;
                        }

                        return (T)db;
                    });
                    break;

                case ServiceLifetime.Transient:
                    services.AddTransient(serviceProvider =>
                    {
                        var configT = new ConnectionConfig()
                        {
                            ConfigureExternalServices = new ConfigureExternalServices
                            {
                                SqlFuncServices = SqlSugarConfig.GetLambda()
                            }
                        };
                        configAction.Invoke(configT);
                        var db = new SqlSugarClient(configT)
                        {
                            MappingTables = SqlSugarConfig.listTable
                        };
                        var log = serviceProvider.GetRequiredService<ILogUtil>();
                        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                        string flag = configuration["Log:sqllog"];
                        if (string.IsNullOrWhiteSpace(flag))
                        {
                            flag = "false";
                        }
                        if (flag.Equals("true", StringComparison.OrdinalIgnoreCase))
                        {
                            db.Ado.IsEnableLogEvent = true;
                            //SQL执行完事件
                            db.Aop.OnLogExecuted = (sql, pars) =>
                            {
                                log.Info(sql);
                            };
                            //SQL执行前事件
                            db.Aop.OnLogExecuting = (sql, pars) =>
                            {
                                log.Info(db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                            };
                            db.Aop.OnError = (exp) =>//执行SQL 错误事件
                            {
                                log.Debug(exp.Message);
                            };
                        }
                        else
                        {
                            db.Ado.IsEnableLogEvent = false;
                        }
                        return (T)db;
                    });
                    break;
            }
            return services;
        }
    }
}