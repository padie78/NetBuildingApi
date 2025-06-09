using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
namespace NetBuilding.models;

public class User : IdentityUser
{
    public string? Name { get; set; }
    public string? LastName { get; set; }
}