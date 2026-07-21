using Microsoft.EntityFrameworkCore;
using ONG_connect.Data;
using ONG_connect.Interfaces;

namespace ONG_connect.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync() =>
        await _dbSet.AsNoTracking().ToListAsync();

    public async Task<T?> GetByIdAsync(int id) =>
        await _dbSet.FindAsync(id);

    public async Task AddAsync(T entity) =>
        await _dbSet.AddAsync(entity);

    public void Update(T entity) => _dbSet.Update(entity);

    public void Remove(T entity) => _dbSet.Remove(entity);

    public async Task<bool> SaveChangesAsync() =>
        await _context.SaveChangesAsync() > 0;
}