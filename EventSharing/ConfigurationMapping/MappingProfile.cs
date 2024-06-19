using AutoMapper;
using EventSharing.Models;
using EventSharing.ViewModels;

namespace EventSharing.ConfigurationMapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Event, EventViewModel>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src =>
                src.Category.Name))
                .ForMember(dest => dest.IdCategory, opt => opt.MapFrom(src =>
                src.Category.Id))
                .ReverseMap();
            CreateMap<Category, CategoryViewModel>()
                .ReverseMap();
            CreateMap<User, UserViewModel>()
                .ReverseMap();
        }
    }
}
