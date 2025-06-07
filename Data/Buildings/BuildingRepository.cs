using Microsoft.AspNetCore.Identity;
using NetBuilding.models;
using NetBuilding.Token;

namespace NetBuilding.Data;

public class BuildingRepository : IBuildingRepository
{

    private readonly AppDbContext _context;
    private readonly IUserSession _userSession;

    private readonly UserManager<User> _userManager;


    public BuildingRepository(AppDbContext context, IUserSession session, UserManager<User> userManager)
    {
        _context = context;
        _userSession = session;
        _userManager = userManager;
    }
    public bool SaveChanges()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Building> GetAllBuilding()
    {
        throw new NotImplementedException();
    }

    public Building GetBuildingById(int id)
    {
        throw new NotImplementedException();
    }

    public async Task AddBuilding(Building building)
    {
        var user = await _userManager.FindByNameAsync(_userSession.getUserSession());
        building.DateCreation = DateTime.Now;
        building.User = Guid.Parse(user!.Id);
        _context.Buildings!.Add(building);
    }

    public void DelBuilding(int id)
    {
        throw new NotImplementedException();
    }
}