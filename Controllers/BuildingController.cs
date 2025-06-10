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
    public async Task<ActionResult<IEnumerable<BuildingResponseDTO>>> GetBuildings()
    {
        var buildings = await _buildingRepository.GetAllBuilding();
        return Ok(_mapper.Map<IEnumerable<BuildingResponseDTO>>(buildings));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BuildingResponseDTO>> GetBuildingById(int id)
    {
        var building = await _buildingRepository.GetBuildingById(id) ?? throw new MiddlewareException(System.Net.HttpStatusCode.NotFound, new { message = $"Building not found {id}" });
        return Ok(_mapper.Map<BuildingResponseDTO>(building));
    }

    [HttpPost()]
    public async Task<ActionResult<BuildingResponseDTO>> RegisterBuilding([FromBody] BuildingRequestDTO request)
    {
        var building = _mapper.Map<models.Building>(request);
        await _buildingRepository.AddBuilding(building);
        await _buildingRepository.SaveChanges();
       
        return CreatedAtRoute(
            nameof(RegisterBuilding),
            new { building.Id },
            _mapper.Map<BuildingResponseDTO>(building)
        );
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DelBuilding(int id)
    {  
        await _buildingRepository.DelBuilding(id);
        await _buildingRepository.SaveChanges();
        return Ok();
    }



}