using Microsoft.EntityFrameworkCore;
using YanehCheck.EpicGamesUtils.Db.Bl.Facades.Interfaces;
using YanehCheck.EpicGamesUtils.Db.Bl.Mappers.Interfaces;
using YanehCheck.EpicGamesUtils.Db.Bl.Models;
using YanehCheck.EpicGamesUtils.Db.Dal.Entities;
using YanehCheck.EpicGamesUtils.Db.Dal.Mappers;
using YanehCheck.EpicGamesUtils.Db.Dal.UnitOfWork;

namespace YanehCheck.EpicGamesUtils.Db.Bl.Facades;

public class ItemStyleFacade(IUnitOfWorkFactory unitOfWorkFactory, IModelMapper<ItemStyleEntity, ItemStyleModel> mapper) 
    : Facade<ItemStyleEntity, ItemStyleModel, ItemStyleEntityMapper>(unitOfWorkFactory, mapper), IItemStyleFacade {
    public async Task<ItemStyleModel> SaveByFortniteIdAsync(ItemStyleModel model) {
        await using var uow = UnitOfWorkFactory.Create();
        var repo = uow.GetRepository<ItemStyleEntity, ItemStyleEntityMapper>();
        var entity = Mapper.MapToEntity(model);

        // Update X Create
        if(await repo.Get().AnyAsync(e => e.FortniteId == entity.FortniteId)) {
            entity = await repo.UpdateAsync(entity);
        }
        else {
            entity.Id = Guid.NewGuid();
            entity = await repo.AddAsync(entity);
        }

        await uow.SaveChangesAsync();
        return Mapper.MapToModel(entity);
    }

    public async Task<IEnumerable<ItemStyleModel>> SaveByFortniteIdAsync(IEnumerable<ItemStyleModel> models) {
        await using var uow = UnitOfWorkFactory.Create();
        var repo = uow.GetRepository<ItemStyleEntity, ItemStyleEntityMapper>();
        var query = repo.Get();
        var entities = models.Select(m => {
            var e = Mapper.MapToEntity(m);
            e.Id = Guid.NewGuid();
            return e;
        }).ToList();

        await query.ExecuteDeleteAsync();
        await repo.BulkAddOrUpdateAsync(entities);

        await uow.SaveChangesAsync();
        return entities.Select(Mapper.MapToModel);
    }


    public async Task<IEnumerable<ItemStyleModel>> GetByFortniteIdAsync(IEnumerable<ItemStyleModel> models) {
        await using var uow = UnitOfWorkFactory.Create();
        var repo = uow.GetRepository<ItemStyleEntity, ItemStyleEntityMapper>();
        var entities = models.Select(mapper.MapToEntity).ToList();
        var query = repo.BulkReadAsync(entities, [nameof(ItemStyleEntity.FortniteId)]);
        return entities.Select(Mapper.MapToModel);
    }

    public async Task<IEnumerable<ItemStyleModel>> GetByFortniteItemIdAsync(string itemFortniteId, IEnumerable<string>? validProperties = null) {
        await using var uow = UnitOfWorkFactory.Create();
        var repo = uow.GetRepository<ItemStyleEntity, ItemStyleEntityMapper>();
        var query = repo.Get().Where(e => 
            e.ItemFortniteId == itemFortniteId && 
            (validProperties == null ||validProperties.Contains(e.Property)));
        return query.Select(e => mapper.MapToModel(e)).ToList();
    }
}