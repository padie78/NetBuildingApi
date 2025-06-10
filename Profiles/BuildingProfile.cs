using AutoMapper;
using NetBuilding.Dtos;
using NetBuilding.models;
namespace NetBuilding.Profiles;

public class BuildingProfile : Profile
{
    public BuildingProfile()
    {
        CreateMap<Building, BuildingResponseDTO>();
        CreateMap<BuildingRequestDTO, Building>();
}
}
