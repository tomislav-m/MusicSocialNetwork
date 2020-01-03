using AutoMapper;
using Common.MessageContracts.Ticketing.Commands;
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
using TicketingService.Consumers;
using TicketingService.Models;
using TicketingService.Services;

namespace TicketingService
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
            services.AddDbContext<TicketingDbContext>(x => x.UseInMemoryDatabase("TicketsDb"));
            services.AddControllers();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<ITicketsService, TicketsService>();

            services.AddScoped<BuyTicketsConsumer>();
            services.AddScoped<GetEventTicketsConsumer>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<BuyTicketsConsumer>();
                x.AddConsumer<GetEventTicketsConsumer>();
            });

            services.AddSingleton(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("localhost", "/", h => { });
                cfg.ReceiveEndpoint("ticketing-service", e =>
                {
                    e.PrefetchCount = 16;
                    e.UseMessageRetry(x => x.Interval(2, 100));

                    e.Consumer<BuyTicketsConsumer>(provider);
                    EndpointConvention.Map<BuyTickets>(e.InputAddress);

                    e.Consumer<GetEventTicketsConsumer>(provider);
                    EndpointConvention.Map<GetEventTickets>(e.InputAddress);
                });
            }));

            services.AddSingleton<IPublishEndpoint>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<ISendEndpointProvider>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<IBus>(provider => provider.GetRequiredService<IBusControl>());

            services.AddScoped(provider => provider.GetRequiredService<IBus>().CreateRequestClient<BuyTickets>());
            services.AddScoped(provider => provider.GetRequiredService<IBus>().CreateRequestClient<GetEventTickets>());

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
