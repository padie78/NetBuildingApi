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
       return _context.SaveChanges() >= 0;
    }

    public IEnumerable<Building> GetAllBuilding()
    {
        return _context.Buildings!.ToList();
    }

    public Building GetBuildingById(int id)
    {
        return _context.Buildings!.FirstOrDefault(b => b.Id == id)!;   
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
        var building = _context.Buildings!.FirstOrDefault(b => b.Id == id);
        _context.Buildings!.Remove(building!);  
        _context.SaveChanges();            
    }
}