using AutoMapper;
using UserManagementAPI.DTOs;
using UserManagementAPI.Models;

namespace UserManagementAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, UserDto>();
            CreateMap<RegisterDto, ApplicationUser>();
            CreateMap<UpdateUserDto, ApplicationUser>();

            CreateMap<ProductModel, ProductResponseDTO>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.GetType().Name))
                .Include<Television, ProductResponseDTO>()
                .Include<WashingMachine, ProductResponseDTO>()
                .Include<AirConditioner, ProductResponseDTO>();
                CreateMap<Television, ProductResponseDTO>();
                CreateMap<WashingMachine, ProductResponseDTO>();
                CreateMap<AirConditioner, ProductResponseDTO>();
            CreateMap<ProductCreateDTO, ProductModel>()
                .Include<ProductCreateDTO, AirConditioner>()
                .Include<ProductCreateDTO, WashingMachine>()
                .Include<ProductCreateDTO, Television>();
            CreateMap<ProductCreateDTO, AirConditioner>();
            CreateMap<ProductCreateDTO, WashingMachine>();
            CreateMap<ProductCreateDTO, Television>();
            CreateMap<ProductUpdateDTO, ProductModel>()
                .Include<ProductUpdateDTO, AirConditioner>()
                .Include<ProductUpdateDTO, WashingMachine>()
                .Include<ProductUpdateDTO, Television>();
            CreateMap<ProductUpdateDTO, AirConditioner>();
            CreateMap<ProductUpdateDTO, WashingMachine>();
            CreateMap<ProductUpdateDTO, Television>();
        }
    }
} 