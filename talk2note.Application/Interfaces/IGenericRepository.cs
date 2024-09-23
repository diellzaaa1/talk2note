namespace talk2note.Application.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        void Update(T entity);
        Task <bool> DeleteAsync(int id);
        Task<IEnumerable<T>> GetByUserIdAsync(int userId);

    }

}
