using alura_webapi.Data;
using alura_webapi.Models;
using alura_webapi.Models.DTO;
using alura_webapi.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace alura_webapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VideosController : ControllerBase
{
  private readonly IRepository<Video> _VideoRepository;
  private int pageSize = 5;

  public VideosController(IRepository<Video> repoService)
  {
    _VideoRepository = repoService;
  }

  [HttpGet]
  public IActionResult Index([FromQuery] string? search, [FromQuery] int page = 1)
  {
    if (_VideoRepository.Items.Count() > 0)
    {
      List<Video> result = _VideoRepository.Items.ToList();

      if (!string.IsNullOrEmpty(search))
      {
        result = result.Where(video => video.Titulo.ToLower().Contains(search.ToLower())).ToList();

        if(result.Count > 0)
          return Ok(result.Skip(pageSize * (page - 1)).Take(pageSize));
        else
          return NotFound( new { Errors = $"Nenhum video contém: '{search}' em seu titulo."});
      }

      return Ok(result.Skip(pageSize * (page - 1)).Take(pageSize));
    }

    return NotFound(new { Errors = "Nenhum video foi encontrado!" });
  }

  [HttpGet]
  [Route("{id:int}")]
  public IActionResult GetVideo([FromRoute] int id)
  {
    Video? result = _VideoRepository.Items.SingleOrDefault(video => video.Id == id);

    if (result != null)
      return Ok(result);

    return NotFound(new { Errors = $"O video de id: {id} não foi encontrado!" });
  }

  [HttpGet("free")]
  public IActionResult GetFreeVideos([FromQuery] string? search, [FromQuery] int page = 1)
  { 
    var freeVideosList = _VideoRepository.Items.Where(v => v.IsFree);
    
    if (freeVideosList.Count() > 0)
    {
      List<Video> result = freeVideosList.ToList();

      if (!string.IsNullOrEmpty(search))
      {
        result = result.Where(video => video.Titulo.ToLower().Contains(search.ToLower())).ToList();

        if(result.Count > 0)
          return Ok(result.Skip(pageSize * (page - 1)).Take(pageSize));
        else
          return NotFound( new { Errors = $"Nenhum video contém: '{search}' em seu titulo."});
      }

      return Ok(result.Skip(pageSize * (page - 1)).Take(pageSize));
    }

    return NotFound(new { Errors = "Não há videos gratuitos cadastrados!" });
  }

  [Authorize]
  [HttpPost]
  public IActionResult PostVideo([FromBody] VideoDTO data)
  {
    Video video = new Video
    {
      Titulo = data.Titulo,
      Descricao = data.Descricao,
      URL = data.URL,
      IsFree = data.IsFree,
      CategoriaId = data.CategoriaId.HasValue ? data.CategoriaId.Value : 1
    };

    if (video.CategoriaId > 1 && !_VideoRepository.ValidateEntity(video))
      return BadRequest(new { Errors = $"O valor da categoria: {data.CategoriaId} não existe!" });

    _VideoRepository.Create(video);

    return Ok(new
    {
      Message = "O video foi salvo com sucesso!",
      Video = video
    });
  }

  [HttpPatch, HttpPut]
  [Route("{id:int}")]
  public IActionResult UpdateVideo([FromRoute] int id, [FromBody] VideoDTO data)
  {
    Video? video = _VideoRepository.Items.SingleOrDefault(video => video.Id == id);

    if (video != null)
    {
      video.Titulo = data.Titulo;
      video.Descricao = data.Descricao;
      video.IsFree = data.IsFree;
      video.CategoriaId = data.CategoriaId.HasValue ? data.CategoriaId.Value : video.CategoriaId;
      video.URL = data.URL;

      if (data.CategoriaId > 1 && !_VideoRepository.ValidateEntity(video))
        return BadRequest(new { Errors = $"O valor da categoria: {data.CategoriaId} não existe!" });

      _VideoRepository.Update(video);

      return Ok(new
      {
        Message = "O video foi atualizado com sucesso",
        Video = video
      });
    }
    else
      return BadRequest(new { Errors = $"Nenhum registro foi encontrado para o id: {id}" });
  }

  [HttpDelete]
  [Route("{id:int}")]
  public IActionResult DeleteVideo([FromRoute] int id)
  {
    Video? video = _VideoRepository.Items.FirstOrDefault(video => video.Id == id);

    if (video != null)
    {
      _VideoRepository.Delete(video);
      return Ok(new { Message = $"O video de id: {id} foi deletado com sucesso" });
    }
    else
      return BadRequest(new { Errors = $"O video de id: {id} não foi encontrado" });
  }
}
