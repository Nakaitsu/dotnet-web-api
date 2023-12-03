using alura_webapi.Data;
using alura_webapi.Models;
using alura_webapi.Models.DTO;
using alura_webapi.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace alura_webapi.Controllers
{
  [ApiController]
  [Route("/api/[controller]")]
  public class CategoriasController : ControllerBase
  {
    private readonly IRepository<Categoria> _CategoriaRepository;
    private readonly IRepository<Video> _VideoRepository;

    public CategoriasController(IRepository<Categoria> categoriaRepository, IRepository<Video> videoRepository)
    {
      _CategoriaRepository = categoriaRepository;
      _VideoRepository = videoRepository;
    }

    [HttpGet]
    public IActionResult Index()
    {
      if (_CategoriaRepository.Items.Count() > 0)
      {
        List<Categoria> result = _CategoriaRepository.Items.AsNoTracking().ToList();
        return Ok(result);
      }

      return NotFound(new { Errors = "Nenhuma categoria foi encontrada!" });
    }

    [HttpGet]
    [Route("{id:int}")]
    public IActionResult GetCategoria([FromRoute] int id)
    {
      Categoria? result = _CategoriaRepository.Items.AsNoTracking().SingleOrDefault(categoria => categoria.Id == id);

      if(result != null)
        return Ok(result);

      return NotFound(new { Errors = $"A categoria de id: {id} não foi encontrada" });
    }

    [HttpPost]
    public IActionResult PostCategoria([FromBody] CategoriaDTO data)
    {
      Categoria categoria = new Categoria
      {
        Titulo = data.Titulo,
        Cor = data.Cor
      };

      _CategoriaRepository.Create(categoria);

      return Ok(new
      {
        Message = "A categoria foi salva com sucesso",
        Categoria = categoria
      });
    }

    [HttpPut, HttpPatch]
    [Route("{id:int}")]
    public IActionResult UpdateCategoria([FromRoute] int id, [FromBody] CategoriaDTO data)
    {
      Categoria categoria = _CategoriaRepository.Items.AsNoTracking().SingleOrDefault(categoria => categoria.Id == id);

      if(categoria != null)
      {
        categoria.Cor = data.Cor;
        categoria.Titulo = data.Titulo;

        _CategoriaRepository.Update(categoria);

        return Ok(new
        {
          Message = "A categoria foi atualizada com sucesso",
          Categoria = categoria
        });
      }
     else
      return BadRequest(new { Errors = $"Nenhum registro foi encontrado para o id: {id}"});
    }

    [HttpDelete]
    [Route("{id:int}")]
    public IActionResult DeleteVideo([FromRoute] int id)
    {
      Categoria categoria = _CategoriaRepository.Items.AsNoTracking().SingleOrDefault(categoria => categoria.Id == id);

      if (categoria != null)
      {
        _CategoriaRepository.Delete(categoria);

        return Ok(new { Message = $"A categoria de id: {id} foi deletada com sucesso"});
      }
      else
        return BadRequest(new { Errors = $"A categoria de id: {id} não foi encontrada"});
    }

    [HttpGet]
    [Route("{id:int}/videos")]
    public IActionResult GetVideos([FromRoute] int id)
    {
      Categoria? categoria = _CategoriaRepository.Items.SingleOrDefault(categoria => categoria.Id == id);

      if(categoria != null)
      {
        List<Video> result = _VideoRepository.Items.Where(video => video.CategoriaId == id).ToList();

        if(result.Count > 0)
          return Ok(result);

        return NotFound(new { Message = $"Não foram encontrados videos para a categoria: {categoria.Titulo}"});
      }

      return BadRequest(new { Errors = $"A categoria de id: {id} não existe! "});
    }

  }
}