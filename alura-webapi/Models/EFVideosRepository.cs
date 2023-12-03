using alura_webapi.Data;
using alura_webapi.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace alura_webapi.Models
{
  public class EFVideosRepository : IRepository<Video>
  {
    private readonly EFContext _Context;

    public EFVideosRepository(EFContext context)
    {
      _Context = context;
    }

    public IQueryable<Video> Items => _Context.Videos.AsNoTracking();

    public void Create(Video entity)
    {
      _Context.Videos.Add(entity);
      _Context.SaveChanges();
    }

    public void Delete(Video entity)
    {
      _Context.Videos.Remove(entity);
      _Context.SaveChanges();
    }

    public void Update(Video entity)
    {
      _Context.Videos.Update(entity);
      _Context.SaveChanges();
    }

    public bool ValidateEntity(Video entity)
    {
      return Items.Any(categoria => categoria.Id == entity.CategoriaId);
    }
  }
}