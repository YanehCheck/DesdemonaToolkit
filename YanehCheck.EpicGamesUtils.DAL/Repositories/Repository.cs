using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using YanehCheck.EpicGamesUtils.DAL.Entities;
using YanehCheck.EpicGamesUtils.DAL.Mappers;

namespace YanehCheck.EpicGamesUtils.DAL.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity {
    private readonly DbContext _context;
    private readonly IEntityMapper<TEntity> _mapper;

    public Repository(DbContext context, IEntityMapper<TEntity> mapper) {
        _context = context;
        _mapper = mapper;
    }

    public IQueryable<TEntity> Get() => _context.Set<TEntity>();
    public async Task<TEntity?> GetByIdAsync(Guid id) => await _context.Set<TEntity>().FindAsync(id);
    public async Task<List<TEntity>> GetAllAsync() => await _context.Set<TEntity>().ToListAsync();
    public async Task<bool> ExistsAsync(TEntity entity) => await _context.Set<TEntity>().AnyAsync(e => e.Id == entity.Id);
    public async Task<bool> ExistsByIdAsync(Guid id) => await _context.Set<TEntity>().AnyAsync(e => e.Id == id);

    public async Task<TEntity> AddAsync(TEntity entity) {
        EntityEntry<TEntity> entry = await _context.Set<TEntity>().AddAsync(entity);
        return entry.Entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity) {
        TEntity originalEntity = await _context.Set<TEntity>().SingleAsync(e => e.Id == entity.Id);
        _mapper.Map(originalEntity, entity);
        return originalEntity;
    }

    public void Delete(TEntity entity) => _context.Set<TEntity>().Remove(entity);

    public void DeleteById(Guid id) => _context.Set<TEntity>().Remove(_context.Set<TEntity>().Single(e => e.Id == id));

}