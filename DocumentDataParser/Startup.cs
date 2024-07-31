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
        private const string Prefix = "APPSETTINGS_";
        private const string KeyCode = "KEY_DOCUMENT_INTELLIGENCE";
        private const string EndpointCode = "ENDPOINT_DOCUMENT_INTELLIGENCE";

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

            services.AddSingleton<ILogger>(provider => 
                provider.GetRequiredService<ILogger<Startup>>()
            );

            try{
                _ = services.AddSingleton<DocumentIntelligenceClient>(provider =>
                {
                    var _configuration = provider.GetRequiredService<IConfiguration>();
                    var key = _configuration[Prefix + KeyCode];
                    var endpoint = _configuration[Prefix + EndpointCode];
                    var credential = new AzureKeyCredential(key);
                    return new DocumentIntelligenceClient(new Uri(endpoint), credential);
                });
            }catch (Exception e){
                Console.WriteLine(e.Message);
            }

            services.AddScoped<IDataParser, DataParserService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            Logger.Configure(logger);

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
