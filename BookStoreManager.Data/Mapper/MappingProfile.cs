using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookStoreManager.Domain.DTOs;
using BookStoreManager.Domain.Entities;

namespace BookStoreManager.Data.Mapper
{
    public class MappingProfile : Profile
    {
    public MappingProfile()
    {
        CreateMap<Book, BookDTO>()
        .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
        .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
        .ForMember(dest => dest.Quantities, opt => opt.MapFrom(src => src.Quantities))
        .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId))
        .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Year))
        .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

         CreateMap<BookDTO, Book>()
        .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
        .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
        .ForMember(dest => dest.Quantities, opt => opt.MapFrom(src => src.Quantities))
        .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId))
        .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Year))
        .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));


         CreateMap<Author, AuthorDTO>()
        .ForMember(dest => dest.Firstname, opt => opt.MapFrom(src => src.Firstname))
        .ForMember(dest => dest.Lastname, opt => opt.MapFrom(src => src.Lastname))
        .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
        .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));

         CreateMap<AuthorDTO, Author>()
        .ForMember(dest => dest.Firstname, opt => opt.MapFrom(src => src.Firstname))
        .ForMember(dest => dest.Lastname, opt => opt.MapFrom(src => src.Lastname))
        .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
        .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));
    }

    }
}