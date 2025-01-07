using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AcceralytDevTest.models;

public class User
{
    [Required]
    [Key]
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Name is required")]
    [StringLength(1000,ErrorMessage = "Name cannot be longer than 1000 characters")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Email is required")]
    public string UserName { get; set; }
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
    [Required(ErrorMessage = "Birthday is required")]
    public string Birthday { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    [Required(ErrorMessage = "PhoneNumber is required")]
    public string PhoneNumber { get; set; }
    [Required(ErrorMessage = "Role is required.")]
    [StringLength(50, ErrorMessage = "Role cannot exceed 50 characters.")]
    public string Role { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}