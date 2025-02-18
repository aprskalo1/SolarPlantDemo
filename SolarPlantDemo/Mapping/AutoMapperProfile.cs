using AutoMapper;
using SolarPlantDemo.Models.DTOs.Request;
using SolarPlantDemo.Models.DTOs.Response;
using SolarPlantDemo.Models.Entities;

namespace SolarPlantDemo.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<User, UserRegisterRequestDto>().ReverseMap();
        CreateMap<User, UserResponseDto>().ReverseMap();

        CreateMap<PowerPlant, PowerPlantRequestDto>().ReverseMap();
        CreateMap<PowerPlant, PowerPlantResponseDto>().ReverseMap();

        CreateMap<PlantRecord, RecordResponseDto>().ReverseMap();
    }
}