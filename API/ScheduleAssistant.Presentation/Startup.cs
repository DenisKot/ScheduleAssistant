using System;
using System.IO;
using System.Reflection;
using System.Xml.XPath;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using ScheduleAssistant.Application;
using ScheduleAssistant.Common.DependencyInjection;
using ScheduleAssistant.Data;
using ScheduleAssistant.Data.EntityFramework;
using ScheduleAssistant.Data.Migrations;
using ScheduleAssistant.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using IHostingEnvironment = Microsoft.Extensions.Hosting.IHostingEnvironment;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ScheduleAssistant.Presentation
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            this.Configuration = builder.Build();
            this.HostingEnvironment = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }
        public ILifetimeScope AutofacContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry();

            services.AddDbContext<AppDbContext>(options =>
                //options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
                options.UseInMemoryDatabase("InMemoryAppDbContext")
                       .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)));

            services.AddCors();

            services.AddControllers()
                .AddNewtonsoftJson(options =>  options.SerializerSettings.Converters.Add(new StringEnumConverter()));

            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ScheduleAssistant API", Version = "v1" });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            services.AddSwaggerGenNewtonsoftSupport();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            loggerFactory.AddLog4Net();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ScheduleAssistant API V1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseCors(builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed(_ => true)
                .AllowCredentials());

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Run seed
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbCotnext = serviceScope.ServiceProvider.GetService<AppDbContext>();
                //dbCotnext.Database.EnsureCreated();
                dbCotnext.SeedData();
            }
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new DataModule());
            builder.RegisterModule(new InfrastructureModule());
            builder.RegisterModule(new ApplicationModule());

            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                .AssignableTo<ITransientDependency>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                .AssignableTo<ISingletonDependency>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}
