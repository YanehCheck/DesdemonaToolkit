using Microsoft.EntityFrameworkCore;
using YanehCheck.EpicGamesUtils.BL.Facades.Interfaces;
using YanehCheck.EpicGamesUtils.BL.Mappers.Interfaces;
using YanehCheck.EpicGamesUtils.BL.Models;
using YanehCheck.EpicGamesUtils.DAL.Entities;
using YanehCheck.EpicGamesUtils.DAL.Mappers;
using YanehCheck.EpicGamesUtils.DAL.UnitOfWork;

namespace YanehCheck.EpicGamesUtils.BL.Facades;

public class ItemFacade(IUnitOfWorkFactory unitOfWorkFactory, IModelMapper<ItemEntity, ItemModel> mapper) : Facade<ItemEntity, ItemModel, ItemEntityMapper>(unitOfWorkFactory, mapper), IItemFacade {
    public async Task<ItemModel> SaveAsyncByFortniteId(ItemModel model) {
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

    public async Task<IEnumerable<ItemModel>> SaveAsyncByFortniteId(IEnumerable<ItemModel> models) {
        await using var uow = UnitOfWorkFactory.Create();
        var repo = uow.GetRepository<ItemEntity, ItemEntityMapper>();
        var entities = models.Select(Mapper.MapToEntity);

        entities = await Task.WhenAll(entities.Select(async entity => {
            var fetchedEntity = await repo.Get().SingleOrDefaultAsync(e => e.FortniteId == entity.FortniteId);
            if (fetchedEntity != null) {
                entity.Id = fetchedEntity.Id;
                return await repo.UpdateAsync(entity);
            }
            else {
                entity.Id = Guid.NewGuid();
                return await repo.AddAsync(entity);
            }
        }));

        await uow.SaveChangesAsync();
        return entities.Select(Mapper.MapToModel);
    }
}   