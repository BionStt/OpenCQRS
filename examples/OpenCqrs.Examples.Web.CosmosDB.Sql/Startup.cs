﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenCqrs.Bus.ServiceBus.Extensions;
using OpenCqrs.Examples.Domain.Commands;
using OpenCqrs.Examples.Reporting.Queries;
using OpenCqrs.Examples.Shared;
using OpenCqrs.Extensions;
using OpenCqrs.Store.CosmosDB.Sql.Configuration;
using OpenCqrs.Store.CosmosDB.Sql.Extensions;

namespace OpenCqrs.Examples.Web.CosmosDB.Sql
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddHttpContextAccessor();

            services
                .AddOpenCqrs(typeof(CreateProduct), typeof(GetProduct))
                .AddCosmosDbSqlProvider(Configuration)
                .AddServiceBusProvider(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IDispatcher dispatcher, IOptions<DomainDbConfiguration> settings)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseOpenCqrs().EnsureCosmosDbSqlDbCreated(settings);

            app.Run(async (context) =>
            {
                // Create a sample product loading data from domain events.
                var product = await GettingStarted.CreateProduct(dispatcher);

                // Display product title.
                await context.Response.WriteAsync($"Product title: {product.Title}");
            });
        }
    }
}
