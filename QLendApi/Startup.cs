using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using QLendApi.Helpers;
using QLendApi.Models;
using QLendApi.Repositories;
using QLendApi.Services;
using restapi.Settings;

namespace QLendApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCors();

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            var Mssqlsettings = Configuration.GetSection(nameof(MssqlSettings))
                .Get<MssqlSettings>();
            services.AddDbContext<QLendDBContext>((options) =>
            {
                var str = Mssqlsettings.ConnectionString;
                options.UseSqlServer(str);
            });

            
            services.AddScoped<IForeignWorkerService, ForeignWorkerService>();

            services.AddScoped<IForeignWorkerRepository, ForeignWorkerRepository>();
            services.AddScoped<ICertificateRepository, CertificateRepository>();
            services.AddScoped<ILoanRecordRepository, LoanRecordRepository>();
            services.AddScoped<IIncomeInformationRepository, IncomeInformationRepository>();
            services.AddScoped<IRepaymentRecordRepository, RepaymentRecordRepository>();
            
            services.AddControllersWithViews();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "QLend", Version = "v1" });
            });

            services.AddHealthChecks();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "QLend v1"));
            }

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapHealthChecks("/health/live", new HealthCheckOptions
                {
                    Predicate = (_) => false
                });
            });
        }
    }
}
