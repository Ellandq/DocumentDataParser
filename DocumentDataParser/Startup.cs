using System.Configuration;
using DocumentDataParser.Services;
using Azure;
using Azure.AI.DocumentIntelligence;
using Microsoft.Extensions.Logging.AzureAppServices;

namespace DocumentDataParser
{
    public class Startup
    {   
        private const string KeyCode = "KEY_DOCUMENT_INTELLIGENCE";
        private const string EndpointCode = "ENDPOINT_DOCUMENT_INTELLIGENCE";

        // Testing
        private string key;
        private string endpoint;

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
                services.AddSingleton<DocumentIntelligenceClient>(provider =>
                {
                    var _configuration = provider.GetRequiredService<IConfiguration>();

                    // key = Environment.GetEnvironmentVariable(KeyCode);
                    // endpoint = Environment.GetEnvironmentVariable(EndpointCode);
                    key = System.Configuration.ConfigurationManager.AppSettings[KeyCode];
                    endpoint = System.Configuration.ConfigurationManager.AppSettings[EndpointCode];
                    var credential = new AzureKeyCredential(key);
                    return new DocumentIntelligenceClient(new Uri(endpoint), credential);
                });
            }catch(Exception e){
                Logger.LogError($"ERROR: {e.Message}");
            }

            services.AddScoped<IDataParser, DataParserService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            Logger.Configure(logger);


            try{
                Logger.LogInfo($"KEY: {key}");
                Logger.LogInfo($"ENDPOINT: {endpoint}");
            }catch (Exception e){
                Logger.LogError($"ERROR: {e.Message}");
            }

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
