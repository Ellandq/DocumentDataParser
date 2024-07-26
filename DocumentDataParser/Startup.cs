using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DocumentDataParser.Services;
using Azure;
using Azure.AI.DocumentIntelligence;
using Microsoft.Extensions.Logging.AzureAppServices;

namespace DocumentDataParser
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddApplicationInsightsTelemetry();
            
            services.Configure<AzureFileLoggerOptions>(options =>
            {
                options.FileName = "azure-diagnostics-";
                options.FileSizeLimit = 50 * 1024;
                options.RetainedFileCountLimit = 5;
            });

            services.AddSingleton<DocumentIntelligenceClient>(provider =>
            {
                var _configuration = provider.GetRequiredService<IConfiguration>();
                var key = _configuration["KEY_DOCUMENT_INTELLIGENCE"];
                var endpoint = _configuration["ENDPOINT_DOCUMENT_INTELLIGENCE"];
                var credential = new AzureKeyCredential(key);
                return new DocumentIntelligenceClient(new Uri(endpoint), credential);
            });

            services.AddScoped<IDataParser, DataParserService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
