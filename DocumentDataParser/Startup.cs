using Azure.AI.DocumentIntelligence;
using Azure;
using DocumentDataParser.Services;
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

            services.AddSingleton<DocumentIntelligenceClient>(provider =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                var _logger = provider.GetRequiredService<ILogger<Startup>>();
                var key = Environment.GetEnvironmentVariable(Prefix + KeyCode);
                var endpoint = Environment.GetEnvironmentVariable(Prefix + EndpointCode);

                _logger.LogInformation("Retrieved configuration values:");
                _logger.LogError($"Endpoint: {endpoint}");
                _logger.LogError($"Endpoint: {key}");

                if (string.IsNullOrEmpty(key))
                {
                    _logger.LogError("Key configuration value is missing or empty.");
                    throw new ArgumentException("Key configuration value is missing or empty.");
                }

                if (string.IsNullOrEmpty(endpoint))
                {
                    _logger.LogError("Endpoint configuration value is missing or empty.");
                    throw new ArgumentException("Endpoint configuration value is missing or empty.");
                }

                var credential = new AzureKeyCredential(key);
                return new DocumentIntelligenceClient(new Uri(endpoint), credential);            
                });

            services.AddScoped<IDataParser, DataParserService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            logger.LogInformation("Configuring the HTTP request pipeline.");

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
