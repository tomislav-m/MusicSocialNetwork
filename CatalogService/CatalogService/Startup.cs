using AutoMapper;
using CatalogService.Consumers;
using CatalogService.Helpers;
using CatalogService.MessageContracts;
using CatalogService.Models;
using CatalogService.Services;
using Common.MessageContracts.Catalog.Commands;
using Common.Services;
using GreenPipes;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace CatalogService
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
            services.AddDbContext<ApplicationDbContext>(x => x.UseInMemoryDatabase("CatalogDb"));
            services.AddControllers();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IRatingService, RatingService>();
            services.AddScoped<ICollectionService, CollectionService>();

            services.AddScoped<RateAlbumConsumer>();
            services.AddScoped<CollectionConsumer>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<RateAlbumConsumer>();
                x.AddConsumer<CollectionConsumer>();
            });

            services.AddSingleton(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("localhost", "/", h => { });
                cfg.ReceiveEndpoint("catalog-service", e =>
                {
                    e.PrefetchCount = 16;
                    e.UseMessageRetry(x => x.Interval(2, 100));

                    e.Consumer<RateAlbumConsumer>(provider);
                    EndpointConvention.Map<RateAlbum>(e.InputAddress);
                    EndpointConvention.Map<GetAverageRating>(e.InputAddress);

                    e.Consumer<CollectionConsumer>(provider);
                    EndpointConvention.Map<AddToCollection>(e.InputAddress);
                });
            }));

            services.AddSingleton<IPublishEndpoint>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<ISendEndpointProvider>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<IBus>(provider => provider.GetRequiredService<IBusControl>());

            services.AddScoped(provider => provider.GetRequiredService<IBus>().CreateRequestClient<RateAlbum>());
            services.AddScoped(provider => provider.GetRequiredService<IBus>().CreateRequestClient<GetAverageRating>());
            services.AddScoped(provider => provider.GetRequiredService<IBus>().CreateRequestClient<AddToCollection>());
            services.AddSingleton<IHostedService, BusService>();

            services.AddScoped<EventStoreService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
