using System.ComponentModel.DataAnnotations;

namespace alura_webapi.Models
{
  public class Video
  {
    [Key]
    public int Id { get; set; }

    [StringLength(70)]
    public string Titulo { get; set; }

    [StringLength(100)]
    public string Descricao { get; set; }

    public string URL { get; set; }

    public int CategoriaId { get; set; }

    public bool IsFree { get; set; }
  }
}