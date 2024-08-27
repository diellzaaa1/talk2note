using talk2note.Domain.Entities;
using System.Threading.Tasks;

namespace talk2note.Application.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
    }
}
