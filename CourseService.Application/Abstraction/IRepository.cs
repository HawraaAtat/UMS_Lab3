namespace CourseService.Application.Abstraction;

public interface IRepository<TEntity> where TEntity : class, new()
{
    bool CheckIfExist(long id);
    IQueryable<TEntity> GetAll();
    //Task<string> AddAsync(TEntity entity);
    Task<string> AddAsync(TEntity entity, long tenantId);
    Task<string> UpdateAsync(TEntity entity);
    Task<string> DeleteAsync(long id);
    
}