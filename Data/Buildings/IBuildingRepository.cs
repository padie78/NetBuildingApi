namespace NetBuilding.Data;

using NetBuilding.models;

public interface IBuildingRepository
{
    public bool SaveChanges();

    public IEnumerable<Building> GetAllBuilding();

    public Building GetBuildingById(int id);

    public Task AddBuilding(Building building);

    public void DelBuilding(int id);
}