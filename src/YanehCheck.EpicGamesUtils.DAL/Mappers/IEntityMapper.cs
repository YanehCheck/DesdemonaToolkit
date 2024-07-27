using YanehCheck.EpicGamesUtils.DAL.Entities;

namespace YanehCheck.EpicGamesUtils.DAL.Mappers;

public interface IEntityMapper<in TEntity> where TEntity : IEntity {
    void Map(TEntity originalEntity, TEntity newEntity);
}