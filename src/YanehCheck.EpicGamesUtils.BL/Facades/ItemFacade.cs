using Microsoft.EntityFrameworkCore;
using YanehCheck.EpicGamesUtils.BL.Facades.Interfaces;
using YanehCheck.EpicGamesUtils.BL.Mappers.Interfaces;
using YanehCheck.EpicGamesUtils.BL.Models;
using YanehCheck.EpicGamesUtils.DAL.Entities;
using YanehCheck.EpicGamesUtils.DAL.Mappers;
using YanehCheck.EpicGamesUtils.DAL.UnitOfWork;

namespace YanehCheck.EpicGamesUtils.BL.Facades;

public class ItemFacade(IUnitOfWorkFactory unitOfWorkFactory, IModelMapper<ItemEntity, ItemModel> mapper) : Facade<ItemEntity, ItemModel, ItemEntityMapper>(unitOfWorkFactory, mapper), IItemFacade {
    public async Task<ItemModel> SaveByFortniteIdAsync(ItemModel model) {
        await using var uow = UnitOfWorkFactory.Create();
        var repo = uow.GetRepository<ItemEntity, ItemEntityMapper>();
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

    public async Task<IEnumerable<ItemModel>> SaveByFortniteIdAsync(IEnumerable<ItemModel> models) {
        await using var uow = UnitOfWorkFactory.Create();
        var repo = uow.GetRepository<ItemEntity, ItemEntityMapper>();
        var query = repo.Get();
        var entities = models.Select(m => {
            var e = Mapper.MapToEntity(m);
            e.Id = Guid.NewGuid();
            return e;
        }).ToList();

        await query.ExecuteDeleteAsync();
        await repo.BulkAddAsync(entities);

        await uow.SaveChangesAsync();
        return entities.Select(Mapper.MapToModel);
    }
    
    /// <summary>
    /// Reads items by fortnite ID.
    /// Models need to have their FortniteId set.
    /// </summary>
    public async Task<IEnumerable<ItemModel>> GetByFortniteIdAsync(IEnumerable<ItemModel> models) {
        await using var uow = UnitOfWorkFactory.Create();
        var repo = uow.GetRepository<ItemEntity, ItemEntityMapper>();
        var entities = models.Select(mapper.MapToEntity).ToList();
        var query = repo.BulkReadAsync(entities, [nameof(ItemEntity.FortniteId)]);
        return entities.Select(Mapper.MapToModel);
    }
}   