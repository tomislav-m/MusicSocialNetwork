﻿using AutoMapper;
using Common.MessageContracts.Music.Commands;
using Common.MessageContracts.Music.Events;

namespace MusicService.Service.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<DomainModel.Artist, ArtistFound>();

            CreateMap<CreateArtist, DomainModel.Artist>();
            CreateMap<DomainModel.Artist, CreateArtist>();

            CreateMap<DomainModel.Artist, ArtistCreated>();

            CreateMap<DomainModel.Artist, ArtistEdited>();
            CreateMap<EditArtist, DomainModel.Artist>();

            CreateMap<DomainModel.Artist, Artist>();

            CreateMap<DomainModel.Album, AlbumFound>();
            CreateMap<DomainModel.Album, Album>()
                .ForMember(d => d.Format, s => s.MapFrom(x => x.Format.Title))
                .ForMember(d => d.Style, s => s.MapFrom(x => x.Style.Title))
                .ForMember(d => d.Genre, s => s.MapFrom(x => x.Genre.Title))
                .ForMember(d => d.Tracks, s => s.MapFrom(x => x.Tracks));

            CreateMap<DomainModel.Track, Track>();
        }
    }
}
