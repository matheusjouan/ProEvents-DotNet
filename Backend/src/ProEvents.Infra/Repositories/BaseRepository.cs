using ProEvents.Infra.Context;
using Microsoft.EntityFrameworkCore;
using ProEvents.Infra.Interface;

namespace ProEvents.Infra.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly AppDbContext _context;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task Add(T entity)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeteleRange(T[] entityArray)
        {
            _context.RemoveRange(entityArray);
            await _context.SaveChangesAsync();
        }
    }
}