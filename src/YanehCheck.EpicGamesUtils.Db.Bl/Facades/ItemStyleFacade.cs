using YanehCheck.EpicGamesUtils.Db.Bl.Facades.Interfaces;
using YanehCheck.EpicGamesUtils.Db.Bl.Mappers.Interfaces;
using YanehCheck.EpicGamesUtils.Db.Bl.Models;
using YanehCheck.EpicGamesUtils.Db.Dal.Entities;
using YanehCheck.EpicGamesUtils.Db.Dal.Mappers;
using YanehCheck.EpicGamesUtils.Db.Dal.UnitOfWork;

namespace YanehCheck.EpicGamesUtils.Db.Bl.Facades;

public class ItemStyleFacade(IUnitOfWorkFactory unitOfWorkFactory, IModelMapper<ItemStyleEntity, ItemStyleModel> mapper) 
    : Facade<ItemStyleEntity, ItemStyleModel, ItemStyleEntityMapper>(unitOfWorkFactory, mapper), IItemStyleFacade {
}