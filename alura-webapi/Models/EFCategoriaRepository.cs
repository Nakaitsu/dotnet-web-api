using alura_webapi.Data;
using alura_webapi.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace alura_webapi.Models
{
  public class EFCategoriaRepository : IRepository<Categoria>
  {
    private readonly EFContext _Context;

    public IQueryable<Categoria> Items => _Context.Categorias.AsNoTracking();

    public EFCategoriaRepository(EFContext context)
    {
      _Context = context;
    }

    public void Create(Categoria entity)
    {
      _Context.Categorias.Add(entity);
      _Context.SaveChanges();
    }

    public void Delete(Categoria entity)
    {
      _Context.Categorias.Remove(entity);
      _Context.SaveChanges();
    }

    public void Update(Categoria entity)
    {
      _Context.Categorias.Update(entity);
      _Context.SaveChanges();
    }

    public bool ValidateEntity(Categoria entity)
    {
      return Items.Any(c => c.Titulo == entity.Titulo);
    }
  }
}