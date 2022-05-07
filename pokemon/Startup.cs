using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using pokemon.Core;
using pokemon.Models;
using System;

namespace pokemon
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();

            services.AddControllers();

            services.Configure<PokeAPISettings>(_configuration.GetSection("PokeAPISettings"));
            services.Configure<TranslatorAPISettings>(_configuration.GetSection("TranslatorAPISettings"));

            services.AddSingleton<IPokemonHttpClient, PokemonHttpClient>();
            services.AddSingleton<IPokeApiClient, PokeApiClient>();
            services.AddSingleton<ITranslatorClient, TranslatorClient>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.Use(async (context, next) =>
            {
                try
                {
                    await next();
                }
                catch (APIException ex)
                {
                    context.Response.StatusCode = ex.StatusCodes;
                    context.Response.ContentType = "text/plain";
                    await context.Response.WriteAsync($"{ex.Message} - Status code: {ex.StatusCodes}");
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "text/plain";
                    await context.Response.WriteAsync("Internla Error");
                }
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    public class ErrorDetails
    {
        public int StatusCode { get; set; }

        public string Message { get; set; }
    }
}
