using System.ComponentModel.DataAnnotations;

namespace alura_webapi.Models.DTO
{
  public class CategoriaDTO
  {
    [Required]
    [MinLength(1)]
    [StringLength(70)]
    public string Titulo { get; set; }
 
    [Required]
    [MinLength(1)]
    [StringLength(100)]
    public string Cor { get; set; } // Precisa ser acessível no CSS
  }
}