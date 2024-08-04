using Microsoft.EntityFrameworkCore;
using YanehCheck.EpicGamesUtils.BL.Facades.Interfaces;
using YanehCheck.EpicGamesUtils.BL.Mappers.Interfaces;
using YanehCheck.EpicGamesUtils.BL.Models.Interfaces;
using YanehCheck.EpicGamesUtils.DAL.Entities;
using YanehCheck.EpicGamesUtils.DAL.Mappers;
using YanehCheck.EpicGamesUtils.DAL.UnitOfWork;

namespace YanehCheck.EpicGamesUtils.BL.Facades;

public abstract class Facade<TEntity, TModel, TEntityMapper>(IUnitOfWorkFactory unitOfWorkFactory, IModelMapper<TEntity, TModel> mapper)
    : IFacade<TEntity, TModel>
    where TEntity : class, IEntity
    where TModel : class, IModel
    where TEntityMapper : class, IEntityMapper<TEntity>, new() {

    protected readonly IModelMapper<TEntity, TModel> Mapper = mapper;
    protected readonly IUnitOfWorkFactory UnitOfWorkFactory = unitOfWorkFactory;

    protected virtual IEnumerable<string> IncludeRelatedObjects => [];

    public async Task DeleteAsync(Guid id) {
        await using var uow = UnitOfWorkFactory.Create();
        var repo = uow.GetRepository<TEntity, TEntityMapper>();

        repo.DeleteById(id);
        await uow.SaveChangesAsync();
    }

    public async Task<IEnumerable<TModel>> GetAllAsync() {
        await using var uow = UnitOfWorkFactory.Create();
        var repo = uow.GetRepository<TEntity, TEntityMapper>();

        var entities = repo.Get();
        return entities.Select(e => Mapper.MapToModel(e)).ToList();
    }

    public async Task<TModel?> GetAsync(Guid id, bool dontIncludeRelatedObjects = false) {
        await using var uow = UnitOfWorkFactory.Create();
        var repo = uow.GetRepository<TEntity, TEntityMapper>().Get();

        if(!dontIncludeRelatedObjects) {
            foreach(var include in IncludeRelatedObjects) {
                repo = repo.Include(include);
            }
        }

        var entity = await repo.SingleOrDefaultAsync(entity => entity.Id == id);
        return entity != null ? Mapper.MapToModel(entity) : null;
    }

    public async Task<TModel> SaveAsync(TModel model) {
        await using var uow = UnitOfWorkFactory.Create();
        var repo = uow.GetRepository<TEntity, TEntityMapper>();
        var entity = Mapper.MapToEntity(model);

        // Update X Create
        if(await repo.ExistsAsync(entity)) {
            entity = await repo.UpdateAsync(entity);
        }
        else {
            entity.Id = Guid.NewGuid();
            entity = await repo.AddAsync(entity);
        }

        await uow.SaveChangesAsync();
        return Mapper.MapToModel(entity);
    }

    public async Task<bool> AnyAsync() {
        await using var uow = UnitOfWorkFactory.Create();
        var repo = uow.GetRepository<TEntity, TEntityMapper>();
        return await repo.Get().AnyAsync();
    }
}