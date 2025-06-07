using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace NetBuilding.models;

public class Building
{

    [Key]
    [Required]
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,4)")]
    public Decimal Price { get; set; }    
    public string? Picture { get; set; }
    public DateTime? DateCreation { get; set; }



}