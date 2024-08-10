using YanehCheck.EpicGamesUtils.Db.Dal.Entities;

namespace YanehCheck.EpicGamesUtils.Db.Dal.Mappers;

public interface IEntityMapper<in TEntity> where TEntity : IEntity {
    void Map(TEntity originalEntity, TEntity newEntity);
}