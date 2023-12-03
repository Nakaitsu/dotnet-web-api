using alura_webapi.Data;
using alura_webapi.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace alura_webapi.Models
{
  public class EFUserRepository : IRepository<User>
  {
    private readonly EFContext _Context;

    public EFUserRepository(EFContext context)
    {
      _Context = context;
    }

    public IQueryable<User> Items => _Context.Users.AsNoTracking();

    public void Create(User entity)
    {
      _Context.Users.Add(entity);
      _Context.SaveChanges();
    }

    public void Delete(User entity)
    {
      throw new NotImplementedException();
    }

    public void Update(User entity)
    {
      throw new NotImplementedException();
    }

    public bool ValidateEntity(User entity)
    {
      return !Items.Any(u => u.Email == entity.Email);
    }

  }
}