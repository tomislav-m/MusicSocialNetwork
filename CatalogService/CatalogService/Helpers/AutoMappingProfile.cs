using AutoMapper;
using CatalogService.MessageContracts;
using CatalogService.Models;

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

            CreateMap<Tag, CustomTagAdded>();
            CreateMap<CustomTagAdded, Tag>();

            CreateMap<AddToTag, AlbumAddedToTag>();
        }
    }
}
