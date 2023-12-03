using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace alura_webapi.Models
{
  public class Categoria
  {
    [Key]
    public int Id { get; set; }

    [StringLength(70)]
    public string Titulo { get; set; }
 
    [StringLength(100)]
    public string Cor { get; set; } // Precisa ser acessível no CSS
  }
}