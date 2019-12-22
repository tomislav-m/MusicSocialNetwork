using CatalogService.MessageContract;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using MusicService.MessageContracts;
using System;
using UserService.MessageContracts;

namespace Producer
{
    public static class BusConfiguration
    {
        public static void ConfigureBus(this IServiceCollection services)
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri("rabbitmq://localhost/"), h => { });
            });

            services.AddSingleton<IPublishEndpoint>(bus);
            services.AddSingleton<ISendEndpointProvider>(bus);
            services.AddSingleton<IBus>(bus);

            var timeout = TimeSpan.FromSeconds(120);
            var userServiceAddress = new Uri("rabbitmq://localhost/user-service");

            services.AddScoped<IRequestClient<SignInUser, UserSignedIn>>(x =>
                new MessageRequestClient<SignInUser, UserSignedIn>(x.GetRequiredService<IBus>(), userServiceAddress, timeout, timeout));


            var musicServiceAddress = new Uri("rabbitmq://localhost/music-service");

            services.AddScoped<IRequestClient<CreateArtist, ArtistCreated>>(x =>
                new MessageRequestClient<CreateArtist, ArtistCreated>(x.GetRequiredService<IBus>(), musicServiceAddress, timeout, timeout));
            services.AddScoped<IRequestClient<SearchArtist, ArtistFound[]>>(x =>
                new MessageRequestClient<SearchArtist, ArtistFound[]>(x.GetRequiredService<IBus>(), musicServiceAddress, timeout, timeout));
            services.AddScoped<IRequestClient<GetArtist, Artist>>(x =>
                new MessageRequestClient<GetArtist, Artist>(x.GetRequiredService<IBus>(), musicServiceAddress, timeout, timeout));
            services.AddScoped<IRequestClient<GetAlbum, Album>>(x =>
                new MessageRequestClient<GetAlbum, Album>(x.GetRequiredService<IBus>(), musicServiceAddress, timeout, timeout));
            services.AddScoped<IRequestClient<SearchAlbum, AlbumFound[]>>(x =>
                new MessageRequestClient<SearchAlbum, AlbumFound[]>(x.GetRequiredService<IBus>(), musicServiceAddress, timeout, timeout));


            var catalogServiceAddress = new Uri("rabbitmq://localhost/catalog-service");

            services.AddScoped<IRequestClient<AddCustomCollection, CustomCollectionAdded>>(x =>
                new MessageRequestClient<AddCustomCollection, CustomCollectionAdded>(x.GetRequiredService<IBus>(), catalogServiceAddress, timeout, timeout));
            services.AddScoped<IRequestClient<AddToCollection, AlbumAddedToCollection>>(x =>
                new MessageRequestClient<AddToCollection, AlbumAddedToCollection>(x.GetRequiredService<IBus>(), catalogServiceAddress, timeout, timeout));
            services.AddScoped<IRequestClient<RateAlbum, AlbumRated>>(x =>
                new MessageRequestClient<RateAlbum, AlbumRated>(x.GetRequiredService<IBus>(), catalogServiceAddress, timeout, timeout));


            var eventServiceAddress = new Uri("rabbitmq://localhost/event-service");

            bus.Start();
        }
    }
}
