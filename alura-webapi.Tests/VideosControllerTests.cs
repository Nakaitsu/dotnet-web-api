using alura_webapi.Controllers;
using alura_webapi.Models;
using alura_webapi.Models.DTO;
using alura_webapi.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace alura_webapi.Tests
{
  public class VideosControllerTests
  {
    [Fact]
    public async Task GET_ReturnsOkResult() // VOU REMOVER
    {
      // Given
      Mock<IRepository<Video>> mock = new Mock<IRepository<Video>>();
      mock.Setup(m => m.Items).Returns((new Video[] {
        new Video { Titulo = "video sample 1", URL = "http://test.url", Descricao = "Testing", CategoriaId = 1}
      }).AsQueryable<Video>());

      // When
      var targetController = new VideosController(mock.Object);
      var result = targetController.Index(null);

      // Then
      Assert.IsType<OkObjectResult>(result as OkObjectResult);
    }

    [Fact]
    public void Can_Get_All_Videos()
    {
      // Given
      Mock<IRepository<Video>> mock = new Mock<IRepository<Video>>();
      mock.Setup(m => m.Items).Returns((new Video[] {
        new Video { Titulo = "video sample 1", URL = "http://test.url", Descricao = "Testing", CategoriaId = 1},
        new Video { Titulo = "video sample 2", URL = "http://test.url", Descricao = "Testing", CategoriaId = 1},
        new Video { Titulo = "video sample 3", URL = "http://test.url", Descricao = "Testing", CategoriaId = 1},
        new Video { Titulo = "video sample 4", URL = "http://test.url", Descricao = "Testing", CategoriaId = 1}
      }).AsQueryable<Video>());

      // When
      var targetController = new VideosController(mock.Object);
      var result = targetController.Index(null) as OkObjectResult;

      // Then
      var items = Assert.IsType<List<Video>>(result.Value);
      Assert.True(items.Count == 4);
    }

    [Fact]
    public void Can_Search_Videos()
    {
      // Given
      Mock<IRepository<Video>> mock = new Mock<IRepository<Video>>();
      mock.Setup(m => m.Items).Returns((new Video[] {
        new Video { Titulo = "Uma Noite no Cinema", URL = "http://test.url", Descricao = "Testing", CategoriaId = 1},
        new Video { Titulo = "A Aventura de Jim", URL = "http://test.url", Descricao = "Testing", CategoriaId = 1},
        new Video { Titulo = "O Cavalo de Troia", URL = "http://test.url", Descricao = "Testing", CategoriaId = 1},
        new Video { Titulo = "A Capela", URL = "http://test.url", Descricao = "Testing", CategoriaId = 1},
        new Video { Titulo = "O Cisne", URL = "http://test.url", Descricao = "Testing", CategoriaId = 1}
      }).AsQueryable<Video>());

      // When
      string searchParam = "Aventura";
      var targetController = new VideosController(mock.Object);
      var result = targetController.Index(searchParam) as OkObjectResult;

      // Then
      var items = Assert.IsType<List<Video>>(result.Value);
      Assert.True(items.Count == 1);
      Assert.Contains(searchParam, items.First().Titulo);
    }

    [Fact]
    public void Can_Post_Video()
    {
      // Given
      Mock<IRepository<Video>> mock = new Mock<IRepository<Video>>();
      mock.Setup(m => m.Items).Returns((new Video[] {
        new Video { Titulo = "Meu Primerio Video", URL = "http://test.url", Descricao = "Testing", CategoriaId = 1}
      }).AsQueryable<Video>());

      IQueryable<Categoria> categorias = (new Categoria[] {
        new Categoria { Id = 1, Titulo = "Terror" },
        new Categoria { Id = 2, Titulo = "Ação" },
        new Categoria { Id = 3, Titulo = "Drama" },
      }).AsQueryable<Categoria>();

      mock.Setup(m => m.ValidateEntity(It.IsAny<Video>()))
        .Returns<Video>(video => categorias.Any(categoria => categoria.Id == video.CategoriaId));

      List<Video> lista = new List<Video>()
      {
        new Video { Titulo = "Uma Noite no Museu", URL="http://test.url", Descricao = "Testing", CategoriaId = 3}
      };

      mock.Setup(m => m.Create(It.IsAny<Video>())).Callback<Video>(
        video => lista.Add(video)); 

      VideoDTO targetVideo = new VideoDTO {
        Titulo = "Em Alto Mar",
        Descricao = "Testing",
        URL = "http://test.url",
        CategoriaId = 1
      };

      // When
      var targetController = new VideosController(mock.Object);
      var result = targetController.PostVideo(targetVideo) as OkObjectResult; 
    
      // Then
      Assert.True(lista.Count == 2); 
      Assert.Equal(lista.Last().Titulo, targetVideo.Titulo);
      Assert.Equal(lista.Last().Descricao, targetVideo.Descricao);
      Assert.Equal(lista.Last().URL, targetVideo.URL);
    }
  }
}