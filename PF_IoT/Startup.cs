using IRepository;
using IServices;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository;
using Services;
using System;
using System.Text;
using PF.Core.Orm.SqlSugar;
using PF.NetCore.Attributes;
using PF.NetCore.Conventions;
using PF.NetCore.DI;
using PF.NetCoreApp.Extensions;
using PF.Utils.Json;
using MediatR;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using PF.NetCore;
//owen
using PF_IoT;
using PF.Utils.Pub;
using SqlSugar;

namespace PF
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        //IServiceProvider This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(option =>
            {
                option.Filters.Add<BaseExceptionAttribute>();
                //option.Filters.Add<FilterXSSAttribute>();
                option.Conventions.Add(new ApplicationDescription("title", Configuration["sys:title"]));
                option.Conventions.Add(new ApplicationDescription("company", Configuration["sys:company"]));
                option.Conventions.Add(new ApplicationDescription("customer", Configuration["sys:customer"]));
            });
            services.AddControllersWithViews();
            services.AddRazorPages()
                    .AddRazorRuntimeCompilation();
            //.SetCompatibilityVersion(CompatibilityVersion.Latest);
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});
            PubId.InitId();
            //services.AddTimedJob();
            services.AddOptions();
            services.AddXsrf();
            services.AddXss();
            services.AddAuthentication(c =>
            {
                c.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                c.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(cfg =>
            {
                cfg.LoginPath = "/Login/Index";
                cfg.LogoutPath = "/Login/Logout";
                //cfg.ExpireTimeSpan = TimeSpan.FromDays(1);
                //cfg.Cookie.Expiration = TimeSpan.FromDays(1);
                cfg.Cookie.Name = CookieAuthenticationDefaults.AuthenticationScheme;
                cfg.Cookie.Path = "/";
                cfg.Cookie.HttpOnly = true;
                //cfg.SlidingExpiration = true;
            });
            var sqlSugarConfig = SqlSugarConfig.GetConnectionString(Configuration);
            services.AddSqlSugarClient<SqlSugarClient>(config =>
            {
                config.ConnectionString = sqlSugarConfig.Item2;
                config.DbType = sqlSugarConfig.Item1;
                config.IsAutoCloseConnection = true;
                config.InitKeyType = InitKeyType.Attribute;
                //config.IsShardSameThread = true;
            }, ServiceLifetime.Transient);
            services.AddJson(o =>
            {
                o.JsonType = JsonType.Jil;
            });
            //services.AddDIProperty();
            services.AddHttpContextAccessor();
            services.AddHtmlEncoder();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //services.AddSingleton(typeof(ClientIpCheckFilter));
            //services.AddSingleton(typeof(Student));

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            services.AddBr(); //br压缩
            services.AddResponseCompression();//添加压缩
            services.AddResponseCaching(); //响应式缓存
            services.AddMemoryCache();
            //services.AddMediatR();
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(typeof(Startup).GetTypeInfo().Assembly);
            });
            //添加httpclient
            //services.AddHttpClient();
            //@1 DependencyInjection 注册
            services.AddNlog(); //添加Nlog
            RegisterBase(services);

            //owen
            services.AddSingleton<IRegisterApplicationService, RegisterApplicationService>();

            ServiceExtension.RegisterAssembly(services, "Services");
            ServiceExtension.RegisterAssembly(services, "Repository");


            SetServiceResolve(services);
            //var bulid = services.BuildServiceProvider();
            //ServiceResolve.SetServiceResolve(bulid);
        }

        private static void SetServiceResolve(IServiceCollection services)
        {
            var bulid = services.BuildServiceProvider();
            ServiceResolve.SetServiceResolve(bulid);
        }

        /// <summary>
        /// 泛型注册
        /// </summary>
        /// <param name="services"></param>
        /// <param name="injection"></param>
        private static void RegisterBase(IServiceCollection services, ServiceLifetime injection = ServiceLifetime.Scoped)
        {
            switch (injection)
            {
                case ServiceLifetime.Scoped:
                    services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
                    services.AddScoped(typeof(IBaseServices<>), typeof(BaseServices<>));
                    break;

                case ServiceLifetime.Singleton:
                    services.AddSingleton(typeof(IBaseRepository<>), typeof(BaseRepository<>));
                    services.AddSingleton(typeof(IBaseServices<>), typeof(BaseServices<>));
                    break;

                case ServiceLifetime.Transient:
                    services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
                    services.AddTransient(typeof(IBaseServices<>), typeof(BaseServices<>));
                    break;
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
               // app.UseDeveloperExceptionPage();
                app.UseExceptionHandler("/Home/OriginalSensorIndex");
            }
            else
            {
                app.UseExceptionHandler("/Home/OriginalSensorIndex");
            }
            app.UseStaticFiles(); //使用静态文件
            app.UseGlobalCore();
            app.UseExecuteTime();
            //app.UseTimedJob();
            app.UseResponseCompression();  //使用压缩
            app.UseResponseCaching();    //使用缓存

       
            app.UseCookiePolicy();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseStatusCodePagesWithRedirects("/Home/Error/{0}");
            app.UseStatusCodePagesWithRedirects("/Home/OriginalSensorIndex");


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "default",
                  pattern: "{controller=Stock}/{action=Index}/{id?}");

                //endpoints.MapAreaControllerRoute(
                //    name: "areas",
                //    areaName: "areas",
                //    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                //endpoints.MapRazorPages();
            });
            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Stock}/{action=Index}/{id?}");
            //    // Areas
            //    //routes.MapRoute(
            //    //    name: "areas",
            //    //    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
            //    //  );
            //});

            //ServiceResolve.Resolve<IRegisterApplicationService>().initRegister();
        }
    }
}