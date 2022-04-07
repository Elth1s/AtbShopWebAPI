﻿using AtbShop.Models;
using AutoMapper;
using DAL.Entities.Identity;

namespace AtbShop.Mapper
{
    public class AppMapProfile : Profile
    {
        public AppMapProfile()
        {
            CreateMap<RegisterViewModel, AppUser>()
                .ForMember(x => x.Photo, opt => opt.Ignore())
                .ForMember(x => x.PhoneNumber, opt => opt.MapFrom(x=>x.Phone))
                .ForMember(x => x.UserName, opt => opt.MapFrom(x => x.Email));

            CreateMap<AppUser, UserItemViewModel>()
                .ForMember(x => x.Photo, opt => opt.MapFrom(x=> $"/images/{x.Photo}"))
                .ForMember(x => x.Phone, opt => opt.MapFrom(x => x.PhoneNumber));
        }
    }
}
