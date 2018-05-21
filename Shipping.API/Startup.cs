namespace Shipping.API
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Shipping.API.Controllers;
    using Shipping.API.Token;
    using Swashbuckle.AspNetCore.Swagger;

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
            services.AddMvc();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Shipping API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMvc();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            //add the configuration settings
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddJsonFile(env.ContentRootPath + "/config.json")
                .AddJsonFile(env.ContentRootPath + "/config.development.json", true)
                .Build();

            //get token to check before start the method
            var _token = configuration.GetValue<string>("AuthHeader:TokenAPI");
            var _pwd = configuration.GetValue<string>("AuthHeader:PWAPI");
            TokenWebAPI.publicToken = _token;
            TokenWebAPI.pwdToken = _pwd;

            //get configuration cashe dev or homol
            //you can enable inside the json configuration file.
            var builder = new ConfigurationBuilder()
             .AddEnvironmentVariables()
             .AddJsonFile(env.ContentRootPath + "/appsettings.json")
             .AddJsonFile(env.ContentRootPath + "/appsettings.Development.json", true)
             .Build();

            //set the builder inside the class
            ShippingController.Configuration = builder;

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shipping API");
            });
        }
    }
}