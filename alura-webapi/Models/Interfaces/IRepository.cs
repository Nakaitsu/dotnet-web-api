namespace alura_webapi.Models.Interfaces
{
  public interface IRepository<T> where T : class
  {
    IQueryable<T> Items { get; }

    bool ValidateEntity(T entity);

    void Create(T entity);

    void Update(T entity);

    void Delete(T entity);
  }
}