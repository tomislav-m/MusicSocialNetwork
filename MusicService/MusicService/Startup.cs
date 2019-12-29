using AutoMapper;
using Common.MessageContracts.Music.Commands;
using Common.Services;
using GreenPipes;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MusicService.DomainModel;
using MusicService.Service.Consumers;
using MusicService.Service.Services;
using System;

namespace MusicService.Service
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
            services.AddControllers();
            services.AddEntityFrameworkNpgsql().AddDbContext<MusicDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("MusicDbContext")));
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IArtistService, ArtistService>();
            services.AddScoped<IAlbumService, AlbumService>();

            services.AddScoped<SearchArtistConsumer>();
            services.AddScoped<CreateArtistConsumer>();
            services.AddScoped<GetArtistConsumer>();
            services.AddScoped<SearchAlbumConsumer>();
            services.AddScoped<GetAlbumConsumer>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<SearchArtistConsumer>();
                x.AddConsumer<CreateArtistConsumer>();
                x.AddConsumer<GetArtistConsumer>();
                x.AddConsumer<SearchAlbumConsumer>();
                x.AddConsumer<GetAlbumConsumer>();
            });

            services.AddSingleton(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("localhost", "/", h => { });
                cfg.ReceiveEndpoint("music-service", e =>
                {
                    e.PrefetchCount = 16;
                    e.UseMessageRetry(x => x.Interval(2, 100));

                    e.Consumer<SearchArtistConsumer>(provider);
                    EndpointConvention.Map<SearchArtist>(e.InputAddress);

                    e.Consumer<CreateArtistConsumer>(provider);
                    EndpointConvention.Map<CreateArtist>(e.InputAddress);

                    e.Consumer<GetArtistConsumer>(provider);
                    EndpointConvention.Map<GetArtist>(e.InputAddress);

                    e.Consumer<SearchAlbumConsumer>(provider);
                    EndpointConvention.Map<SearchAlbum>(e.InputAddress);

                    e.Consumer<GetAlbumConsumer>(provider);
                    EndpointConvention.Map<GetAlbum>(e.InputAddress);
                });
            }));

            services.AddSingleton<IPublishEndpoint>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<ISendEndpointProvider>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<IBus>(provider => provider.GetRequiredService<IBusControl>());

            services.AddScoped(provider => provider.GetRequiredService<IBus>().CreateRequestClient<Search>());
            services.AddScoped(provider => provider.GetRequiredService<IBus>().CreateRequestClient<CreateArtist>());
            services.AddScoped(provider => provider.GetRequiredService<IBus>().CreateRequestClient<GetAlbum>());
            services.AddScoped(provider => provider.GetRequiredService<IBus>().CreateRequestClient<GetArtist>());
            services.AddSingleton<IHostedService, BusService>();
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
