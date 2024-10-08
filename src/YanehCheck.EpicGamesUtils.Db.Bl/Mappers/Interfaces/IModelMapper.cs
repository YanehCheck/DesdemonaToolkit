﻿namespace YanehCheck.EpicGamesUtils.Db.Bl.Mappers.Interfaces;

public interface IModelMapper<TEntity, TModel> {
    TModel MapToModel(TEntity entity);

    IEnumerable<TModel> MapToModel(IEnumerable<TEntity> entities)
        => entities.Select(MapToModel);

    TEntity MapToEntity(TModel model);
}