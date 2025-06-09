using AutoMapper;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using NetBuilding.Data;
using NetBuilding.Dtos;
using NetBuilding.Middleware;

namespace NetBuilding.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BuildingController : ControllerBase
{
    private readonly IBuildingRepository _buildingRepository;
    private IMapper _mapper;

    public BuildingController(Data.IBuildingRepository buildingRepository, IMapper mapper)
    {
        _buildingRepository = buildingRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<BuildingResponseDTO>> GetBuildings()
    {
        var buildings = _buildingRepository.GetAllBuilding();
        return Ok(_mapper.Map<IEnumerable<BuildingResponseDTO>>(buildings));
    }

    [HttpGet("{id}")]
    public ActionResult<BuildingResponseDTO> GetBuilding(int id)
    {
        var building = _buildingRepository.GetBuildingById(id) ?? throw new MiddlewareException(System.Net.HttpStatusCode.NotFound, new { message = $"Building not found {id}" });
        return Ok(_mapper.Map<BuildingResponseDTO>(building));
    }

    [HttpPost()]
    public ActionResult<BuildingResponseDTO> RegisterBuilding([FromBody] BuildingRequestDTO request)
    {
        var building = _mapper.Map<models.Building>(request);
        _buildingRepository.AddBuilding(building);
        if (!_buildingRepository.SaveChanges())
        {
            throw new MiddlewareException(HttpStatusCode.InternalServerError, new { message = "Error saving changes" });
        }


        return CreatedAtRoute(
            nameof(GetBuilding),
            new { building.Id },
            _mapper.Map<BuildingResponseDTO>(building)
        );
    }

    [HttpDelete("{id}")]
    public ActionResult DelBuilding(int id)
    {  
        _buildingRepository.DelBuilding(id);
        if (!_buildingRepository.SaveChanges())
        {
            throw new MiddlewareException(HttpStatusCode.InternalServerError, new { message = "Error deleting building" });
        }
        return Ok();
    }



}