using System.ComponentModel.DataAnnotations;
using alura_webapi.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace alura_webapi.Models.DTO
{
  public class VideoDTO
  {
    [Required]
    [StringLength(70)]
    [MinLength(1)]
    public string Titulo { get; set; }

    [StringLength(100)]
    public string Descricao { get; set; }

    [Required]
    [Url]
    [MinLength(1)]
    public string URL { get; set; }

    public int? CategoriaId { get; set; }

    [Required]
    public bool IsFree { get; set; }
  }
}