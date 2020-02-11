using AutoMapper;
using Common.MessageContracts.Event.Commands;
using Common.Services;
using EventService.Consumers;
using EventService.Models;
using EventService.Services;
using GreenPipes;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace EventService
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
            services.AddDbContext<EventsDbContext>(x => x.UseInMemoryDatabase("EventDb"));
            services.AddControllers();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IEventService, Services.EventService>();

            services.AddScoped<AddEventConsumer>();
            services.AddScoped<EditEventConsumer>();
            services.AddScoped<GetEventConsumer>();
            services.AddScoped<MarkEventConsumer>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<AddEventConsumer>();
                x.AddConsumer<EditEventConsumer>();
                x.AddConsumer<GetEventConsumer>();
                x.AddConsumer<MarkEventConsumer>();
            });

            services.AddSingleton(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("localhost", "/", h => { });
                cfg.ReceiveEndpoint("event-service", e =>
                {
                    e.PrefetchCount = 16;
                    e.UseMessageRetry(x => x.Interval(2, 100));

                    e.Consumer<AddEventConsumer>(provider);
                    e.Consumer<EditEventConsumer>(provider);
                    EndpointConvention.Map<AddEvent>(e.InputAddress);

                    e.Consumer<GetEventConsumer>(provider);
                    EndpointConvention.Map<GetEvent>(e.InputAddress);
                    EndpointConvention.Map<GetEventsByArtist>(e.InputAddress);
                    EndpointConvention.Map<EditEvent>(e.InputAddress);

                    e.Consumer<MarkEventConsumer>(provider);
                    EndpointConvention.Map<MarkEvent>(e.InputAddress);
                    EndpointConvention.Map<GetMarkedEvents>(e.InputAddress);
                });
            }));

            services.AddSingleton<IPublishEndpoint>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<ISendEndpointProvider>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<IBus>(provider => provider.GetRequiredService<IBusControl>());

            services.AddScoped(provider => provider.GetRequiredService<IBus>().CreateRequestClient<AddEvent>());
            services.AddScoped(provider => provider.GetRequiredService<IBus>().CreateRequestClient<EditEvent>());
            services.AddScoped(provider => provider.GetRequiredService<IBus>().CreateRequestClient<GetEvent>());
            services.AddScoped(provider => provider.GetRequiredService<IBus>().CreateRequestClient<GetEventsByArtist>());
            services.AddScoped(provider => provider.GetRequiredService<IBus>().CreateRequestClient<MarkEvent>());
            services.AddScoped(provider => provider.GetRequiredService<IBus>().CreateRequestClient<GetMarkedEvents>());
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
