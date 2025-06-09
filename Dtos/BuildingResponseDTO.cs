namespace NetBuilding.Dtos;

public class BuildingResponseDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
    public decimal Price { get; set; }
    public string? Picture { get; set; }
    public DateTime? DateCreation { get; set; }
}