﻿using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using YanehCheck.EpicGamesUtils.Db.Dal.Entities;
using YanehCheck.EpicGamesUtils.Db.Dal.Mappers;

namespace YanehCheck.EpicGamesUtils.Db.Dal.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity {
    private readonly DbContext context;
    private readonly IEntityMapper<TEntity> mapper;
    private readonly DbSet<TEntity> set;

    public Repository(DbContext context, IEntityMapper<TEntity> mapper) {
        this.context = context;
        this.mapper = mapper;
        set = this.context.Set<TEntity>();
    }

    public IQueryable<TEntity> Get() => set;
    public async Task<TEntity?> GetByIdAsync(Guid id) => await set.FindAsync(id);
    public async Task<List<TEntity>> GetAllAsync() => await set.ToListAsync();
    public async Task<bool> ExistsAsync(TEntity entity) => await set.AnyAsync(e => e.Id == entity.Id);
    public async Task<bool> ExistsByIdAsync(Guid id) => await set.AnyAsync(e => e.Id == id);

    public async Task<TEntity> AddAsync(TEntity entity) {
        EntityEntry<TEntity> entry = await set.AddAsync(entity);
        return entry.Entity;
    }

    public async Task BulkAddOrUpdateAsync(IEnumerable<TEntity> entity) {
        await context.BulkInsertOrUpdateAsync(entity);
    }

    public async Task BulkReadAsync(IEnumerable<TEntity> entities, List<string> byProperties = null) {
        await context.BulkReadAsync(entities, new BulkConfig {
            UpdateByProperties = byProperties,
            ReplaceReadEntities = true
        });
    }

    public async Task<TEntity> UpdateAsync(TEntity entity) {
        TEntity originalEntity = await set.SingleAsync(e => e.Id == entity.Id);
        mapper.Map(originalEntity, entity);
        return originalEntity;
    }

    public void Delete(TEntity entity) => set.Remove(entity);

    public void DeleteById(Guid id) => set.Remove(set.Single(e => e.Id == id));

}