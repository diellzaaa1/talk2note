using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using talk2note.Domain.Entities;
using talk2note.Infrastructure.Data;
using talk2note.Application.Interfaces;
using talk2note.Infrastructure.Persistence;

namespace talk2note.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly AppDbContext _context; 

        public UserRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        }
    }
}
