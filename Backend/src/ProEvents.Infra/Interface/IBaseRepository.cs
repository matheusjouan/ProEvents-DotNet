namespace ProEvents.Infra.Interface
{
    public interface IBaseRepository<T> where T : class
    {
        Task Add(T entity);
        Task Update(T entity);
        Task Delete(T entity);
        Task DeteleRange(T[] entity);
    }
}