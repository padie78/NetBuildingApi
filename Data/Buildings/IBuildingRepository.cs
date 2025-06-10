namespace NetBuilding.Data;

using NetBuilding.models;

public interface IBuildingRepository
{
    Task<bool> SaveChanges();

    Task<IEnumerable<Building>> GetAllBuilding();

    Task<Building> GetBuildingById(int id);

    Task AddBuilding(Building building);

    Task DelBuilding(int id);
}