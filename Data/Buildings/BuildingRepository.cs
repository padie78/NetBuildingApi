using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetBuilding.Middleware;
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
    public async Task<bool> SaveChanges()
    {
       return await _context.SaveChangesAsync() >= 0;
    }

    public async Task<IEnumerable<Building>> GetAllBuilding()
    {
        return await _context.Buildings!.ToListAsync();
    }

    public async Task<Building> GetBuildingById(int id)
    {
        return await _context.Buildings!.FirstOrDefaultAsync(b => b.Id == id) 
               ?? throw new MiddlewareException(System.Net.HttpStatusCode.NotFound,
                                          new { message = "Building not found" });   
    }

    public async Task AddBuilding(Building building)
    {
        var user = await _userManager.FindByNameAsync(_userSession.getUserSession()) ?? throw new MiddlewareException(System.Net.HttpStatusCode.Unauthorized,
                                          new { message = "User not autohrized" });

        if (building == null)
        {
            throw new MiddlewareException(System.Net.HttpStatusCode.BadRequest,
                                          new { message = "Building already exist in DB" });
        }

        building.DateCreation = DateTime.Now;
        building.User = Guid.Parse(user!.Id);
        _context.Buildings!.Add(building);
    }

    public async Task DelBuilding(int id)
    {
        var building = await _context.Buildings!.FirstOrDefaultAsync(b => b.Id == id);
        _context.Buildings!.Remove(building!);  
        _context.SaveChanges();            
    }
}