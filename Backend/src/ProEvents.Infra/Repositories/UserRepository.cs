using Microsoft.EntityFrameworkCore;
using ProEvents.Domain.Identity;
using ProEvents.Infra.Context;
using ProEvents.Infra.Interface;

namespace ProEvents.Infra.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            IQueryable<User> query = _context.Set<User>();

            return await query.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            IQueryable<User> query = _context.Users
                .Where(u => u.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            IQueryable<User> query = _context.Users
                .Where(u => u.UserName.ToLower() == username.ToLower());
            return await query.FirstOrDefaultAsync();
        }
    }
}