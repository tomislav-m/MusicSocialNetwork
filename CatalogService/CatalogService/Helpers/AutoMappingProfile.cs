using AutoMapper;
using CatalogService.Models;
using Common.MessageContracts.Catalog.Commands;
using Common.MessageContracts.Catalog.Events;

namespace CatalogService.Helpers
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            CreateMap<RateAlbum, AlbumRating>();
            CreateMap<AlbumRating, RateAlbum>();
            CreateMap<AlbumRated, AlbumRating>();
            CreateMap<AlbumRating, AlbumRated>();

            CreateMap<AddToCollection, AlbumAddedToCollection>();
        }
    }
}
