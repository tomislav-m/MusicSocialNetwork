using Common.MessageContracts.Catalog.Commands;
using Common.MessageContracts.Catalog.Events;
using Common.MessageContracts.Event.Commands;
using Common.MessageContracts.Event.Event;
using Common.MessageContracts.Event.Events;
using Common.MessageContracts.Music.Commands;
using Common.MessageContracts.Music.Events;
using Common.MessageContracts.Recommender.Commands;
using Common.MessageContracts.Recommender.Events;
using Common.MessageContracts.Ticketing.Commands;
using Common.MessageContracts.Ticketing.Events;
using Common.MessageContracts.User.Commands;
using Common.MessageContracts.User.Events;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace WebApi
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

            var timeout = TimeSpan.FromSeconds(60);
            var userServiceAddress = new Uri("rabbitmq://localhost/user-service");

            services.AddScoped<IRequestClient<SignInUser, UserSignedIn>>(x =>
                new MessageRequestClient<SignInUser, UserSignedIn>(x.GetRequiredService<IBus>(), userServiceAddress, timeout, timeout));
            services.AddScoped<IRequestClient<CreateUser, UserCreated>>(x =>
                new MessageRequestClient<CreateUser, UserCreated>(x.GetRequiredService<IBus>(), userServiceAddress, timeout, timeout));
            services.AddScoped<IRequestClient<AddComment, CommentEvent>>(x =>
                new MessageRequestClient<AddComment, CommentEvent>(x.GetRequiredService<IBus>(), userServiceAddress, timeout, timeout));
            services.AddScoped<IRequestClient<GetComments, CommentEvent[]>>(x =>
                new MessageRequestClient<GetComments, CommentEvent[]>(x.GetRequiredService<IBus>(), userServiceAddress, timeout, timeout));


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
            services.AddScoped<IRequestClient<GetArtistNamesByIds, ArtistSimple[]>>(x =>
                new MessageRequestClient<GetArtistNamesByIds, ArtistSimple[]>(x.GetRequiredService<IBus>(), musicServiceAddress, timeout, timeout));


            var catalogServiceAddress = new Uri("rabbitmq://localhost/catalog-service");

            services.AddScoped<IRequestClient<AddToCollection, AlbumAddedToCollection>>(x =>
                new MessageRequestClient<AddToCollection, AlbumAddedToCollection>(x.GetRequiredService<IBus>(), catalogServiceAddress, timeout, timeout));
            services.AddScoped<IRequestClient<RateAlbum, AlbumRated>>(x =>
                new MessageRequestClient<RateAlbum, AlbumRated>(x.GetRequiredService<IBus>(), catalogServiceAddress, timeout, timeout));
            services.AddScoped<IRequestClient<GetRatedAlbums, AlbumRated[]>>(x =>
                new MessageRequestClient<GetRatedAlbums, AlbumRated[]>(x.GetRequiredService<IBus>(), catalogServiceAddress, timeout, timeout));
            services.AddScoped<IRequestClient<GetAverageRating, AlbumAverageRating>>(x =>
                new MessageRequestClient<GetAverageRating, AlbumAverageRating>(x.GetRequiredService<IBus>(), catalogServiceAddress, timeout, timeout));
            services.AddScoped<IRequestClient<GetPopularAlbums, PopularAlbums>>(x =>
                new MessageRequestClient<GetPopularAlbums, PopularAlbums>(x.GetRequiredService<IBus>(), catalogServiceAddress, timeout, timeout));


            var eventServiceAddress = new Uri("rabbitmq://localhost/event-service");

            services.AddScoped<IRequestClient<AddEvent, EventAdded>>(x =>
                new MessageRequestClient<AddEvent, EventAdded>(x.GetRequiredService<IBus>(), eventServiceAddress, timeout, timeout));
            services.AddScoped<IRequestClient<EditEvent, EventEdited>>(x =>
                new MessageRequestClient<EditEvent, EventEdited>(x.GetRequiredService<IBus>(), eventServiceAddress, timeout, timeout));
            services.AddScoped<IRequestClient<GetEvent, EventEvent>>(x =>
                new MessageRequestClient<GetEvent, EventEvent>(x.GetRequiredService<IBus>(), eventServiceAddress, timeout, timeout));
            services.AddScoped<IRequestClient<GetEventsByArtist, EventEvent[]>>(x =>
                new MessageRequestClient<GetEventsByArtist, EventEvent[]>(x.GetRequiredService<IBus>(), eventServiceAddress, timeout, timeout));
            services.AddScoped<IRequestClient<MarkEvent, EventMarked>>(x =>
                new MessageRequestClient<MarkEvent, EventMarked>(x.GetRequiredService<IBus>(), eventServiceAddress, timeout, timeout));
            services.AddScoped<IRequestClient<GetMarkedEvents, MarkedEvent[]>>(x =>
                new MessageRequestClient<GetMarkedEvents, MarkedEvent[]>(x.GetRequiredService<IBus>(), eventServiceAddress, timeout, timeout));


            var ticketingServiceAddress = new Uri("rabbitmq://localhost/ticketing-service");

            services.AddScoped<IRequestClient<BuyTickets, TicketBought>>(x =>
                new MessageRequestClient<BuyTickets, TicketBought>(x.GetRequiredService<IBus>(), ticketingServiceAddress, timeout, timeout));
            services.AddScoped<IRequestClient<GetEventTickets, EventTickets>>(x =>
                new MessageRequestClient<GetEventTickets, EventTickets>(x.GetRequiredService<IBus>(), ticketingServiceAddress, timeout, timeout));
            services.AddScoped<IRequestClient<AddEditEventTickets, EventTicketAdded>>(x =>
                new MessageRequestClient<AddEditEventTickets, EventTicketAdded>(x.GetRequiredService<IBus>(), ticketingServiceAddress, timeout, timeout));

            var recommenderServiceAddress = new Uri("rabbitmq://localhost/recommender-service");
            services.AddScoped<IRequestClient<GetRecommendations, Recommendations>>(x =>
                new MessageRequestClient<GetRecommendations, Recommendations>(x.GetRequiredService<IBus>(), recommenderServiceAddress, timeout, timeout));

            bus.Start();
        }
    }
}
