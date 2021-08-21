using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using QLendApi.Services;
using QLendApi.Dtos;
using QLendApi.Helpers;
using QLendApi.Models;
using QLendApi.Repositories;
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
                var str = Configuration.GetConnectionString("MssqlConnectString");
                if (str != null)
                {
                    options.UseSqlServer(str);
                }
                var localStr = Mssqlsettings.ConnectionString;
                options.UseSqlServer(localStr);

            });


            services.AddScoped<IForeignWorkerService, ForeignWorkerService>();
            services.AddScoped<IForeignWorkerRepository, ForeignWorkerRepository>();
            services.AddScoped<ICertificateRepository, CertificateRepository>();
            services.AddScoped<ILoanRecordRepository, LoanRecordRepository>();
            services.AddScoped<IIncomeInformationRepository, IncomeInformationRepository>();
            services.AddScoped<IRepaymentRecordRepository, RepaymentRecordRepository>();
            services.AddScoped<INoticeRepository, NoticeRepository>();
            services.AddScoped<ILoanRecordService, LoanRecordService>();
            services.AddScoped<ISmsService, SmsService>();

            services.AddControllersWithViews();
           // services.AddControllers().AddNewtonsoftJson();
           

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "QLend", Version = "v1" });
            });

            services.AddHealthChecks();

            services.AddSingleton<INotificationService, NotificationHubService>();

            services.AddOptions<NotificationHubOptions>()
                .Configure(Configuration.GetSection("NotificationHub").Bind)
                .ValidateDataAnnotations();

            services.AddOptions<SmsServiceOptions>()
                .Configure(Configuration.GetSection("SmsService").Bind)
                .ValidateDataAnnotations();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // if (env.IsDevelopment())
            // {
            //     app.UseDeveloperExceptionPage();
            //     app.UseSwagger();
            //     app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "QLend v1"));
            // }
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "QLend v1"));

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
