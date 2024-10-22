﻿using AutoMapper;
using Common.MessageContracts.User.Events;
using UserService.Models;

namespace UserService.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserModel>();
            CreateMap<RegisterModel, User>();
            CreateMap<UpdateModel, User>();
            CreateMap<AuthenticateModel, User>();
            CreateMap<User, UserCreated>();
            CreateMap<UserCreated, User>();

            CreateMap<Comment, CommentEvent>();
            CreateMap<CommentEvent, Comment>()
                .ForMember(x => x.Author, o => o.Ignore())
                .ForMember(x => x.AuthorName, o => o.MapFrom(s => s.Author));
        }
    }
}
