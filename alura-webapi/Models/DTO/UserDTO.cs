using System.ComponentModel.DataAnnotations;

namespace alura_webapi.Models.DTO
{
  public class UserDTO
  {
    [Required]
    [MinLength(1)]
    [MaxLength(100)]
    public string Username { get; set; }

    // [Required]
    [MinLength(1)]
    [EmailAddress]
    [MaxLength(100)]
    public string? Email { get; set; }

    [Required]
    [MinLength(8)]
    public string Password { get; set; }
  }
}