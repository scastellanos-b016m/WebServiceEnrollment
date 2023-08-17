// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WebServiceEnrollment.Services;
using SoapCore;

namespace WebServiceEnrollment
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.TryAddSingleton<IEnrollmentService, EnrollmentService>();
            services.AddMvc();
            services.AddSoapCore();
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env, IConfiguration Configuration)
        {
            Console.WriteLine(Configuration.GetConnectionString("defaultConnection"));
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSoapEndpoint<IEnrollmentService>("/EnrollmentService.asmx", new SoapEncoderOptions(), SoapSerializer.XmlSerializer);
        }
    }
}